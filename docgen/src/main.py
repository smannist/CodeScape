import os
from code_description import describe_file_funcs, describe_file_classes, describe_file
from code_documentation import RepoDoc
from overview import generate_repository_overview
from util import save_to_json, load_json, list_files_if
from cli import parse_cli_args
from similarity_graph import create_code_graph, plot_graph
from langchain_groq import ChatGroq
from langchain.globals import set_debug
from dotenv import load_dotenv

def describe_repo(llm, cli_args):
    code_files = list_files_if(cli_args["repo_path"], lambda name: name.endswith(".py"))

    code_docs = [describe_file(llm, file, 
        include_funcs="function" in cli_args["types"],
        include_classes="class" in cli_args["types"],
        include_overview="file" in cli_args["types"]
    ) for file in code_files]
    
    result = {"files": code_docs}
    if "repo" in cli_args["types"]:
        result["overview"] = generate_repository_overview(llm, [doc.as_dict() for doc in code_docs])
    if "code-graph" in cli_args["types"]:
        result["code_graph"] = create_code_graph(code_docs)
    return RepoDoc(**result)

def print_stats(repo_doc):
    def print_sum(text, func):
        print(text, sum([func(file) for file in repo_doc.files]))
    print("# Stats")
    print("files:", len(repo_doc.files))
    print_sum("classes:", lambda filedoc : len(filedoc.classes))
    print_sum("  methods:", lambda filedoc : len(filedoc.get_methods()))
    print_sum("    params:", lambda filedoc : filedoc.count_method_params())
    print_sum("functions:", lambda filedoc : len(filedoc.functions))
    print_sum("  params:", lambda filedoc : filedoc.count_function_params())


if __name__ == "__main__":
    load_dotenv()
    cli_args = parse_cli_args()
    llm = ChatGroq(model=cli_args["model"], api_key=os.getenv("API_KEY"), temperature=0.0)
    repo_doc = describe_repo(llm, cli_args)
    save_to_json(repo_doc.as_dict(), cli_args["output"])
    print_stats(repo_doc)
    print(f"\nFinished and saved to {cli_args['output']}")
    if cli_args["plot_graph"]:
       plot_graph(repo_doc.code_graph) 



