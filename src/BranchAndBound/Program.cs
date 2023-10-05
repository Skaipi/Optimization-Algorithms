using System;
using System.Collections.Generic;

using Graphs;

namespace BranchAndBound;
class Program
{
  static void Main(string[] args)
  {
    Loader loader = new Loader("../../Wykład/comivoyager_b&b.txt");
    // Loader loader = new Loader("../../Wykład/comivoyager.txt");
    // Loader loader = new Loader("../../Maksymalne skojarzenie/maksymalne_skojarzenie1.txt");
    var loadedGraph = loader.loadMatrixGraph();
    if (!loadedGraph.IsDirected)
      loadedGraph = loadedGraph.Copy(true);

    var minSolutionCost = 0;
    minSolutionCost += ReduceRows(loadedGraph);
    minSolutionCost += ReduceCols(loadedGraph);

    PriorityQueue<Solution<Vertex>, int> solutions = new PriorityQueue<Solution<Vertex>, int>();
    solutions.Enqueue(new Solution<Vertex>(loadedGraph, minSolutionCost, new List<(Vertex, Vertex)>()), minSolutionCost);
    var solution = solutions.Dequeue();

    while (HasAnyConnections(solution.Graph))
    {
      var nextBranch = FindNextBranch(solution.Graph);

      Solution<Vertex> solutionWithNextBranch = GetSolutionWithNextBranch(solution, nextBranch);
      solutions.Enqueue(solutionWithNextBranch, solutionWithNextBranch.Cost);

      Solution<Vertex> solutionWithoutNextBranch = GetSolutionWithoutNextBranch(solution, nextBranch);
      solutions.Enqueue(solutionWithoutNextBranch, solutionWithoutNextBranch.Cost);

      solution = solutions.Dequeue();
    }

    Console.Write("Voyage Voyage:");
    foreach (var vertex in solution.GetCycle())
    {
      Console.Write($" {vertex}");
    }
    Console.WriteLine($" (with cost: {solution.Cost})");
  }

  private static Solution<T> GetSolutionWithNextBranch<T>(Solution<T> currentSolution, (T Src, T Dst) nextBranch) where T : Vertex
  {
    int costWithNextBranch = currentSolution.Cost;
    Graph<T> graphWithNextBranch = currentSolution.Graph.Copy();
    foreach (var conn in graphWithNextBranch.GetAdjacentConnections(nextBranch.Src))
      graphWithNextBranch.RemoveEdge(nextBranch.Src, conn.Vertex);
    foreach (var conn in graphWithNextBranch.GetConnectionsTo(nextBranch.Dst))
      graphWithNextBranch.RemoveEdge(conn.Vertex, nextBranch.Dst);
    graphWithNextBranch.RemoveEdge(nextBranch.Dst, nextBranch.Src);
    costWithNextBranch += ReduceRows(graphWithNextBranch);
    costWithNextBranch += ReduceCols(graphWithNextBranch);
    return currentSolution.Update(graphWithNextBranch, costWithNextBranch, (nextBranch.Src, nextBranch.Dst));
  }

  private static Solution<T> GetSolutionWithoutNextBranch<T>(Solution<T> currentSolution, (T Src, T Dst) nextBranch) where T : Vertex
  {
    int costWithoutNextBranch = currentSolution.Cost;
    Graph<T> graphWithoutNextBranch = currentSolution.Graph.Copy();
    graphWithoutNextBranch.RemoveEdge(nextBranch.Src, nextBranch.Dst);
    costWithoutNextBranch += ReduceRows(graphWithoutNextBranch);
    costWithoutNextBranch += ReduceCols(graphWithoutNextBranch);
    return currentSolution.Update(graphWithoutNextBranch, costWithoutNextBranch);
  }

  private static int ReduceRows<T>(Graph<T> graph) where T : Vertex
  {
    var totalCostReduction = 0;
    foreach (var vertex in graph)
    {
      var minRowWeight = graph.GetAdjacentConnections(vertex).DefaultIfEmpty().MinBy(conn => conn.Weight).Weight;
      foreach (var conn in graph.GetAdjacentConnections(vertex))
        graph.AddEdge(vertex, conn.Vertex, conn.Weight - minRowWeight);
      totalCostReduction += minRowWeight;
    }
    return totalCostReduction;
  }

  private static int ReduceCols<T>(Graph<T> graph) where T : Vertex
  {
    var totalCostReduction = 0;
    foreach (var vertex in graph)
    {
      var minColWeight = graph.GetConnectionsTo(vertex).DefaultIfEmpty().MinBy(conn => conn.Weight).Weight;
      foreach (var conn in graph.GetConnectionsTo(vertex))
        graph.AddEdge(conn.Vertex, vertex, conn.Weight - minColWeight);
      totalCostReduction += minColWeight;
    }
    return totalCostReduction;
  }

  private static (T Src, T Dst) FindNextBranch<T>(Graph<T> graph) where T : Vertex
  {
    var nonTrivialMinimums = GetNonTrivialMinimums(graph);

    if (nonTrivialMinimums.Count > 0)
    {
      T resultColumn = nonTrivialMinimums.MaxBy(conn => conn.Weight).Column;
      T resultRow = graph.GetConnectionsTo(resultColumn).MinBy(conn => conn.Weight).Vertex;
      return (resultRow, resultColumn);
    }
    else
    {
      foreach (var vertex in graph)
      {
        if (graph.GetConnectionsTo(vertex).Count > 0)
        {
          T resultRow = graph.GetConnectionsTo(vertex)[0].Vertex;
          T resultColumn = vertex;
          return (resultRow, resultColumn);
        }
      }
    }

    throw new ArgumentException("Graph does not contain next branch");
  }

  private static List<(T Column, int Weight)> GetNonTrivialMinimums<T>(Graph<T> graph) where T : Vertex
  {
    var nonTrivialMinimums = new List<(T Column, int Weight)>();
    foreach (var vertex in graph)
    {
      var minimum = new List<int>(graph.GetConnectionsTo(vertex).Select(conn => conn.Weight));
      minimum.Sort();

      if (minimum.Count > 1)
        nonTrivialMinimums.Add((vertex, minimum[1]));
    }
    return nonTrivialMinimums;
  }

  private static bool HasAnyConnections<T>(Graph<T> graph) where T : Vertex
  {
    var result = false;
    foreach (var vertex in graph)
      if (graph.GetAdjacentConnections(vertex).Count > 0)
        result = true;
    return result;
  }

  private struct Solution<T> where T : Vertex
  {
    public Graph<T> Graph { get; }
    public int Cost { get; }
    public List<(T V1, T V2)> Edges { get; }
    public Solution(Graph<T> graph, int cost, List<(T, T)> edges)
    {
      Graph = graph;
      Cost = cost;
      Edges = edges;
    }

    public Solution<T> Update(Graph<T> graph, int cost, (T, T) edge = default)
    {
      var newEdges = new List<(T, T)>(Edges);
      if (edge != default)
        newEdges.Add(edge);
      return new Solution<T>(graph, cost, newEdges);
    }

    public List<T> GetCycle()
    {
      var localEdges = Edges;
      var startingEdge = Edges.Where(edge => localEdges.Find(other => other.V2 == edge.V1) == default).ToList()[0];
      var start = startingEdge.V1;
      var nextStart = startingEdge.V2;
      var solution = new List<T>();

      solution.Add(start);
      while (nextStart != null)
      {
        solution.Add(nextStart);
        nextStart = Edges.Find(edge => edge.V1.Equals(nextStart)).V2;
      }
      solution.Add(start);
      return solution;
    }
  }
}
