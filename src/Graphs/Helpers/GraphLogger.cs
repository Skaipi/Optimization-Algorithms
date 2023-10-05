namespace Graphs;

public static class GraphLogger
{
  public static void Display<T>(Graph<T> graph) where T : Vertex
  {
    Console.WriteLine("Adjacent Graph:");
    foreach (T v in graph)
    {
      Console.Write($"  {v.Id}: ");
      foreach (var conn in graph.GetAdjacentConnections(v))
      {
        Console.Write($"(v:{conn.Vertex.Id}, w:{conn.Weight}) ");
      }
      Console.WriteLine();
    }
    Console.WriteLine();
  }

  public static void Display<T>(BipartiteGraph<T> graph) where T : Vertex
  {
    Console.WriteLine("Adjacent Bipartite Graph:");
    foreach (T v in graph)
    {
      Console.Write($"  {v.Id}: ");
      foreach (var conn in graph.GetAdjacentConnections(v))
      {
        Console.Write($"(v:{conn.Vertex.Id}, w:{conn.Weight}) ");
      }
      Console.WriteLine();
    }

    Console.Write("  Left Vertices:");
    foreach (var vertex in graph.leftVertices)
    {
      Console.Write($" {vertex.Id}");
    }
    Console.WriteLine();

    Console.Write("  Right Vertices:");
    foreach (var vertex in graph.rightVertices)
    {
      Console.Write($" {vertex.Id}");
    }
    Console.WriteLine();

    Console.WriteLine("  Matches:");
    foreach (var match in graph.Matches)
    {
      Console.WriteLine($"    {match.V1.Id} <-> {match.V2.Id}");
    }
    Console.WriteLine();
  }
}