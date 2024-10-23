import chromadb
import networkx as nx
from util import save_to_json


def get_document_distances(doc_ids, docs, max_result_count):
    """Returns a dictionary where each doc_id has a list of (other_id, distance_from_other_id) tuples"""
    chroma_client = chromadb.Client()
    db = chroma_client.create_collection(name="code-descriptions")
    db.add(documents=docs, ids=doc_ids)

    results = {}
    for (idx, doc) in enumerate(docs):
        query = db.query(query_texts=[doc], n_results=min(len(docs), max_result_count))
        results[doc_ids[idx]] = list(zip(query["ids"][0], query["distances"][0]))

    return results


def build_similarity_graph(dists, dist_treshold):
    """Returns a networkx graph where nodes with distance < dist_treshold are connected.
    Distances are stored as edge weights.
    """
    graph = nx.Graph()
    ids = dists.keys()
    graph.add_nodes_from(ids)
    for id in ids:
        for related in dists[id]:
            related_id = related[0]
            distance = related[1]
            if distance < dist_treshold and related_id != id:
                graph.add_edge(id, related_id)
                graph.edges[id, related_id]['weight'] = distance
    return graph


def plot_graph(graph):
    import matplotlib.pyplot as plt
    pos = nx.spring_layout(graph)
    weights = {e: f"{graph.edges[e]['weight']:.2}" for e in graph.edges}
    nx.draw_networkx_edge_labels(graph, pos, edge_labels=weights)
    nx.draw(graph, pos=pos, with_labels=True)
    plt.show()


def save_graph_json(graph, filepath):
    edges = {}
    for n, nbrs in graph.adj.items():
        neighbours = []
        for nbr, eattr in nbrs.items():
            neighbours.append({"to": nbr, "dist": eattr["weight"]})
        edges[n] = neighbours
    json = {"nodes" : list(graph.nodes), "edges" : edges}
    save_to_json(json, filepath)


if __name__ == "__main__": # For testing
    from util import load_json
    from glob import glob

    def get_docs_by_field(file_docs, field):
        return [doc["description"][field] for file in file_docs for doc in file]

    test_input = [load_json(path) for path in glob('*.json')]
    keys = get_docs_by_field(test_input, "name")
    values = get_docs_by_field(test_input, "description")

    distances = get_document_distances(keys, values, 50)
    g = build_similarity_graph(distances, 0.8)
    save_graph_json(g, "graph.json")
    print("saved graph as graph.json")
    plot_graph(g)
