using System;
using Graphs;

namespace GraphsTest;

[TestClass]
public class TestAdjacentBipartiteGraph
{
  [TestMethod]
  public void TestValidConstructorCycleGraph()
  {
    bool isDirected = false;
    AdjacentGraph adjacentGraph = new AdjacentGraph(isDirected);
    var vertices = GetVertices(6);

    adjacentGraph.AddEdge(vertices[0], vertices[1]);
    adjacentGraph.AddEdge(vertices[0], vertices[5]);

    adjacentGraph.AddEdge(vertices[1], vertices[2]);
    adjacentGraph.AddEdge(vertices[1], vertices[0]);

    adjacentGraph.AddEdge(vertices[2], vertices[3]);
    adjacentGraph.AddEdge(vertices[2], vertices[1]);

    adjacentGraph.AddEdge(vertices[3], vertices[4]);
    adjacentGraph.AddEdge(vertices[3], vertices[2]);

    adjacentGraph.AddEdge(vertices[4], vertices[5]);
    adjacentGraph.AddEdge(vertices[4], vertices[3]);

    adjacentGraph.AddEdge(vertices[5], vertices[0]);
    adjacentGraph.AddEdge(vertices[5], vertices[4]);

    var adjacentBipartiteGraph = new BipartiteGraph<AdjacencyVertex>(adjacentGraph);

    var expectedInitialMatches = new List<(AdjacencyVertex, AdjacencyVertex)>{
      (vertices[0], vertices[1]),
      (vertices[2], vertices[3]),
      (vertices[4], vertices[5])
    };

    CollectionAssert.AreEquivalent(adjacentBipartiteGraph.Matches, expectedInitialMatches);
  }

  [TestMethod]
  public void TestInvalidConstructorCycleGraph()
  {
    bool isDirected = false;
    AdjacentGraph adjacentGraph = new AdjacentGraph(isDirected);
    var vertices = GetVertices(5);

    adjacentGraph.AddEdge(vertices[0], vertices[1]);
    adjacentGraph.AddEdge(vertices[0], vertices[4]);

    adjacentGraph.AddEdge(vertices[1], vertices[2]);
    adjacentGraph.AddEdge(vertices[1], vertices[0]);

    adjacentGraph.AddEdge(vertices[2], vertices[3]);
    adjacentGraph.AddEdge(vertices[2], vertices[1]);

    adjacentGraph.AddEdge(vertices[3], vertices[4]);
    adjacentGraph.AddEdge(vertices[3], vertices[2]);

    adjacentGraph.AddEdge(vertices[4], vertices[0]);
    adjacentGraph.AddEdge(vertices[4], vertices[3]);

    try
    {
      var adjacentBipartiteGraph = new BipartiteGraph<AdjacencyVertex>(adjacentGraph);
      Assert.Fail();
    }
    catch (Exception e)
    {
      Assert.AreEqual(e.Message, "Graph is not bipartite");
    }
  }

  private List<AdjacencyVertex> GetVertices(int amount)
  {
    List<AdjacencyVertex> result = new List<AdjacencyVertex>();
    for (int i = 0; i < amount; i++)
    {
      result.Add(AdjacencyVertex.FromIndex(i));
    }
    return result;
  }
}