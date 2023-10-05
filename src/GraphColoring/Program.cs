using System.Collections.Generic;

using Graphs;

namespace GraphColoring;
class Program
{
  static void Main(string[] args)
  {
    Loader loader = new Loader("../../Cykl Eulera/euler-yes.txt");
    // var loadedGraph = loader.loadMatrixGraph();
    var loadedGraph = loader.loadAdjacentGraph();

    var colorSets = ColorGraph(loadedGraph);

    foreach (var set in colorSets)
    {
      Console.WriteLine("Hash Set:");
      foreach (var vertex in set)
      {
        Console.WriteLine($"Vertex({vertex.Id}) <-> Color({vertex.Color})");
      }
      Console.WriteLine();
    }
  }

  public static List<HashSet<T>> ColorGraph<T>(Graph<T> graph) where T : Vertex
  {
    HashSet<T> discoveredVertices = new HashSet<T>();
    List<HashSet<T>> result = new List<HashSet<T>>();

    while (discoveredVertices.Count < graph.GetAllVertices().Count)
    {
      HashSet<T> newSet = graph.MaximalIndependentSet(discoveredVertices);
      result.Add(newSet);
      discoveredVertices.UnionWith(newSet);
    }

    int color = 1;
    foreach (var hashSet in result)
    {
      foreach (var vertex in hashSet)
      {
        vertex.Color = color;
      }
      color++;
    }

    return result;
  }
}
