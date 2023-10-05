using System.Collections.Generic;
using Graphs;

namespace Christofides;
class Program
{
  static void Main(string[] args)
  {
    Loader loader = new Loader("../../Wykład/comivoyager.txt");
    // Loader loader = new Loader("../../Maksymalne skojarzenie/maksymalne_skojarzenie2.txt");
    // var loadedGraph = loader.loadMatrixGraph();
    var loadedGraph = loader.loadAdjacentGraph();

    GraphLogger.Display(loadedGraph);

    var cycle = Christofides(loadedGraph);
    Console.Write("Voyage voyage:");
    foreach (var vertex in cycle)
    {
      Console.Write($" {vertex.Id} ->");
    }
    Console.WriteLine($" {cycle[0].Id} (with cost: {PathCost(loadedGraph, cycle)})");
  }

  public static List<AdjacencyVertex> Christofides(Graph<AdjacencyVertex> graph)
  {
    AdjacentGraph minimalSpanningTree = (AdjacentGraph)MSTAlgorithm.MST(graph);
    HashSet<AdjacencyVertex> oddDegreeVertices = new HashSet<AdjacencyVertex>();

    foreach (AdjacencyVertex vertex in minimalSpanningTree)
    {
      if (minimalSpanningTree.GetAdjacentVertices(vertex).Count() % 2 == 1)
      {
        oddDegreeVertices.Add(vertex);
      }
    }

    Graph<AdjacencyVertex> subGraph = graph.GetSubgraph(oddDegreeVertices);
    // [TODO]: https://pub.ista.ac.at/~vnk/papers/BLOSSOM5.html
    // subGraph.SetPerfectMatching();
    // foreach (var match in subGraph.Matches)
    // {
    //   int connectionWeight = graph.GetConnectionWeight(match.V1, match.V2);
    //   minimalSpanningTree.AddEdge(match.V1, match.V2, connectionWeight);
    // }

    List<AdjacencyVertex?> optimalCycle = EulerCycle.Euler(minimalSpanningTree).Cast<AdjacencyVertex?>().ToList();
    HashSet<AdjacencyVertex?> visitedVertices = new HashSet<AdjacencyVertex?>();
    for (int i = 0; i < optimalCycle.Count(); i++)
    {
      if (visitedVertices.Contains(optimalCycle[i]))
      {
        optimalCycle[i] = null;
      }
      else
      {
        visitedVertices.Add(optimalCycle[i]);
      }
    }

    optimalCycle.RemoveAll(vertex => vertex == null);
    return optimalCycle.Cast<AdjacencyVertex>().ToList();
  }

  public static int PathCost(Graph<AdjacencyVertex> graph, List<AdjacencyVertex> vertices)
  {
    var result = 0;
    for (int i = 0; i < vertices.Count; i++)
      result += graph.GetConnectionWeight(vertices[i], vertices[(i + 1) % vertices.Count]);
    return result;
  }
}
