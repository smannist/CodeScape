from similarity_graph import graph_to_dict

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

class RepoDoc():
    "Contains the documentation for the whole repo"

    def __init__(self, **kwargs):
        self.overview = kwargs.get("overview", "") # String
        self.files = kwargs.get("files", []) # List[FileDoc]
        self.code_graph = kwargs.get("code_graph", None) # networkx.Graph

    def as_dict(self):
        return {
            "overview": self.overview,
            "files": [file.as_dict() for file in self.files],
            "code_graph": graph_to_dict(self.code_graph),
        }
