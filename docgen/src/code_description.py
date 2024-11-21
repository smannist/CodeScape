import os
from typing_extensions import Annotated, TypedDict, List, Optional
from langchain_core.prompts import ChatPromptTemplate
from parsing import parse_code_file, get_definitions, parse_python_imports
from code_documentation import FileDoc
from overview import generate_file_overview
from builders import ClassFewShotPromptBuilder

class Param(TypedDict):
    name: Annotated[
        str,
        ...,
        "variable identifier"
    ]
    description: Annotated[
        str,
        ...,
        "purpose of the variable"
    ]


class FunctionDescription(TypedDict):
    name: Annotated[
        str,
        ...,
        "identifier of the function"
    ]
    params: Annotated[
        Optional[List[Param]],
        [],
        "the arguments passed to the function"
    ]
    description: Annotated[
        str,
        ...,
        "short description of what the function does (<5 sentences)"
    ]
    returns: Annotated[
        Optional[str],
        ...,
        "A maximum of 70 character long description of the value returned by the function"
    ]


class ClassDescription(TypedDict):
    name: Annotated[
        str,
        ...,
        "identifier of the class"
    ]
    methods: Annotated[
        Optional[List[FunctionDescription]],
        [],
        "the methods defined in the class, including their parameters and return values, if present"
    ]
    description: Annotated[
        str,
        ...,
        "short description of what the class does (<5 sentences)"
    ]


def describe_file_funcs(llm, filepath):
    return describe_funcs(llm, parse_code_file(filepath))

def pass_batch_to_llm(llm, batch, fallback_func):
    """Invokes llm for each value in batch and returns the transformed list.
    If an exception occurs, the value is instead computed with fallback_func"""
    results = []
    for data in batch:
        try:
            results.append(llm.invoke(data))
        except:
            results.append(fallback_func(data))
    return results


def __get_fallback_func_describer(llm):
    "Given an llm, returns a fallback function that tries to get just the name and description of a function without structured output"
    def fallback_func(func):
        description = llm.invoke("Describe this function with 5 sentences or less:\n" + func).content
        name = llm.invoke("Specify the name of the following function. Answer with only the name:\n" + func).content
        return {"name": name, "description": description, "error": "main prompt failed"}
    return fallback_func


def __get_fallback_class_describer(llm):
    "Given an llm, returns a fallback function that tries to get just the name and description of a class without structured output"
    def fallback_func(class_):
        class_ = class_["input"]
        description = llm.invoke("Describe this class with 5 sentences or less:\n" + class_).content
        name = llm.invoke("Specify the name of the following class. Answer with only the name:\n" + class_).content
        return {"name": name, "description": description, "error": "main prompt failed"}
    return fallback_func

def describe_funcs(llm, tree_reader):
    system = "Your task is to describe functions in source code. Always include the arguments of the function if present."
    prompt = ChatPromptTemplate.from_messages([("system", system), ("human", "{input}")])

    # The with_structured_output internally does llm tool use, which fails with some inputs, like parsing.py
    # however it seems to work with the groq models that are made for tool use
    # related issue:
    # https://github.com/langchain-ai/langchain/discussions/24309
    func_llm = prompt | llm.with_structured_output(FunctionDescription)
    return pass_batch_to_llm(func_llm, tree_reader.get_functions(), __get_fallback_func_describer(llm))

def describe_file_classes(llm, filepath):
    return describe_classes(llm, parse_code_file(filepath))


def describe_classes(llm, parsed_code):
    (src, tree) = parsed_code
    builder = ClassFewShotPromptBuilder()
    prompt = builder.create_prompt()
    few_shot_llm = prompt | llm.with_structured_output(
        ClassDescription, method="json_mode")
    inputs = [{'input': given_class} for given_class in tree_reader.get_classes()]
    return pass_batch_to_llm(few_shot_llm, inputs, __get_fallback_class_describer(llm))


def describe_file(llm, filepath, include_funcs=True,
        include_classes=True, include_overview=True,
        include_imports=True) -> FileDoc:
    tree_reader = parse_code_file(filepath)
    filedoc_args = {"name": filepath, "classes": [], "funcs": [], "imports": []}
    if include_imports:
        base_path = os.path.split(filepath)[0]
        filedoc_args["imports"] = parse_python_imports(tree_reader.get_tree(), base_path) 
    if include_classes:
        filedoc_args["classes"] = describe_classes(llm, tree_reader)
    if include_funcs:
        filedoc_args["funcs"] = describe_funcs(llm, tree_reader)
    if include_overview:
        filedoc_args["overview"] = generate_file_overview(llm, filedoc_args["classes"], filedoc_args["funcs"])
    return FileDoc(**filedoc_args)
