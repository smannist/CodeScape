import sys
import os
from code_description import describe_file_funcs, describe_file_classes, describe_file, describe_file_
from util import save_to_json
from langchain_groq import ChatGroq
from langchain.globals import set_debug
from dotenv import load_dotenv

def get_stats(result):
    funcs = []
    classes = []
    result_contains = lambda key: len([x for x in result if key in x]) > 0
    if result_contains("type"):
        # FullDescription
        classes = [x for x in result if x["type"] == "class"]
        funcs = [x for x in result if x["type"] == "function"]
    if result_contains("functions"):
        # ClassDescription
        classes = result
    else:
        # FunctionDescription
        funcs = result
    methods = [f for c in classes if "functions" in c for f in c.get("functions", [])]
    func_arg_count = sum([len(f["params"]) for f in funcs if "params" in f])
    method_arg_count = sum([len(f["params"]) for f in methods if "params" in f])
    return {"func_arg_count":func_arg_count,
            "method_arg_count":method_arg_count,
            "class_count": len(classes),
            "method_count": len(methods),
            "func_count": len(funcs)
            }

def print_stats(result):
    stats = get_stats(result)
    print("# Stats")
    print("classes:", stats["class_count"])
    print("  methods:", stats["class_count"])
    print("    params:", stats["method_arg_count"])
    print("functions:", stats["func_count"])
    print("  params:", stats["func_arg_count"])


load_dotenv()

if __name__ == "__main__":
    if len(sys.argv) < 2:
        exit("USAGE: python3 main.py <python file>")

    # set_debug(True)
    llm = ChatGroq(
        model="llama3-groq-8b-8192-tool-use-preview",
        api_key=os.getenv("API_KEY")
    )

    # for now: describe_file_funcs, describe_file_classes or describe_file
    result = describe_file_(llm, sys.argv[1])

    filename = "output.json"
    save_to_json(result, filename)
    print_stats(result)
    print(f"\nFinished and saved to {filename}")
