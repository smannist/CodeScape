from langchain_core.prompts import PromptTemplate, FewShotPromptTemplate

class ClassFewShotPromptBuilder:
    def __init__(
        self,
        examples=None,
        example_format=None,
        prefix=None,
        suffix=None,
        input_variables=None,
        example_prompt=None,
    ):
        self.examples = examples or [
            {
                "input":
                """
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

                def __count_elements(self, count_type):
                    module_count = len(self.modules)
                    function_count = sum(len(module.functions) for module in self.modules)
                    if count_type == "module":
                        return module_count
                    elif count_type == "function":
                        return function_count
                    else:
                        raise ValueError("Invalid count_type specified. Use 'module' or 'function'.")
                """,
                "description":
                """
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
                            "returns": "The total number of functions or modules"
                        }}
                    ]
                }}
                """
            },
            {
                "input":
                """
                class HelloWorld:

                def hello(self):
                    print("Hello, World!")
                """,
                "description":
                """
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
                """
            },
            {
                "input":
                """
                class Token(TypedDict):
                loc: L
                type: str | None
                text: str
                """,
                "description":
                """
                {{
                "name": "Token",
                "description": "A TypedDict representing a token with attributes for location, type, and text, typically used in parsing or lexical analysis.",
                "methods": []
                }}
                """
            }
        ]

        self.example_format = example_format or """
        Given class: {input}
        Your description: {description}
        """

        self.prefix = prefix or """
        Your task is to describe the given class, including all of its class methods regardless of visibility (i.e., __count_elements should be included) from the provided source code.
        Return your answer as JSON.

        - Do not include constructors inherited from a superclass.
        - Only include methods that are explicitly defined in the class.
        - Do not add any additional fields such as 'type' or 'default' in your description.
        - Do not treat attributes as parameters.
        - Do not include any of the given examples in your final JSON output.
        - If a method returns a print statement, do not include a "returns" field in its description.

        Here are some example classes and returned descriptions:
        """

        self.suffix = suffix or """
        Given class: {input}
        Your description:
        """

        self.input_variables = input_variables or ["input"]

        self.example_prompt = example_prompt or PromptTemplate(
            input_variables=["input", "description"],
            template=self.example_format,
        )

    def create_prompt(self):
        prompt = FewShotPromptTemplate(
            examples=self.examples,
            example_prompt=self.example_prompt,
            suffix=self.suffix,
            prefix=self.prefix,
            input_variables=self.input_variables
        )
        return prompt
