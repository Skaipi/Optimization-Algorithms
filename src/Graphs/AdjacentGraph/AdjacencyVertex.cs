namespace Graphs;

public class AdjacencyVertex : Vertex
{
  private List<(AdjacencyVertex Vertex, int Weight)> adjacencyList;

  public List<(AdjacencyVertex Vertex, int Weight)> AdjacencyList { get { return adjacencyList; } }

  public AdjacencyVertex(AdjacencyVertex vertex) : base(vertex)
  {
    // [NOTE]: There is no adjacency list copy, this is graph responsibility
    this.adjacencyList = new List<(AdjacencyVertex, int)>();
  }

  public AdjacencyVertex(int id, int color = -1) : base(id, color)
  {
    this.adjacencyList = new List<(AdjacencyVertex, int)>();
  }

  public void AddEdge(AdjacencyVertex vertex, int weight)
  {
    if (this.Id == vertex.Id)
      throw new ArgumentException("The vertex cannot be adjacent to itself");

    var elIndex = this.adjacencyList.FindIndex(connection => connection.Vertex.Id == vertex.Id);

    if (elIndex >= 0)
      adjacencyList[elIndex] = (adjacencyList[elIndex].Vertex, weight);
    else
      this.adjacencyList.Add((vertex, weight));
  }

  public void RemoveEdge(AdjacencyVertex vertex)
  {
    var removeCandidate = this.adjacencyList.Find(connection => connection.Vertex.Id == vertex.Id);

    this.adjacencyList.Remove(removeCandidate);
  }
}
