using System.Collections;

namespace Graphs;

internal class GraphEnumerator<T> : IEnumerator<T> where T : Vertex
{
  private AbstractGraph<T> instance;
  private int vertexCursor = -1;

  public GraphEnumerator(AbstractGraph<T> instance)
  {
    this.instance = instance;
  }

  public T Current
  {
    get { return instance.Vertices[vertexCursor]; }
  }

  private object Current1
  {
    get { return this.Current; }
  }

  object IEnumerator.Current
  {
    get { return Current1; }
  }

  public bool MoveNext()
  {
    vertexCursor++;
    return (vertexCursor < instance.Vertices.Count);
  }

  public void Reset()
  {
    vertexCursor = -1;
  }

  private bool disposed = false;

  public void Dispose()
  {
    Dispose(disposing: true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!this.disposed)
    {
      if (disposing)
      {
        // Dispose managed resources
      }
      disposed = true;
    }
  }

  ~GraphEnumerator()
  {
    Dispose();
  }
}