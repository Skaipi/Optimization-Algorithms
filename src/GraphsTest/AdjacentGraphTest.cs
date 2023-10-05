using Graphs;

namespace GraphsTest;

[TestClass]
public class TestAdjacentGraph
{
  [TestMethod]
  public void TestCopyConstructor()
  {
    bool isDirected = false;
    AdjacentGraph adjacentGraph = new AdjacentGraph(isDirected);
    List<AdjacencyVertex> vertices = GetVertices(3);

    adjacentGraph.AddEdge(vertices[0], vertices[1], 5);
    adjacentGraph.AddEdge(vertices[1], vertices[2], 3);
    adjacentGraph.AddEdge(vertices[0], vertices[2], 9);

    AdjacentGraph copiedGraph = new AdjacentGraph(adjacentGraph);

    CollectionAssert.AreEqual(copiedGraph.GetAdjacentConnections(vertices[0]), adjacentGraph.GetAdjacentConnections(vertices[0]));
    CollectionAssert.AreEqual(copiedGraph.GetAdjacentConnections(vertices[1]), adjacentGraph.GetAdjacentConnections(vertices[1]));
    CollectionAssert.AreEqual(copiedGraph.GetAdjacentConnections(vertices[2]), adjacentGraph.GetAdjacentConnections(vertices[2]));
  }

  [TestMethod]
  public void TestAddEdge()
  {
    List<AdjacencyVertex> expectedVertices = GetVertices(2);
    bool isDirected = false;
    AdjacentGraph adjacentGraph = new AdjacentGraph(isDirected);

    AdjacencyVertex v1 = new AdjacencyVertex(1);
    AdjacencyVertex v2 = new AdjacencyVertex(2);
    adjacentGraph.AddEdge(expectedVertices[0], expectedVertices[1], 1);
    CollectionAssert.AreEquivalent(expectedVertices, adjacentGraph.GetAllVertices());
  }

  [TestMethod]
  public void TestRemoveEdge()
  {
    List<(AdjacencyVertex Vertex, int Weight)> expectedConnections = new List<(AdjacencyVertex Vertex, int Weight)>();
    bool isDirected = false;
    AdjacentGraph adjacentGraph = new AdjacentGraph(isDirected);

    AdjacencyVertex v1 = new AdjacencyVertex(1);
    AdjacencyVertex v2 = new AdjacencyVertex(2);
    adjacentGraph.AddEdge(v1, v2, 1);
    adjacentGraph.RemoveEdge(v1, v2);
    CollectionAssert.AreEqual(expectedConnections, adjacentGraph.GetAdjacentConnections(v1));
  }

  [TestMethod]
  public void TestAddConnection()
  {
    bool isDirected = false;
    AdjacentGraph adjacentGraph = new AdjacentGraph(isDirected);
    List<AdjacencyVertex> vertices = GetVertices(3);

    adjacentGraph.AddEdge(vertices[0], vertices[1], 5);
    adjacentGraph.AddEdge(vertices[1], vertices[2], 3);
    adjacentGraph.AddEdge(vertices[0], vertices[2], 9);

    var expectedConnectionsFrom0 = new List<(AdjacencyVertex Vertex, int Weight)> { (new AdjacencyVertex(2), 5), (new AdjacencyVertex(3), 9) };
    var expectedConnectionsFrom1 = new List<(AdjacencyVertex Vertex, int Weight)> { (new AdjacencyVertex(1), 5), (new AdjacencyVertex(3), 3) };
    var expectedConnectionsFrom2 = new List<(AdjacencyVertex Vertex, int Weight)> { (new AdjacencyVertex(1), 9), (new AdjacencyVertex(2), 3) };

    CollectionAssert.AreEquivalent(expectedConnectionsFrom0, adjacentGraph.GetAdjacentConnections(vertices[0]));
    CollectionAssert.AreEquivalent(expectedConnectionsFrom1, adjacentGraph.GetAdjacentConnections(vertices[1]));
    CollectionAssert.AreEquivalent(expectedConnectionsFrom2, adjacentGraph.GetAdjacentConnections(vertices[2]));
  }

  [TestMethod]
  public void TestAddConnectionDirected()
  {
    bool isDirected = true;
    AdjacentGraph adjacentGraph = new AdjacentGraph(isDirected);
    List<AdjacencyVertex> vertices = GetVertices(3);

    adjacentGraph.AddEdge(vertices[0], vertices[1], 5);
    adjacentGraph.AddEdge(vertices[1], vertices[2], 3);
    adjacentGraph.AddEdge(vertices[0], vertices[2], 9);

    var expectedConnectionsFrom0 = new List<(AdjacencyVertex Vertex, int Weight)> { (new AdjacencyVertex(2), 5), (new AdjacencyVertex(3), 9) };
    var expectedConnectionsFrom1 = new List<(AdjacencyVertex Vertex, int Weight)> { (new AdjacencyVertex(3), 3) };
    var expectedConnectionsFrom2 = new List<(AdjacencyVertex Vertex, int Weight)> { };

    CollectionAssert.AreEquivalent(expectedConnectionsFrom0, adjacentGraph.GetAdjacentConnections(vertices[0]));
    CollectionAssert.AreEquivalent(expectedConnectionsFrom1, adjacentGraph.GetAdjacentConnections(vertices[1]));
    CollectionAssert.AreEquivalent(expectedConnectionsFrom2, adjacentGraph.GetAdjacentConnections(vertices[2]));
  }

  [TestMethod]
  public void TestIsFullyConnected()
  {
    bool isDirected = false;
    AdjacentGraph adjacentGraph = new AdjacentGraph(isDirected);
    List<AdjacencyVertex> vertices = GetVertices(3);

    adjacentGraph.AddEdge(vertices[0], vertices[1], 5);
    Assert.IsTrue(adjacentGraph.IsFullyConnected());

    adjacentGraph.RemoveEdge(vertices[0], vertices[1]);
    Assert.IsFalse(adjacentGraph.IsFullyConnected());

    adjacentGraph.AddEdge(vertices[1], vertices[2], 3);
    Assert.IsFalse(adjacentGraph.IsFullyConnected());

    adjacentGraph.AddEdge(vertices[0], vertices[1], 5);
    adjacentGraph.AddEdge(vertices[0], vertices[2], 9);
    Assert.IsTrue(adjacentGraph.IsFullyConnected());
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