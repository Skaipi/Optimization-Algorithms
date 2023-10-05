using System;
using System.Linq;
using System.Collections.Generic;

using Graphs;

namespace Hungarian;

class Program
{
  static void Main(string[] args)
  {
    // Loader loader = new Loader("../../Maksymalne skojarzenie/maksymalne_skojarzenie2.txt");
    Loader loader = new Loader("../../Wykład/maksymalne_skojarzenie.txt");

    // var loadedGraph = loader.loadMatrixGraph();
    var loadedGraph = loader.loadAdjacentGraph();
    var bipartiteGraph = loadedGraph.ToBipartite();
    GraphLogger.Display(bipartiteGraph);

    var algorithm = new HungarianAlgorithm<AdjacencyVertex>(bipartiteGraph);
    var balancedSubgraph = algorithm.Run();

    GraphLogger.Display(balancedSubgraph);

    int sum = 0;
    foreach (var match in balancedSubgraph.Matches)
      sum += balancedSubgraph.GetConnectionWeight(match.V1, match.V2);
    Console.WriteLine(sum);
  }
}