class_struct_example = """

   Your task is to describe the given class, including all of its class methods regardless of visibility (i.e., __count_elements should be included) from the provided source code.
    - Do **not** include constructors inherited from a superclass. 
    - Only include methods that are explicitly defined in the class.
    - Do not add any additional fields.
    - Do not treat attributes as parameters.
    - If a method returns a print statement, do not include a "returns" field in its description.

    Here are some example descriptions of Python classes and their methods:

    class LibraryDoc:

        def __init__(self, **kwargs):
            self.name = kwargs.get("name", "")
            self.modules = kwargs.get("modules", [])
            self.license = kwargs.get("license", "MIT")

        def as_dict(self):
            return {{
                "name": self.name,
                "modules": [module.as_dict() for module in self.modules],
                "license": self.license,
            }}

        def __count_elements(self):
            module_count = len(self.modules)
            function_count = sum(len(module.functions) for module in self.modules)
            if count_type == "module":
                return module_count
            elif count_type == "function":
                return function_count
            else:
                raise ValueError("Invalid count_type specified. Use 'module' or 'function'.")

    Your description:

    {{
        "name": "LibraryDoc",
        "description": "Stores documentation details for a library, including modules and their functions.",
        "methods": [
            {{
                "name": "__init__",
                "description": "Initializes the LibraryDoc class with optional parameters.",
                "params": [
                    {{
                        "name": "name",
                        "description": "String representing the library's name."
                    }},
                    {{
                        "name": "modules",
                        "description": "List of ModuleDoc objects representing the modules in the library."
                    }},
                    {{
                        "name": "license",
                        "description": "String representing the type of license used by the library."
                    }}
                ],
                "returns": "An instance of LibraryDoc"
            }},
            {{
                "name": "as_dict",
                "description": "Converts the LibraryDoc instance into a dictionary format.",
                "returns": "A dictionary representation of the LibraryDoc instance."
            }},
            {{
                "name": "__count_elements",
                "description": "Counts key elements within the LibraryDoc instance, returning an integer count based on the specified count_type ('module' or 'function').",
                "params": [
                    {{
                        "name": "count_type",
                        "description": "String specifying the count type to return; either 'module' for module count or 'function' for function count."
                    }}
                ],
                "returns": "Total number of functions or modules"
            }},
        ]
    }}

    Given class:

    class HelloWorld:
        def hello(self):
            print("Hello, World!")
    
    Your description:

    {{
    "name": "HelloWorld",
    "description": "A simple class that prints a greeting",
    "methods": [
        {{
        "name": "hello",
        "description": "Prints a hello message"
        }}
      ]
    }}

    Given class:

    class Token:
        loc: L
        type: str | None
        text: str

    Your description:

    {{
    "name": "Token",
    "description": "Represents a token with location, type, and text attributes, typically used in parsing or lexical analysis.",
    }}

    """
