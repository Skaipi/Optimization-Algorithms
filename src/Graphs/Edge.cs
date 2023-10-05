namespace Graphs;

public struct Edge<T> where T : Vertex
{
  public Edge(T v1, T v2, int weight)
  {
    V1 = v1;
    V2 = v2;
    Weight = weight;
  }

  public T V1 { get; }
  public T V2 { get; }
  public int Weight { get; }
}