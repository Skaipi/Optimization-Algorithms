using System;
using System.Collections.Generic;
using System.Linq;

using Graphs;

namespace Genetic;
class Program
{
  static void Main(string[] args)
  {
    // Loader loader = new Loader("../../Wykład/comivoyager_b&b.txt");
    Loader loader = new Loader("../../Wykład/comivoyager.txt");
    var loadedGraph = loader.loadMatrixGraph();
    if (!loadedGraph.IsDirected)
      loadedGraph = loadedGraph.Copy(true);
    GraphLogger.Display(loadedGraph);

    var random = new Random();
    int generationsAmount = 2;
    int populationSize = 10;
    int elitism = (int)Math.Ceiling(populationSize / 10.0);
    int minFitness = (int)(populationSize / 2.0);

    List<Chromosome<Vertex>> population = new List<Chromosome<Vertex>>();
    for (int i = 0; i < populationSize; i++)
      population.Add(Chromosome<Vertex>.GetRandomChromosome(loadedGraph));

    for (int i = 0; i < generationsAmount; i++)
    {
      var nextPopulation = new List<Chromosome<Vertex>>();
      population = population.OrderBy(chromosome => chromosome.GetFitnessScore()).ToList();

      for (int j = 0; j < elitism; j++)
      {
        nextPopulation.Add(population[j]);
      }

      for (int j = elitism; j < populationSize; j++)
      {
        var parent1 = population[random.Next(minFitness)];
        var parent2 = population[random.Next(minFitness)];
        nextPopulation.Add(parent1.CrossOperator(parent2));
        nextPopulation[j].Mutate();
      }

      population = nextPopulation;
    }

    population = population.OrderBy(permutation => permutation.GetFitnessScore()).ToList();
    var bestSolution = population[0];

    Console.Write("Voyage Voyage:");
    foreach (var vertex in bestSolution.genes)
    {
      Console.Write($" {vertex}");
    }
    Console.WriteLine($" (with cost: {bestSolution.GetFitnessScore()})");
  }
}

public class Chromosome<T> where T : Vertex
{
  private Graph<T> graph;
  public List<T> genes;

  public Chromosome(Graph<T> graph, List<T> vertices)
  {
    genes = new List<T>(vertices);
    this.graph = graph;
  }

  public static Chromosome<T> GetRandomChromosome(Graph<T> graph)
  {
    var random = new Random();
    List<T> allVertices = graph.GetAllVertices();
    List<T> permutation = new List<T>();

    for (int i = 0; i < graph.GetAllVertices().Count; i++)
    {
      int index = random.Next(allVertices.Count);
      permutation.Add(allVertices[index]);
      allVertices.RemoveAt(index);
    }

    return new Chromosome<T>(graph, permutation);
  }

  public int GetFitnessScore()
  {
    int totalCost = 0;
    for (int i = 0; i < genes.Count; i++)
    {
      totalCost += graph.GetConnectionWeight(genes[i], genes[(i + 1) % genes.Count]);
    }
    return totalCost;
  }

  public Chromosome<T> CrossOperator(Chromosome<T> other)
  {
    var random = new Random();
    var result = new List<T>();
    var crossoverPoint = random.Next(genes.Count);

    for (int i = 0; i < crossoverPoint; i++)
    {
      result.Add(genes[i]);
    }

    var remainingGenes = other.genes.Except(result).ToList();
    if (remainingGenes.Count <= 0) return new Chromosome<T>(graph, result);

    for (int i = crossoverPoint; i < genes.Count; i++)
    {
      result.Add(remainingGenes[i - crossoverPoint]);
    }

    return new Chromosome<T>(graph, result);
  }

  public void Mutate()
  {
    var random = new Random();
    var mutationProb = random.NextDouble();

    if (mutationProb > 0.5)
    {
      var src = random.Next(genes.Count);
      var dst = random.Next(genes.Count);
      var tmp = genes[src];
      genes[src] = genes[dst];
      genes[dst] = tmp;
    }

    if (mutationProb > 0.9)
    {
      var src = random.Next(genes.Count);
      var dst = random.Next(genes.Count);
      var tmp = genes[src];
      genes[src] = genes[dst];
      genes[dst] = tmp;
    }
  }
}
