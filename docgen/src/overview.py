from langchain.chains.combine_documents import create_stuff_documents_chain
from langchain_core.documents import Document
from langchain_core.prompts import ChatPromptTemplate

def generate_file_overview(llm, classes, funcs):
    prompt = ChatPromptTemplate.from_messages([
        (
            "system",
            "Given the following class and function descriptions, "
            "describe the purpose of this source code file. Start your "
            "answer with 'This file':\n\n{context}"
        )
    ])
    chain = create_stuff_documents_chain(llm, prompt)
    func_docs = [Document(page_content="function: " + str(func)) for func in funcs]
    class_docs = [Document(page_content="class: " + str(class_)) for class_ in classes]
    return chain.invoke({"context": class_docs + func_docs})

def generate_repository_overview(llm, json_data):
    combined_overview = " ".join(
        overview.get(
            "overview",
            "") for overview in json_data if overview.get(
            "overview",
            ""))

    if not combined_overview:
        return "No overview available."

    prompt = ChatPromptTemplate.from_template(
        "Given the following file overviews, create overview for a git repository. "
        "The overview should only contain a brief description of what the repository is all about. "
        "Maximum length of 100 words. Start your overview with 'This repository'.\n\n{overview}")

    prompt.format(overview=combined_overview)

    llm = prompt | llm
    response = llm.invoke(combined_overview)

    return response.content
