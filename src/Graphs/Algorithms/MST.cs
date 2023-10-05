namespace Graphs;

public class MSTAlgorithm
{
  public static AbstractGraph<T> MST<T>(Graph<T> graph) where T : Vertex
  {
    var resultTree = graph.CreateEmptyGraph();
    var discoveredVertices = new List<T> { graph.GetAllVertices()[0] };

    while (discoveredVertices.Count() < graph.GetAllVertices().Count())
    {
      (T V1, T V2, int Weight) nextConnection = (discoveredVertices[0], discoveredVertices[0], Int32.MaxValue);
      foreach (T vertex in discoveredVertices)
      {
        // Get minimal connection from tree to graph
        (T Vertex, int Weight) connectionCandidate = graph.GetAdjacentConnections(vertex)
          .Where(conn => !discoveredVertices.Contains(conn.Vertex))
          .DefaultIfEmpty()
          .MinBy(conn => conn.Weight);

        if (connectionCandidate != default && connectionCandidate.Weight < nextConnection.Weight)
        {
          nextConnection = (vertex, connectionCandidate.Vertex, connectionCandidate.Weight);
        }
      }

      resultTree.AddEdge(nextConnection.V1, nextConnection.V2, nextConnection.Weight);
      discoveredVertices.Add(nextConnection.V2);
    }

    return resultTree;
  }
}