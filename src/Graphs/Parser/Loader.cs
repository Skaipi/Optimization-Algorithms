using System;
using System.IO;

namespace Graphs;

public class Loader
{
  private readonly StreamReader reader;

  public Loader(String filePath)
  {
    this.reader = new StreamReader(filePath);
  }

  private bool isDigraph()
  {
    string? headerInfo = this.reader.ReadLine();
    if (headerInfo != "#DIGRAPH") { throw new FormatException("Invalid file structure, expected #DIGRAPH header!"); }

    string? headerValue = this.reader.ReadLine();
    if (headerValue is not null)
    {
      return Boolean.Parse(headerValue);
    }
    return false;
  }

  private void parseEdgesHeader()
  {
    string? headerInfo = this.reader.ReadLine();
    if (headerInfo != "#EDGES") { throw new FormatException("Invalid file structure, expected #EDGES header!"); }
  }
  public Graph<Vertex> loadMatrixGraph()
  {
    bool isDirected = this.isDigraph();
    this.parseEdgesHeader();

    List<string> lines = new List<string>();
    HashSet<int> verticesSet = new HashSet<int>();

    string? line;
    while ((line = reader.ReadLine()) != null)
    {
      string[] values = line.Split(" ");
      int vertex1 = Int32.Parse(values[0]);
      int vertex2 = Int32.Parse(values[1]);
      verticesSet.Add(vertex1);
      verticesSet.Add(vertex2);
      lines.Add(line);
    }

    MatrixGraph result = new MatrixGraph(isDirected);

    foreach (string connection in lines)
    {
      string[] values = connection.Split(" ");
      Vertex vertex1 = new Vertex(Int32.Parse(values[0]));
      Vertex vertex2 = new Vertex(Int32.Parse(values[1]));
      int weight = Int32.Parse(values[2]);
      result.AddEdge(vertex1, vertex2, weight);
    }

    return new Graph<Vertex>(result);
  }

  public Graph<AdjacencyVertex> loadAdjacentGraph()
  {
    bool isDirected = this.isDigraph();
    this.parseEdgesHeader();

    List<string> lines = new List<string>();
    HashSet<int> verticesSet = new HashSet<int>();

    string? line;
    while ((line = reader.ReadLine()) != null)
    {
      string[] values = line.Split(" ");
      int vertex1 = Int32.Parse(values[0]);
      int vertex2 = Int32.Parse(values[1]);
      verticesSet.Add(vertex1);
      verticesSet.Add(vertex2);
      lines.Add(line);
    }

    AdjacentGraph result = new AdjacentGraph(isDirected);

    foreach (string connection in lines)
    {
      string[] values = connection.Split(" ");
      AdjacencyVertex vertex1 = new AdjacencyVertex(Int32.Parse(values[0]));
      AdjacencyVertex vertex2 = new AdjacencyVertex(Int32.Parse(values[1]));
      int weight = Int32.Parse(values[2]);
      result.AddEdge(vertex1, vertex2, weight);
    }

    return new Graph<AdjacencyVertex>(result);
  }
}