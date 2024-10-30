import chromadb
import networkx as nx
from util import save_to_json


def get_document_distances(doc_ids, docs, max_result_count):
    """Returns a dictionary where each doc_id has a list of (other_id, distance_from_other_id) tuples"""
    if len(doc_ids) == 0 or len(docs) == 0:
        return {}

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

    
def graph_to_dict(graph):
    edges = {}
    for n, nbrs in graph.adj.items():
        neighbours = []
        for nbr, eattr in nbrs.items():
            neighbours.append({"to": nbr, "dist": eattr["weight"]})
        edges[n] = neighbours
    return {"nodes" : list(graph.nodes), "edges" : edges}


def create_code_graph(filedocs):
    funcs_and_classes = []
    for doc in filedocs:
        funcs_and_classes.extend(doc.classes + doc.functions)
    keys = [code["name"] for code in funcs_and_classes]
    values = [code["description"] for code in funcs_and_classes]
    distances = get_document_distances(keys, values, 50)
    return build_similarity_graph(distances, 0.8)

