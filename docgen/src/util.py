import json


def save_to_json(data, filename):
    with open(filename, 'w') as json_file:
        json.dump(data, json_file, indent=4)

def load_json(filename):
    with open(filename) as json_file:
        return json.load(json_file)
