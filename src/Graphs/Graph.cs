using System.Collections;

namespace Graphs;

public class Graph<T> : AbstractGraph<T>, IEnumerable<T> where T : Vertex
{
  private AbstractGraph<T> _graph;

  public new List<T> Vertices { get { return _graph.Vertices; } }

  public new bool IsDirected { get { return _graph.IsDirected; } }

  public Graph(AbstractGraph<T> graph) : base(graph.IsDirected)
  {
    this._graph = graph;
  }

  public override AbstractGraph<T> CreateEmptyGraph(bool isDirected = false)
  {
    return _graph.CreateEmptyGraph(isDirected);
  }

  public override Graph<T> Copy(bool isDirected = false)
  {
    return new Graph<T>(_graph.Copy(isDirected));
  }

  public new bool IsFullyConnected()
  {
    return _graph.IsFullyConnected();
  }

  public override void AddVertex(T vertex)
  {
    _graph.AddVertex(vertex);
  }

  public override void AddEdge(T vertex1, T vertex2, int weight = 1)
  {
    _graph.AddEdge(vertex1, vertex2, weight);
  }

  public override void RemoveEdge(T vertex1, T vertex2)
  {
    _graph.RemoveEdge(vertex1, vertex2);
  }

  public new List<T> GetAllVertices()
  {
    return _graph.GetAllVertices();
  }

  public override List<T> GetAdjacentVertices(T vertex)
  {
    return _graph.GetAdjacentVertices(vertex);
  }

  public override List<(T Vertex, int Weight)> GetAdjacentConnections(T vertex)
  {
    return _graph.GetAdjacentConnections(vertex);
  }

  public override List<(T Vertex, int Weight)> GetConnectionsTo(T vertex)
  {
    return _graph.GetConnectionsTo(vertex);
  }

  public override int GetConnectionWeight(T vertex1, T vertex2)
  {
    return _graph.GetConnectionWeight(vertex1, vertex2);
  }

  public new HashSet<T> MaximalIndependentSet(HashSet<T>? ignoredVertices = null)
  {
    return _graph.MaximalIndependentSet(ignoredVertices);
  }

  public new Graph<T> Join(AbstractGraph<T> graph)
  {
    return new Graph<T>(_graph.Join(graph));
  }

  public new Graph<T> GetSubgraph(IEnumerable<T> vertices)
  {
    return new Graph<T>(_graph.GetSubgraph(vertices));
  }

  public new BipartiteGraph<T> ToBipartite()
  {
    return _graph.ToBipartite();
  }

  public new IEnumerator<T> GetEnumerator()
  {
    return new GraphEnumerator<T>(_graph);
  }

  private IEnumerator GetEnumerator1()
  {
    return new GraphEnumerator<T>(_graph);
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator1();
  }
}