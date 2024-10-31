class_struct_example = """

    Your task is to describe the given class, including all of the class methods, from the given source code. Do not add any additional fields. Do not include constructors from inherited classes.
    Return the description in JSON format.

    Here are some example descriptions of Python classes and their methods:

    Given class:

    class RepoDoc():

        def __init__(self, **kwargs):
            self.overview = kwargs.get("overview", "") # String
            self.files = kwargs.get("files", []) # List[FileDoc]
            self.code_graph = kwargs.get("code_graph", None) # networkx.Graph

        def as_dict(self):
            return {{
                "overview": self.overview,
                "files": [file.as_dict() for file in self.files],
                "code_graph": graph_to_dict(self.code_graph),
            }}

    Your description:

    {{
    "name": "RepoDoc",
    "description": "Contains the documentation for the whole repo",
    "methods": [
        {{
        "name": "__init__",
        "description": "Initializes the RepoDoc class with optional parameters",
        "params": [
            {{
            "name": "overview",
            "description": "String parameter for the overview of the repo"
            }},
            {{
            "name": "files",
            "description": "List of FileDoc objects representing the files in the repo"
            }},
            {{
            "name": "code_graph",
            "description": "networkx.Graph object representing the code structure"
            }}
        ],
        "returns": "An instance of RepoDoc"
        }},
        {{
        "name": "as_dict",
        "description": "Converts the RepoDoc instance into a dictionary format",
        "returns": "A dictionary representation of the RepoDoc instance"
        }}
    ]
    }}

    Given class:

    class FileDoc():

    def __init__(self, **kwargs):
        self.name = kwargs.get("name","") # String
        self.overview = kwargs.get("overview", "") # String
        self.functions = kwargs.get("funcs", []) # List[FunctionDescription]
        self.classes = kwargs.get("classes", []) # List[ClassDescription]

    def get_methods(self):
        return [m for c in self.classes if "methods" in c for m in c.get("methods", [])]

    def __count_params(functions):
        return sum([len(func["params"]) for func in functions if "params" in func])

    def count_method_params(self):
        return FileDoc.__count_params(self.get_methods())

    def count_function_params(self):
        return FileDoc.__count_params(self.functions)

    def as_dict(self):
        return {{
            'name': self.name,
            'overview': self.overview,
            'functions': self.functions,
            'classes': self.classes,
        }}

    Your description:

    {{
      "name": "FileDoc",
      "description": "Contains the documentation for a single source code file",
      "methods": [
        {{
          "name": "__init__",
          "description": "Initializes the FileDoc class with optional parameters",
          "params": [
            {{
              "name": "name",
              "description": "String parameter for the name of the file"
            }},
            {{
              "name": "overview",
              "description": "String parameter for the overview of the file"
            }},
            {{
              "name": "functions",
              "description": "List of FunctionDescription objects representing the functions in the file"
            }},
            {{
              "name": "classes",
              "description": "List of ClassDescription objects representing the classes in the file"
            }}
          ],
          "returns": "An instance of FileDoc"
        }},
        {{
          "name": "get_methods",
          "description": "Returns a list of methods from the classes in the file",
          "returns": "A list of methods from the classes in the file"
        }},
        {{
          "name": "__count_params",
          "description": "Counts the number of parameters in a list of functions or methods",
          "params": [
            {{
              "name": "functions",
              "description": "List of functions or methods"
            }}
          ],
          "returns": "The total number of parameters"
        }},
        {{
          "name": "count_method_params",
          "description": "Counts the number of parameters in the methods of the classes in the file",
          "returns": "The total number of parameters"
        }},
        {{
          "name": "count_function_params",
          "description": "Counts the number of parameters in the functions in the file",
          "returns": "The total number of parameters"
        }},
        {{
          "name": "as_dict",
          "description": "Converts the FileDoc instance into a dictionary format",
          "returns": "A dictionary representation of the FileDoc instance"
        }}
      ]
    }},

    """
