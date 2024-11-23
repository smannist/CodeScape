import tree_sitter_python
import tree_sitter_c_sharp
import os
from tree_sitter import Language, Parser


def traverse_top_level(tree):
    "Yields children of translation_unit in syntax tree"
    cursor = tree.walk()
    cursor.goto_first_child()
    while True:
        yield cursor.node
        if not cursor.goto_next_sibling():
            break


def traverse_tree(tree): #From tree-sitter examples
    "Yields all nodes of a syntax tree"
    cursor = tree.walk()
    visited_children = False
    while True:
        if not visited_children:
            yield cursor.node
            if not cursor.goto_first_child():
                visited_children = True
        elif cursor.goto_next_sibling():
            visited_children = False
        elif not cursor.goto_parent():
            break


def get_nodes_of_type(tree, node_type, recursive=False):
    traversal = traverse_tree if recursive else traverse_top_level
    return [node for node in traversal(tree) if node.type == node_type]


def node_to_str(node):
    return node.text.decode("utf-8")


def get_definitions(tree, definition_type):
    return [node_to_str(node) for node in get_nodes_of_type(tree, definition_type, False)]


class TreeReader:
    "This class combines together syntax tree and information about how it can be searched for specific features"

    def __init__(self, tree):
        self.tree = tree

    def get_functions(self):
        return get_definitions(self.tree, definition_type="function_definition")

    def get_classes(self):
        return get_definitions(self.tree, definition_type="class_definition")

    def get_imports(self, base_path):
        """Given a base_path, returns a list of names of python files imported in self.tree.
        Only python files within base_path are considered"""
        imports = get_nodes_of_type(self.tree, "import_statement", recursive=True)
        import_names = [node.child_by_field_name("name") for node in imports]
        imports_from = get_nodes_of_type(self.tree, "import_from_statement", recursive=True)
        import_names += [node.child_by_field_name("module_name") for node in imports_from]
        import_files = []
        for module in import_names:
            path = os.path.join(base_path, node_to_str(module).replace('.', '/') + ".py")
            if os.path.exists(path):
                import_files.append(path)
        return import_files


    def get_tree(self):
        return self.tree

class PythonTreeReader(TreeReader):
    pass # the base class works for python

class CSharpTreeReader(TreeReader):
    def get_classes(self):
        return get_definitions(self.tree, definition_type="class_declaration")


__EXTENSION_TO_LANGUAGE = {
    ".py": (tree_sitter_python, PythonTreeReader),
    ".cs": (tree_sitter_c_sharp, CSharpTreeReader),
}


def parse_code_file(filepath):
    extension = filepath[filepath.rfind('.'):]
    if extension in __EXTENSION_TO_LANGUAGE:
        (lang_package, tree_reader) = __EXTENSION_TO_LANGUAGE[extension]
        parser = Parser(Language(lang_package.language()))
        with open(filepath, "rb") as f:
            src = f.read()
        tree = parser.parse(src)
        return tree_reader(tree)
    return None


def is_file_parseable(filepath):
    extension = filepath[filepath.rfind('.'):]
    return extension in __EXTENSION_TO_LANGUAGE


