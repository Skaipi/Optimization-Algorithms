namespace Graphs;

public class BipartiteGraph<T> : Graph<T> where T : Vertex
{
  private List<(T V1, T V2)> matches;
  public List<(T V1, T V2)> Matches { get { return matches; } }
  public List<T> leftVertices { get; }
  public List<T> rightVertices { get; }

  private BipartiteGraph(BipartiteGraph<T> parent, AbstractGraph<T> graph) : base(graph)
  {
    foreach (T vertex in parent)
      this.AddVertex(vertex);

    // Intersect to make sure we have the same references
    this.rightVertices = Vertices.Intersect(parent.rightVertices).ToList();
    this.leftVertices = Vertices.Intersect(parent.leftVertices).ToList();
    this.matches = new List<(T V1, T V2)>();
    InitializeMatching();
  }

  public BipartiteGraph(AbstractGraph<T> graph) : base(graph)
  {
    var bipartitionTest = new TestBipartitionDFS<T>();
    var startingVertex = this.Vertices.Count > 0 ? this.Vertices[0] : null;
    bipartitionTest.Run(this, startingVertex);

    if (!bipartitionTest.isBipartite)
      throw new Exception("Graph is not bipartite");

    // Intersect to make sure we have the same references
    this.leftVertices = Vertices.Intersect(bipartitionTest.LeftVertices).ToList();
    this.rightVertices = Vertices.Intersect(bipartitionTest.RightVertices).ToList();
    this.matches = new List<(T V1, T V2)>();
    InitializeMatching();
    InitializeLabels();
  }

  public bool IsMatched(T v)
  {
    return matches.Find(match => match.V1.Equals(v) || match.V2.Equals(v)) != default;
  }

  public bool AreMatched(T v1, T v2)
  {
    return matches.Find(match => match == (v1, v2) || match == (v2, v1)) != default;
  }

  public bool HasMaxMatching()
  {
    return ExpandedPath().Count <= 0;
  }

  public void AddMatching(T v1, T v2)
  {
    var conn = leftVertices.Contains(v1) ? (v1, v2) : (v2, v1);

    if (matches.Contains(conn))
      matches.Remove(conn);
    else
      matches.Add(conn);
  }

  public List<T> UnmatchedVertices(List<T> vertices)
  {
    var result = new List<T>();

    foreach (var vertex in vertices)
    {
      if (matches.All(conn => !conn.V1.Equals(vertex) && !conn.V2.Equals(vertex)))
      {
        result.Add(vertex);
      }
    }

    return result;
  }

  public void SetPerfectMatching()
  {
    var path = ExpandedPath();
    while (path.Count > 0)
    {
      for (int i = 0; i < path.Count() - 1; i++)
      {
        AddMatching(path[i], path[i + 1]);
      }

      path = ExpandedPath();
    }
  }

  public void InheritMatching(BipartiteGraph<T> graph)
  {
    matches = new List<(T V1, T V2)>();
    foreach (var match in graph.Matches)
    {
      var v1 = Vertices.Find(v => v.Id == match.V1.Id);
      var v2 = Vertices.Find(v => v.Id == match.V2.Id);

      if (v1 == null || v2 == null) continue;

      AddMatching(v1, v2);
    }
  }

  public List<T> FindPath(List<T> source, List<T> dest)
  {
    var path = new List<T>();

    foreach (T start in source)
    {
      var dfs = new DFSAlgorithm<T>();
      dfs.Run(this, start, dest);
      var pathToDest = dfs.PathToDest;

      if (pathToDest.Count() > 0)
      {
        path = pathToDest;
        break;
      }
    }

    return path;
  }

  public List<(T V1, T V2, int Weight)> GetBalancedEdges()
  {
    List<(T V1, T V2, int Weight)> balancedEdges = new List<(T, T, int)>();

    foreach (var v1 in leftVertices)
    {
      foreach (var conn in GetAdjacentConnections(v1))
      {
        var v2 = conn.Vertex;

        if (v1.Label + v2.Label == conn.Weight)
        {
          balancedEdges.Add((v1, v2, conn.Weight));
        }
      }
    }

    return balancedEdges;
  }

  public BipartiteGraph<T> GetBalancedSubgraph(BipartiteGraph<T>? previous = null)
  {
    AbstractGraph<T> graph = CreateEmptyGraph(IsDirected);
    foreach (T vertex in GetAllVertices())
      graph.AddVertex(vertex);

    foreach (var edge in GetBalancedEdges())
    {
      graph.AddEdge(edge.V1, edge.V2, edge.Weight);
    }

    BipartiteGraph<T> result = new BipartiteGraph<T>(this, graph);

    if (previous is not null)
      result.InheritMatching(previous);

    return result;
  }

  public List<T> ExpandedPath(List<T>? from = null, List<T>? to = null)
  {
    var v1 = from ?? UnmatchedVertices(leftVertices);
    var v2 = to ?? UnmatchedVertices(rightVertices);

    var matchGraph = GraphFromMatches();
    var path = matchGraph.FindPath(v1, v2);
    return path;
  }

  private BipartiteGraph<T> GraphFromMatches()
  {
    bool directed = true;
    AbstractGraph<T> graph = CreateEmptyGraph(directed);
    foreach (T vertex in GetAllVertices())
      graph.AddVertex(vertex);

    foreach (T vertex in leftVertices)
    {
      var connections = GetAdjacentConnections(vertex);
      foreach (var conn in connections)
      {
        graph.AddEdge(vertex, conn.Vertex);
      }
    }

    foreach (var match in matches)
    {
      graph.AddEdge(match.V2, match.V1);
    }

    return new BipartiteGraph<T>(graph);
  }

  private void InitializeMatching()
  {
    if (matches.Count > 0)
      matches = new List<(T V1, T V2)>();

    var unmatchedLeft = leftVertices;

    foreach (var vertex in unmatchedLeft)
    {
      var matchedRight = rightVertices.Except(UnmatchedVertices(rightVertices));
      foreach (var neighbor in GetAdjacentVertices(vertex).Except(matchedRight))
      {
        (T, T) matchForNeighbor = matches.Find(match => match.V1.Equals(neighbor) || match.V2.Equals(neighbor));
        if (matchForNeighbor == default)
        {
          AddMatching(vertex, neighbor);
          goto Next;
        }
      }
    Next:
      continue;
    }
  }

  private void InitializeLabels()
  {
    foreach (var vertex in leftVertices)
    {
      vertex.Label = GetAdjacentConnections(vertex).MaxBy(conn => conn.Weight).Weight;
    }

    foreach (var vertex in rightVertices)
    {
      vertex.Label = 0;
    }
  }
}