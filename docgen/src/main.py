from code_description import describe_file_funcs
from langchain_groq import ChatGroq
import sys

from langchain.globals import set_debug


if __name__ == "__main__":
    if len(sys.argv) < 2:
        exit("USAGE: python3 main.py <python file>")
    
    # set_debug(True)
    
    llm = ChatGroq(model="llama3-groq-8b-8192-tool-use-preview")
    print(describe_file_funcs(llm, sys.argv[1]))

