
from argparse import ArgumentParser

def get_supported_types():
    "Gets list of recognized documentation types"
    return ["function", "class", "file", "repo", "code-graph", "imports"]

def validate_doc_types(types):
    "Converts requested documentation types to a set, and changes the types based on the dependencies between types"
    if "all" in types:
        return set(get_supported_types())
    result = set(types)
    
    # Repo overview requires file overviews
    if "repo" in result: 
        result.add("file")

    # File overview and code graph need functions, classes or both
    if "file" in result or"code-graph" in result:
        if "class" not in result and "function" not in result:
            result.add("class")
            result.add("function")
    return result
    

def parse_cli_args():
    "Parses command-line arguments and returns them as a dictionary"
    doc_types = ["all"] + get_supported_types()
    example = "example:\ndocgen . -o docs/generated.json -t function class"
    parser = ArgumentParser(prog="Docgen", description="Generates repository documentation", epilog=example)

    parser.add_argument("repo_path", help="path to repository or file that will be documented")
    parser.add_argument("-m", "--model", default="llama-3.2-90b-vision-preview",
            help="used groq model ID (https://console.groq.com/docs/models)")
    parser.add_argument("-o", "--output", default="output.json",
            help="path to the json file that the tool will generate")
    parser.add_argument("-t", "--types", nargs="+", default=["all"], choices=doc_types,
            help="list of documentation types that will be added to output")
    parser.add_argument("--plot-graph", action="store_true", help="create a plot of generated code_graph")
    
    args = vars(parser.parse_args())
    args["types"] = validate_doc_types(args["types"])
    return args

