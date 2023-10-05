using System;
namespace Graphs;

public class Vertex
{
  public int Id;
  public int Index
  {
    get { return this.Id - 1; }
  }
  public int Color;
  public int Label;

  public Vertex(Vertex vertex)
  {
    this.Id = vertex.Id;
    this.Color = vertex.Color;
    this.Label = vertex.Label;
  }

  public Vertex(int id, int color = -1)
  {
    this.Id = id;
    this.Color = color;
    this.Label = 0;
  }

  public void SetInvalidId()
  {
    Id = -1;
  }

  public bool HasColor()
  {
    return Color >= 0;
  }

  public bool HasValidId()
  {
    return Id > 0;
  }

  public override string ToString()
  {
    return this.Id.ToString();
  }

  public override int GetHashCode()
  {
    return this.Id;
  }

  public override bool Equals(Object? obj)
  {
    if (obj is Vertex)
    {
      return this.Id == ((Vertex)obj).Id;
    }
    if (obj is int)
    {
      return this.Index == (int)obj;
    }
    return false;
  }
}