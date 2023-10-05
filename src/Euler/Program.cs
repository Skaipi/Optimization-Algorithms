using System;
using Graphs;
namespace Euler;
class Program
{
  static void Main(string[] args)
  {
    Loader loader = new Loader("../../Cykl Eulera/euler-yes.txt");
    // var loadedGraph = loader.loadMatrixGraph();
    var loadedGraph = loader.loadAdjacentGraph();

    var eulerCycle = EulerCycle.Euler(loadedGraph);
    foreach (var r in eulerCycle)
    {
      Console.WriteLine(r);
    }
  }
}
