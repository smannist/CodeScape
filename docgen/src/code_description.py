from typing_extensions import Annotated, TypedDict, List, Optional, Union
from langchain_core.prompts import ChatPromptTemplate
from parsing import parse_code_file, get_definitions


class FunctionParam(TypedDict):
    name: Annotated[str, ..., "variable identifier"]
    description: Annotated[str, ..., "purpose of the variable"]


class FunctionDescription(TypedDict):
    name: Annotated[str, ..., "identifier of the function"]
    params: Annotated[Optional[List[FunctionParam]],
                      [], "the arguments passed to the function"]
    description: Annotated[str, ...,
                           "short description of what the function does (<5 sentences)"]


class ClassDescription(TypedDict):
    name: Annotated[str, ..., "identifier of the class"]
    functions: Annotated[Optional[List[FunctionDescription]], [
    ], "the functions defined in the class, always including function params if present"]
    description: Annotated[str, ...,
                           "short description of what the class does (<5 sentences)"]


class FullDescription(TypedDict):
    type: Annotated[str, ...,
                    "'function' or 'class' to identify the description type, "
                    "always including the whole description, function descriptions "
                    "as well as function params if present"]
    description: Union[FunctionDescription, ClassDescription]


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

def describe_file(llm, filepath):
    (src, tree) = parse_code_file(filepath)

    funcs = get_definitions(src, tree, definition_type="function_definition")
    classes = get_definitions(src, tree, definition_type="class_definition")

    structured_llm = llm.with_structured_output(FullDescription)

    func_descriptions = [structured_llm.invoke(
        func_desc) for func_desc in funcs]
    class_descriptions = [structured_llm.invoke(
        class_desc) for class_desc in classes]

    return class_descriptions + func_descriptions


def describe_file_(llm, filepath):
    parsed_code = parse_code_file(filepath)
    classes = [{"type": "class", "description": desc} for desc in describe_classes(llm, parsed_code)]
    funcs = [{"type": "function", "description": desc} for desc in describe_funcs(llm, parsed_code)]
    return classes + funcs

