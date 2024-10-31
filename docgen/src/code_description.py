from typing_extensions import Annotated, TypedDict, List, Optional, Union
from langchain_core.prompts import ChatPromptTemplate
from parsing import parse_code_file, get_definitions
from code_documentation import FileDoc
from overview import generate_file_overview

class Param(TypedDict):
    name: Annotated[str, ..., "variable identifier"]
    description: Annotated[str, ..., "purpose of the variable"]


class FunctionDescription(TypedDict):
    name: Annotated[str, ..., "identifier of the function"]
    params: Annotated[
        Optional[List[Param]],
        [], "the arguments passed to the function, always include function params if present"
    ]
    description: Annotated[
        str, ...,
        "short description of what the function does (<5 sentences)"
    ]
    returns: Annotated[
        Optional[str], ...,
        "the value returned by the function and one sentence description in format "
        "(value: description), for example: 'dict: A dictionary representation of "
        "the RepoDoc object' or 'int: the number of parameters in a list of functions'"
    ]


class ClassDescription(TypedDict):
    name: Annotated[str, ..., "identifier of the class"]
    functions: Annotated[Optional[List[FunctionDescription]], [
    ], "the functions defined in the class, always including function params if present"]
    description: Annotated[str, ...,
                           "short description of what the class does (<5 sentences)"]


def describe_file_funcs(llm, filepath):
    return describe_funcs(llm, parse_code_file(filepath))

def describe_funcs(llm, parsed_code):
    (src, tree) = parsed_code
    system = "Your task is to describe functions in source code. Always include the arguments of the function if present."
    prompt = ChatPromptTemplate.from_messages([("system", system), ("human", "{input}")])

    # The with_structured_output internally does llm tool use, which fails with some inputs, like parsing.py
    # however it seems to work with the groq models that are made for tool use
    # related issue:
    # https://github.com/langchain-ai/langchain/discussions/24309
    func_llm = prompt | llm.with_structured_output(FunctionDescription)
    return [func_llm.invoke(func) for func in get_definitions(src, tree, definition_type="function_definition")]

def describe_file_classes(llm, filepath):
    return describe_classes(llm, parse_code_file(filepath))

def describe_classes(llm, parsed_code):
    (src, tree) = parsed_code
    func_llm = llm.with_structured_output(ClassDescription)
    return [func_llm.invoke(func) for func in get_definitions(src, tree, definition_type="class_definition")]

def describe_file(llm, filepath, include_funcs=True, include_classes=True, include_overview=True) -> FileDoc:
    parsed_code = parse_code_file(filepath)
    filedoc_args = {"name": filepath, "classes": [], "funcs": []}
    if include_classes:
        filedoc_args["classes"] = describe_classes(llm, parsed_code)
    if include_funcs:
        filedoc_args["funcs"] = describe_funcs(llm, parsed_code)
    if include_overview:
        filedoc_args["overview"] = generate_file_overview(llm, filedoc_args["classes"], filedoc_args["funcs"])
    return FileDoc(**filedoc_args)
