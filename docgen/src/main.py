import sys
import os
from code_description import describe_file_funcs, describe_file_classes, describe_file
from util import save_to_json
from langchain_groq import ChatGroq
from langchain.globals import set_debug
from dotenv import load_dotenv

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
    result = describe_file(llm, sys.argv[1])

    filename = "output.json"
    save_to_json(result, filename)

    print(f"Finished and saved to {filename}")
