import tree_sitter_python
from tree_sitter import Language, Parser


def get_contents(text_data: bytes, start: tuple, end: tuple) -> str:
    """
    Returns the text in the range selected by the tuples.

    Args:
    text_data (bytes): The contents of the text file.
    start (tuple): A tuple containing the line number and line position of the start of the range.
    end (tuple): A tuple containing the line number and line position of the end of the range.

    Returns:
    str: The text in the range selected by the tuples.
    """
    text = text_data.decode('utf-8')
    lines = text.splitlines()
    if start[0] < 0 or end[0] < 0 or start[0] >= len(
            lines) or end[0] >= len(lines):
        raise ValueError("Invalid line number")
    if start[1] < 0 or end[1] < 0 or start[1] > len(
            lines[start[0]]) or end[1] > len(lines[end[0]]):
        raise ValueError("Invalid line position")
    result = ""
    if start[0] == end[0]:
        result = lines[start[0]][start[1]:end[1]]
    else:
        result = lines[start[0]][start[1]:]
        for i in range(start[0] + 1, end[0]):
            result += "\n" + lines[i]
        result += "\n" + lines[end[0]][:end[1]]
    return result


def traverse_top_level(tree):
    "Yields children of translation_unit in syntax tree"
    cursor = tree.walk()
    cursor.goto_first_child()
    while True:
        yield cursor.node
        if not cursor.goto_next_sibling():
            break


def get_definitions(src, tree, definition_type):
    return [get_contents(src, node.start_point, node.end_point)
            for node in traverse_top_level(tree) if node.type == definition_type]


def parse_code_file(filepath):
    # TODO support other languages
    language = Language(tree_sitter_python.language())
    parser = Parser(language)
    with open(filepath, "rb") as f:
        src = f.read()
    return (src, parser.parse(src))
