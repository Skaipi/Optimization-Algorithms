using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Graphs;

abstract class DFSAbstractMethod<T> where T : Vertex
{
  private ICollection<T> discoveredVertices = new Collection<T>();
  public List<T> PathToDest = new List<T>();

  public ICollection<T> DiscoveredVertices
  {
    get { return discoveredVertices; }
  }

  public bool Run(IGraph<T> graph, T? startVertex, List<T>? dest = null)
  {
    if (startVertex is null) return false;

    ActionOnEnter(startVertex);
    discoveredVertices.Add(startVertex);

    if (dest != null && dest.Contains(startVertex))
    {
      PathToDest.Insert(0, startVertex);
      return true;
    }

    foreach (var neighbor in graph.GetAdjacentVertices(startVertex))
    {
      ActionOnAdjacentVertices(neighbor, startVertex);

      if (!discoveredVertices.Contains(neighbor))
      {
        bool result = Run(graph, neighbor, dest);
        if (result == true)
        {
          PathToDest.Insert(0, startVertex);
          return true;
        }
      }
    }

    return false;
  }

  protected virtual void ActionOnEnter(T vertex) { return; }
  protected virtual void ActionOnAdjacentVertices(T child, T parent) { return; }
}

class DFSAlgorithm<T> : DFSAbstractMethod<T> where T : Vertex
{
  private HashSet<T> discoveredVertices;

  public DFSAlgorithm()
  {
    discoveredVertices = new HashSet<T>();
  }
}

class TestBipartitionDFS<T> : DFSAlgorithm<T> where T : Vertex
{
  public bool isBipartite = true;
  public HashSet<T> LeftVertices = new HashSet<T>();
  public HashSet<T> RightVertices = new HashSet<T>();

  public TestBipartitionDFS() : base()
  {
  }

  protected override void ActionOnEnter(T vertex)
  {
    if (!vertex.HasColor())
    {
      vertex.Color = 0;
      LeftVertices.Add(vertex);
    }
  }

  protected override void ActionOnAdjacentVertices(T child, T parent)
  {
    if (child.HasColor())
    {
      isBipartite = isBipartite && parent.Color != child.Color;
    }
    else
    {
      if (parent.Color == 0)
      {
        child.Color = 1;
        RightVertices.Add(child);
      }
      else if (parent.Color == 1)
      {
        child.Color = 0;
        LeftVertices.Add(child);
      }
    }
  }
}