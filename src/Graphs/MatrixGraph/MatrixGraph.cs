using System.Collections;

namespace Graphs;

public class MatrixGraph : AbstractGraph<Vertex>
{
  protected Dictionary<Vertex, int> indexMap;
  protected List<List<int>> Matrix;

  public MatrixGraph(MatrixGraph graph, bool isDirected = false) : base(isDirected)
  {
    this.indexMap = new Dictionary<Vertex, int>();
    this.Matrix = new List<List<int>>();

    foreach (Vertex vertex in graph)
      this.AddVertex(vertex);

    foreach (Vertex vertex in graph)
      foreach (var conn in graph.GetAdjacentConnections(vertex))
        this.AddEdge(vertex, conn.Vertex, conn.Weight);
  }

  public MatrixGraph(bool isDirected = false) : base(isDirected)
  {
    this.Matrix = new List<List<int>>();
    this.indexMap = new Dictionary<Vertex, int>();
  }

  public override MatrixGraph CreateEmptyGraph(bool isDirected = false)
  {
    return new MatrixGraph(isDirected);
  }

  public override MatrixGraph Copy(bool isDirected = false)
  {
    return new MatrixGraph(this, this.isDirected || isDirected);
  }

  protected int Index(Vertex vertex)
  {
    return indexMap[vertex];
  }

  public override void AddEdge(Vertex vertex1, Vertex vertex2, int weight = 1)
  {
    if (!indexMap.ContainsKey(vertex1)) AddVertex(vertex1);
    if (!indexMap.ContainsKey(vertex2)) AddVertex(vertex2);

    this.Matrix[Index(vertex1)][Index(vertex2)] = weight;
    if (!isDirected) this.Matrix[Index(vertex2)][Index(vertex1)] = weight;
  }

  public override void AddVertex(Vertex vertex)
  {
    if (indexMap.ContainsKey(vertex)) return;

    vertices.Add(vertex);
    indexMap.Add(vertex, vertices.Count - 1);

    Matrix.Add(new List<int>());
    for (int i = 0; i < vertices.Count; i++)
      Matrix[vertices.Count - 1].Add(Int32.MaxValue);

    foreach (List<int> row in Matrix)
      row.Add(Int32.MaxValue);
  }

  public override void RemoveEdge(Vertex vertex1, Vertex vertex2)
  {
    if (!vertices.Contains(vertex1) || !vertices.Contains(vertex2)) return;

    Matrix[Index(vertex1)][Index(vertex2)] = Int32.MaxValue;
    if (!isDirected) Matrix[Index(vertex2)][Index(vertex1)] = Int32.MaxValue;
  }

  public override List<Vertex> GetAdjacentVertices(Vertex vertex)
  {
    List<Vertex> adjacentVertices = new List<Vertex>();
    foreach (var neighbor in vertices)
    {
      if (Matrix[Index(vertex)][Index(neighbor)] < Int32.MaxValue)
        adjacentVertices.Add(neighbor);
    }
    return adjacentVertices;
  }

  public override List<(Vertex Vertex, int Weight)> GetAdjacentConnections(Vertex vertex)
  {
    var adjacentConnections = new List<(Vertex Vertex, int Weight)>();
    foreach (var neighbor in vertices)
    {
      if (Matrix[Index(vertex)][Index(neighbor)] < Int32.MaxValue)
        adjacentConnections.Add((neighbor, Matrix[Index(vertex)][Index(neighbor)]));
    }
    return adjacentConnections;
  }

  public override List<(Vertex Vertex, int Weight)> GetConnectionsTo(Vertex vertex)
  {
    var connectionsToVertex = new List<(Vertex Vertex, int Weight)>();
    foreach (var neighbor in vertices)
    {
      if (Matrix[Index(neighbor)][Index(vertex)] < Int32.MaxValue)
        connectionsToVertex.Add((neighbor, Matrix[Index(neighbor)][Index(vertex)]));
    }
    return connectionsToVertex;
  }

  public override int GetConnectionWeight(Vertex vertex1, Vertex vertex2)
  {
    return Matrix[Index(vertex1)][Index(vertex2)];
  }
}