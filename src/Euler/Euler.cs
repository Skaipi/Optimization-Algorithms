
using Graphs;
namespace Euler;

public static class EulerCycle
{
  public static List<T> Euler<T>(Graph<T> graph) where T : Vertex
  {
    var result = new List<T>();
    var stack = new Stack<T>(0);

    if (graph.GetAllVertices().Count <= 0 || !graph.IsFullyConnected()) return result;

    var currentVertex = graph.Vertices[0];

    foreach (T vertex in graph)
      if (graph.GetAdjacentConnections(vertex).Count % 2 != 0) return result;

    while (true)
    {
      var adjacentVertices = graph.GetAdjacentVertices(currentVertex);

      if (adjacentVertices.Count > 0)
      {
        var currentNeighbor = adjacentVertices[0];
        stack.Push(currentVertex);
        graph.RemoveEdge(currentVertex, currentNeighbor);
        currentVertex = currentNeighbor;
      }
      else
      {
        if (stack.Count > 0)
        {
          currentVertex = stack.Pop();
          result.Add(currentVertex);
        }
        else { break; }
      }
    };

    return result;
  }
}
