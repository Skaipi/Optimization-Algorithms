using System;
using Graphs;

namespace MaxMatching;

class Program
{
  static void Main(string[] args)
  {
    Loader loader = new Loader("../../Maksymalne skojarzenie/maksymalne_skojarzenie1.txt");
    // var loadedGraph = loader.loadMatrixGraph();
    var loadedGraph = loader.loadAdjacentGraph();
    var bipartiteGraph = loadedGraph.ToBipartite();

    var path = bipartiteGraph.ExpandedPath();
    while (path.Count > 0)
    {
      for (int i = 0; i < path.Count - 1; i++)
      {
        bipartiteGraph.AddMatching(path[i], path[i + 1]);
      }

      path = bipartiteGraph.ExpandedPath();
    }

    GraphLogger.Display(bipartiteGraph);
  }
}