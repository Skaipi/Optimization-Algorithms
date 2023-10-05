using Graphs;

namespace GraphsTest;

[TestClass]
public class TestMatrixGraph
{
  [TestMethod]
  public void TestCopyConstructor()
  {
    bool isDirected = false;
    MatrixGraph matrixGraph = new MatrixGraph(isDirected);
    List<Vertex> vertices = GetVertices(3);

    matrixGraph.AddEdge(vertices[0], vertices[1], 5);
    matrixGraph.AddEdge(vertices[1], vertices[2], 3);
    matrixGraph.AddEdge(vertices[0], vertices[2], 9);

    MatrixGraph copiedGraph = new MatrixGraph(matrixGraph);

    CollectionAssert.AreEqual(copiedGraph.GetAdjacentConnections(vertices[0]), matrixGraph.GetAdjacentConnections(vertices[0]));
    CollectionAssert.AreEqual(copiedGraph.GetAdjacentConnections(vertices[1]), matrixGraph.GetAdjacentConnections(vertices[1]));
    CollectionAssert.AreEqual(copiedGraph.GetAdjacentConnections(vertices[2]), matrixGraph.GetAdjacentConnections(vertices[2]));
  }

  [TestMethod]
  public void TestAddEdge()
  {
    List<Vertex> expectedVertices = GetVertices(1);
    bool isDirected = false;
    MatrixGraph matrixGraph = new MatrixGraph(isDirected);

    Vertex v1 = new Vertex(1);
    matrixGraph.AddEdge(v1, v1, 1);
    CollectionAssert.AreEqual(expectedVertices, matrixGraph.GetAllVertices());
  }

  [TestMethod]
  public void TestRemoveEdge()
  {
    List<Vertex> expectedVertices = GetVertices(0);
    bool isDirected = false;
    MatrixGraph matrixGraph = new MatrixGraph(isDirected);

    Vertex v1 = new Vertex(1);
    matrixGraph.AddEdge(v1, v1, 1);
    matrixGraph.RemoveEdge(v1, v1);
    CollectionAssert.AreEqual(expectedVertices, matrixGraph.GetAdjacentVertices(v1));
  }

  [TestMethod]
  public void TestAddConnection()
  {
    bool isDirected = false;
    MatrixGraph matrixGraph = new MatrixGraph(isDirected);
    List<Vertex> vertices = GetVertices(3);

    matrixGraph.AddEdge(vertices[0], vertices[1], 5);
    matrixGraph.AddEdge(vertices[1], vertices[2], 3);
    matrixGraph.AddEdge(vertices[0], vertices[2], 9);

    List<(Vertex Vertex, int Weight)> expectedConnectionsFrom0 = new List<(Vertex, int)> { (new Vertex(2), 5), (new Vertex(3), 9) };
    List<(Vertex Vertex, int Weight)> expectedConnectionsFrom1 = new List<(Vertex, int)> { (new Vertex(1), 5), (new Vertex(3), 3) };
    List<(Vertex Vertex, int Weight)> expectedConnectionsFrom2 = new List<(Vertex, int)> { (new Vertex(1), 9), (new Vertex(2), 3) };

    CollectionAssert.AreEqual(expectedConnectionsFrom0, matrixGraph.GetAdjacentConnections(vertices[0]));
    CollectionAssert.AreEqual(expectedConnectionsFrom1, matrixGraph.GetAdjacentConnections(vertices[1]));
    CollectionAssert.AreEqual(expectedConnectionsFrom2, matrixGraph.GetAdjacentConnections(vertices[2]));
  }

  [TestMethod]
  public void TestAddConnectionDirected()
  {
    bool isDirected = true;
    MatrixGraph matrixGraph = new MatrixGraph(isDirected);
    List<Vertex> vertices = GetVertices(3);

    matrixGraph.AddEdge(vertices[0], vertices[1], 5);
    matrixGraph.AddEdge(vertices[1], vertices[2], 3);
    matrixGraph.AddEdge(vertices[0], vertices[2], 9);

    List<(Vertex Vertex, int weight)> expectedConnectionsFrom0 = new List<(Vertex, int)> { (new Vertex(2), 5), (new Vertex(3), 9) };
    List<(Vertex Vertex, int weight)> expectedConnectionsFrom1 = new List<(Vertex, int)> { (new Vertex(3), 3) };
    List<(Vertex Vertex, int weight)> expectedConnectionsFrom2 = new List<(Vertex, int)> { };

    CollectionAssert.AreEqual(expectedConnectionsFrom0, matrixGraph.GetAdjacentConnections(vertices[0]));
    CollectionAssert.AreEqual(expectedConnectionsFrom1, matrixGraph.GetAdjacentConnections(vertices[1]));
    CollectionAssert.AreEqual(expectedConnectionsFrom2, matrixGraph.GetAdjacentConnections(vertices[2]));
  }

  [TestMethod]
  public void TestIsFullyConnected()
  {
    bool isDirected = false;
    MatrixGraph matrixGraph = new MatrixGraph(isDirected);
    List<Vertex> vertices = GetVertices(3);

    matrixGraph.AddEdge(vertices[0], vertices[1], 5);
    Assert.IsTrue(matrixGraph.IsFullyConnected());

    matrixGraph.RemoveEdge(vertices[0], vertices[1]);
    matrixGraph.AddEdge(vertices[1], vertices[2], 3);
    Assert.IsFalse(matrixGraph.IsFullyConnected());

    matrixGraph.AddEdge(vertices[0], vertices[1], 5);
    matrixGraph.AddEdge(vertices[0], vertices[2], 9);
    Assert.IsTrue(matrixGraph.IsFullyConnected());
  }

  private List<Vertex> GetVertices(int amount)
  {
    List<Vertex> result = new List<Vertex>();
    for (int i = 0; i < amount; i++)
    {
      result.Add(Vertex.FromIndex(i));
    }
    return result;
  }
}