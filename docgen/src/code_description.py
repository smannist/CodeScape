from typing_extensions import Annotated, TypedDict, List, Optional, Union
from langchain_core.prompts import ChatPromptTemplate
from langchain.chains.combine_documents import create_stuff_documents_chain
from langchain_core.documents import Document
from parsing import parse_code_file, get_definitions


class FileDoc():
    """Contains the documentation for a single source code file"""

    def __init__(self, **kwargs):
        self.name = kwargs.get("name","") # String
        self.overview = kwargs.get("overview", "") # String
        self.functions = kwargs.get("funcs", []) # List[FunctionDescription]
        self.classes = kwargs.get("classes", []) # List[ClassDescription]
    
    def get_methods(self):
        return [m for c in self.classes if "functions" in c for m in c.get("functions", [])]

    def __count_params(functions):
        return sum([len(func["params"]) for func in functions if "params" in func])

    def count_method_params(self):
        return FileDoc.__count_params(self.get_methods())

    def count_function_params(self):
        return FileDoc.__count_params(self.functions)

    def as_dict(self):
        return {
            'name': self.name,
            'overview': self.overview,
            'functions': self.functions,
            'classes': self.classes,
        }


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

def generate_file_overview(llm, classes, funcs):
    prompt = ChatPromptTemplate.from_messages(
        [("system", "Given the following class and function descriptions, describe the purpose of this source code file. Start your answer with 'This file':\n\n{context}")]
    )
    chain = create_stuff_documents_chain(llm, prompt)
    func_docs = [Document(page_content="function: " + str(func)) for func in funcs]
    class_docs = [Document(page_content="class: " + str(class_)) for class_ in classes]
    return chain.invoke({"context": class_docs + func_docs})

def describe_file(llm, filepath) -> FileDoc:
    (src, tree) = parse_code_file(filepath)

    funcs = get_definitions(src, tree, definition_type="function_definition")
    classes = get_definitions(src, tree, definition_type="class_definition")

    structured_llm = llm.with_structured_output(FullDescription)

    func_descriptions = [structured_llm.invoke(
        func_desc)["description"] for func_desc in funcs]
    class_descriptions = [structured_llm.invoke(
        class_desc)["description"] for class_desc in classes]
    
    overview = generate_file_overview(llm, class_descriptions, func_descriptions)
    return FileDoc(name=filepath, classes=class_descriptions, funcs=func_descriptions, overview=overview)


def describe_file_(llm, filepath) -> FileDoc:
    parsed_code = parse_code_file(filepath)
    classes = describe_classes(llm, parsed_code)
    funcs = describe_funcs(llm, parsed_code)
    overview = generate_file_overview(llm, classes, funcs)
    return FileDoc(name=filepath, classes=classes, funcs=funcs, overview=overview)

