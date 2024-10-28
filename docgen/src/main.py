import sys
import os
from code_description import describe_file_funcs, describe_file_classes, describe_file, describe_file_
from overview import generate_repository_overview
from util import save_to_json, load_json
from langchain_groq import ChatGroq
from langchain.globals import set_debug
from dotenv import load_dotenv


def print_stats(filedoc):
    print("# Stats")
    print("classes:", len(filedoc.classes))
    print("  methods:", len(filedoc.get_methods()))
    print("    params:", filedoc.count_method_params())
    print("functions:", len(filedoc.functions))
    print("  params:", filedoc.count_function_params())


load_dotenv()

if __name__ == "__main__":
    if len(sys.argv) < 2:
        exit("USAGE: python3 main.py <python file>")

    #To test repo overview: python3 main.py repo     (note: expects output.json to exist)
    if "repo" in sys.argv:
        llm = ChatGroq(
            model="llama3-groq-8b-8192-tool-use-preview",
            api_key=os.getenv("API_KEY")
        )
        file = load_json("output.json")
        if any("overview" in file_data for file_data in file):
            summary = generate_repository_overview(llm, file)
            print(
                f"\nGenerated the following repo summary based on the overview(s):\n{summary}")
            exit()

    # set_debug(True)
    llm = ChatGroq(
        model="llama3-groq-8b-8192-tool-use-preview",
        api_key=os.getenv("API_KEY")
    )

    # for now: describe_file_funcs, describe_file_classes or describe_file
    result = describe_file_(llm, sys.argv[1])

    filename = "output.json"
    save_to_json([result.as_dict()], filename)
    print_stats(result)
    print(f"\nFinished and saved to {filename}")
