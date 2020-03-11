using System.Diagnostics.CodeAnalysis;
using System.Threading;
using PresentationMergeTool.Contracts;

namespace PresentationMergeTool.Infrastructure
{
  [ExcludeFromCodeCoverage]
  public class TaskManager : ITaskManager
  {
    public void Sleep(int timeoutMilliseconds)
    {
      Thread.Sleep(timeoutMilliseconds);
    }
  }
}