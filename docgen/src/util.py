import json
import os

def save_to_json(data, filename):
    with open(filename, 'w') as json_file:
        json.dump(data, json_file, indent=4)

def load_json(filename):
    with open(filename) as json_file:
        return json.load(json_file)

def list_files_if(base_path, condition_func):
    "Returns a list of filenames within base_path for which condition_func returns True. base_path can also be a single file."
    if os.path.isfile(base_path):
        if condition_func(base_path):
            return [base_path]
        return []
    result = []
    def recurse(directory):
        for item in os.listdir(directory):
            item_path = os.path.join(directory, item)
            if os.path.isdir(item_path):
                recurse(item_path)
            elif condition_func(item):
                result.append(item_path)
    recurse(base_path)
    return result

