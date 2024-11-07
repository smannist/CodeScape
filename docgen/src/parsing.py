import tree_sitter_python
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


def parse_code_file(filepath):
    # TODO support other languages
    language = Language(tree_sitter_python.language())
    parser = Parser(language)
    with open(filepath, "rb") as f:
        src = f.read()
    return (src, parser.parse(src))


def parse_python_imports(tree, base_path):
    """Given a syntax tree and a base_path, returns a list of names of python files imported in the tree.
    Only python files within base_path are considered"""
    imports = get_nodes_of_type(tree, "import_statement", recursive=True)
    import_names = [node.child_by_field_name("name") for node in imports]
    imports_from = get_nodes_of_type(tree, "import_from_statement", recursive=True)
    import_names += [node.child_by_field_name("module_name") for node in imports_from]
    import_files = []
    for module in import_names:
        path = os.path.join(base_path, node_to_str(module).replace('.', '/') + ".py")
        if os.path.exists(path):
            import_files.append(path)
    return import_files

