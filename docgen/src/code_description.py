from typing_extensions import Annotated, TypedDict, List, Optional
from parsing import parse_code_file, get_tree_funcs



class FunctionParam(TypedDict):
    name : Annotated[str, ..., "variable identifier"]
    description: Annotated[str, ..., "purpose of the variable"]


class FunctionDescription(TypedDict):
    name: Annotated[str, ..., "identifier of the function"]
    params: Annotated[Optional[List[FunctionParam]], [], "the arguments passed to the function"]
    description: Annotated[str, ..., "short description of what the function does (<5 sentences)"]

def describe_file_funcs(llm, filepath):
    (src, tree) = parse_code_file(filepath)

    # The with_structured_output internally does llm tool use, which fails with some inputs, like parsing.py
    # however it seems to work with the groq models that are made for tool use
    # related issue: https://github.com/langchain-ai/langchain/discussions/24309
    func_llm = llm.with_structured_output(FunctionDescription)
    return [func_llm.invoke(func) for func in get_tree_funcs(src, tree)]
