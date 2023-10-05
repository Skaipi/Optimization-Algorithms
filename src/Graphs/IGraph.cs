using System.Collections;
namespace Graphs;

public interface IGraph<T> : IEnumerable<T> where T : Vertex
{
  public bool IsDirected { get; }

  public List<T> Vertices { get; }

  public bool IsFullyConnected();

  public void AddVertex(T v);

  public void AddEdge(T v1, T v2, int weight = 1);

  public void RemoveEdge(T v1, T v2);

  public List<T> GetAdjacentVertices(T v);

  public List<T> GetAllVertices();

  public List<(T Vertex, int Weight)> GetAdjacentConnections(T v);

  public List<(T Vertex, int Weight)> GetConnectionsTo(T v);

  public int GetConnectionWeight(T v1, T v2);

  public HashSet<T> MaximalIndependentSet(HashSet<T>? ignoredVertices = null);

  public BipartiteGraph<T> ToBipartite();
}

public abstract class AbstractGraph<T> : IGraph<T> where T : Vertex
{
  protected bool isDirected;
  protected List<T> vertices;
  public bool IsDirected { get { return isDirected; } }
  public List<T> Vertices { get { return vertices; } }

  public AbstractGraph(bool isDirected)
  {
    this.isDirected = isDirected;
    this.vertices = new List<T>();
  }

  public abstract void AddEdge(T vertex1, T vertex2, int weight = 1);

  public abstract void AddVertex(T vertex);

  public abstract void RemoveEdge(T vertex1, T vertex2);

  public abstract List<T> GetAdjacentVertices(T vertex);

  public abstract List<(T Vertex, int Weight)> GetAdjacentConnections(T vertex);

  public abstract List<(T Vertex, int Weight)> GetConnectionsTo(T v);

  public abstract int GetConnectionWeight(T vertex1, T vertex2);

  public abstract AbstractGraph<T> CreateEmptyGraph(bool isDirected = false);

  public abstract AbstractGraph<T> Copy(bool isDirected = false);

  public List<T> GetAllVertices()
  {
    return new List<T>(vertices);
  }

  public bool IsFullyConnected()
  {
    var dfsAlgorithm = new DFSAlgorithm<T>();
    dfsAlgorithm.Run(this, vertices[0]);
    var discoveredVertices = dfsAlgorithm.DiscoveredVertices;

    return vertices.Count == discoveredVertices.Count;
  }

  public HashSet<T> MaximalIndependentSet(HashSet<T>? ignoredVertices = null)
  {
    var result = new HashSet<T>();
    var allVertices = GetAllVertices();

    if (ignoredVertices != null)
      allVertices = allVertices.Except(ignoredVertices).ToList();

    allVertices.OrderBy(vertex => GetAdjacentVertices(vertex).Count);
    while (allVertices.Count() > 0)
    {
      var vertex = allVertices[0];
      result.Add(vertex);

      foreach (var neighbor in GetAdjacentVertices(vertex))
        allVertices.Remove(neighbor);
      allVertices.Remove(vertex);
    }

    return result;
  }

  public AbstractGraph<T> GetSubgraph(IEnumerable<T> vertices)
  {
    var result = CreateEmptyGraph(IsDirected);
    var commonVertices = vertices.Intersect(GetAllVertices());

    foreach (T vertex in commonVertices)
      foreach (var conn in GetAdjacentConnections(vertex))
        if (commonVertices.Contains(conn.Vertex))
          result.AddEdge(vertex, conn.Vertex, conn.Weight);

    return result;
  }

  public AbstractGraph<T> Join(AbstractGraph<T> graph)
  {
    var vertices1 = this.GetAllVertices();
    var vertices2 = graph.GetAllVertices();
    var commonVertices = vertices1.Union(vertices2);

    var resultIsDirected = this.IsDirected || graph.IsDirected;
    var result = CreateEmptyGraph(resultIsDirected);

    foreach (T vertex in commonVertices)
    {
      if (vertices1.Contains(vertex))
        foreach (var conn in GetAdjacentConnections(vertex))
          result.AddEdge(vertex, conn.Vertex, conn.Weight);


      if (vertices2.Contains(vertex))
        foreach (var conn in graph.GetAdjacentConnections(vertex))
          result.AddEdge(vertex, conn.Vertex, conn.Weight);
    }

    return result;
  }

  public BipartiteGraph<T> ToBipartite()
  {
    return new BipartiteGraph<T>(this);
  }

  public IEnumerator<T> GetEnumerator()
  {
    return new GraphEnumerator<T>(this);
  }

  private IEnumerator GetEnumerator1()
  {
    return new GraphEnumerator<T>(this);
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator1();
  }
}