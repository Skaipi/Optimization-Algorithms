using System;
using System.Collections;
using System.Linq;

namespace Graphs;

public class AdjacentGraph : AbstractGraph<AdjacencyVertex>
{
  public AdjacentGraph(AdjacentGraph graph, bool isDirected = false) : base(isDirected)
  {
    this.isDirected = graph.isDirected;
    this.vertices = new List<AdjacencyVertex>();

    foreach (AdjacencyVertex vertex in graph)
      this.AddVertex(vertex);

    foreach (AdjacencyVertex vertex in graph)
      foreach (var conn in graph.GetAdjacentConnections(vertex))
        this.AddEdge(vertex, conn.Vertex, conn.Weight);
  }

  public AdjacentGraph(bool isDirected = false) : base(isDirected)
  {
  }

  public override AdjacentGraph CreateEmptyGraph(bool isDirected = false)
  {
    return new AdjacentGraph(isDirected);
  }

  public override AdjacentGraph Copy(bool isDirected = false)
  {
    return new AdjacentGraph(this, this.isDirected || isDirected);
  }

  public override void AddEdge(AdjacencyVertex vertex1, AdjacencyVertex vertex2, int weight = 1)
  {
    if (!vertices.Contains(vertex1)) AddVertex(vertex1);
    if (!vertices.Contains(vertex2)) AddVertex(vertex2);

    var v1 = vertices.Find(x => x.Id == vertex1.Id);
    var v2 = vertices.Find(x => x.Id == vertex2.Id);

    if (v1 == null || v2 == null) return;

    v1.AddEdge(v2, weight);
    if (!this.isDirected) v2.AddEdge(v1, weight);
  }

  public override void AddVertex(AdjacencyVertex vertex)
  {
    if (vertices.Contains(vertex)) return;
    vertices.Add(new AdjacencyVertex(vertex));
  }

  public override void RemoveEdge(AdjacencyVertex vertex1, AdjacencyVertex vertex2)
  {
    this.vertices.Find(v => v.Id == vertex1.Id)?.RemoveEdge(vertex2);
    if (!this.isDirected) this.vertices.Find(v => v.Id == vertex2.Id)?.RemoveEdge(vertex1);
  }

  public override List<AdjacencyVertex> GetAdjacentVertices(AdjacencyVertex vertex)
  {
    return this.GetAdjacentConnections(vertex).Select(connection => connection.Vertex).ToList();
  }

  public override List<(AdjacencyVertex Vertex, int Weight)> GetAdjacentConnections(AdjacencyVertex vertex)
  {
    AdjacencyVertex? vertexCandidate = this.vertices.Find(v => v.Id == vertex.Id);

    if (vertexCandidate == null)
      throw new ArgumentOutOfRangeException("Vertex is out of bounds");

    return vertexCandidate.AdjacencyList;
  }

  public override List<(AdjacencyVertex Vertex, int Weight)> GetConnectionsTo(AdjacencyVertex vertex)
  {
    var connectionsToVertex = new List<(AdjacencyVertex Vertex, int Weight)>();
    AdjacencyVertex? vertexCandidate = this.vertices.Find(v => v.Id == vertex.Id);

    if (vertexCandidate == null)
      throw new ArgumentOutOfRangeException("Vertex is out of bounds");

    foreach (AdjacencyVertex v in GetAllVertices())
    {
      var connection = v.AdjacencyList.Find(conn => conn.Vertex.Equals(vertex));
      if (connection != default)
        connectionsToVertex.Add((v, connection.Weight));
    }

    return connectionsToVertex;
  }

  public override int GetConnectionWeight(AdjacencyVertex vertex1, AdjacencyVertex vertex2)
  {
    AdjacencyVertex? vertexCandidate = this.vertices.Find(v => v.Id == vertex1.Id);
    return vertexCandidate != null ? vertexCandidate.AdjacencyList.Find(conn => conn.Vertex.Equals(vertex2)).Weight : 0;
  }
}