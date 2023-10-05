using System.Collections.Generic;
using Graphs;

namespace Hungarian;

public class HungarianAlgorithm<T> where T : Vertex
{
  BipartiteGraph<T> graph;
  HashSet<T> workers;
  HashSet<T> jobs;

  public HungarianAlgorithm(BipartiteGraph<T> graph)
  {
    this.graph = graph;
    this.workers = new HashSet<T>();
    this.jobs = new HashSet<T>();
  }

  public BipartiteGraph<T> Run()
  {
    BipartiteGraph<T> balancedSubgraph = graph.GetBalancedSubgraph();
    var freeWorker = GetInitialWorker(balancedSubgraph);

    while (!graph.HasMaxMatching())
    {
      if (freeWorker == null) break;
      HashSet<T> reachableJobs = GetReachableJobs(balancedSubgraph);

      if (reachableJobs.SetEquals(jobs))
      {
        RelabelVertices(balancedSubgraph);
        balancedSubgraph = graph.GetBalancedSubgraph();
        graph.InheritMatching(balancedSubgraph);
      }
      else
      {
        var takeableJob = reachableJobs.Except(jobs).ToList()[0];

        if (!balancedSubgraph.IsMatched(takeableJob))
        {
          UpdateMatchesWithExpandedPath(balancedSubgraph, freeWorker, takeableJob);
          freeWorker = GetInitialWorker(balancedSubgraph);
        }
        else
        {
          var takeableWorker = balancedSubgraph.Matches.Find(match => match.V2.Equals(takeableJob)).V1;
          workers.Add(takeableWorker);
          jobs.Add(takeableJob);
        }
      }
    }

    return balancedSubgraph;
  }

  private T? GetInitialWorker(BipartiteGraph<T> balancedGraph)
  {
    graph.InheritMatching(balancedGraph);
    if (graph.HasMaxMatching())
      return null;

    var freeWorker = balancedGraph.UnmatchedVertices(balancedGraph.leftVertices)[0];
    workers = new HashSet<T> { freeWorker };
    jobs = new HashSet<T>();
    return freeWorker;
  }

  private HashSet<T> GetReachableJobs(BipartiteGraph<T> balancedSubgraph)
  {
    HashSet<T> reachableJobs = new HashSet<T>();
    foreach (var worker in workers)
    {
      foreach (var job in balancedSubgraph.GetAdjacentVertices(worker))
      {
        reachableJobs.Add(job);
      }
    }
    return reachableJobs;
  }

  private void RelabelVertices(BipartiteGraph<T> balancedGraph)
  {
    int minWeight = Int32.MaxValue;
    foreach (var x in workers)
    {
      var workerConnectinos = graph.GetAdjacentConnections(x);
      foreach (var y in graph.rightVertices)
      {
        var w = graph.GetConnectionWeight(x, y);
        if (!jobs.Contains(y) && x.Label + y.Label - w < minWeight)
          minWeight = x.Label + y.Label - w;
      }
    }

    if (minWeight == Int32.MaxValue)
      throw new Exception("Kuhn Munkers algorithm error");

    foreach (T vertex in graph)
    {
      if (workers.Contains(vertex))
        vertex.Label -= minWeight;
      if (jobs.Contains(vertex))
        vertex.Label += minWeight;
    }
  }

  private void UpdateMatchesWithExpandedPath(BipartiteGraph<T> balancedGraph, T freeWorker, T takeableJob)
  {
    var expandedPath = balancedGraph.ExpandedPath();
    for (int i = 0; i < expandedPath.Count - 1; i++)
    {
      balancedGraph.AddMatching(expandedPath[i], expandedPath[i + 1]);
    }
  }
}