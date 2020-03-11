namespace PresentationMergeTool.Contracts
{
  public interface ITaskManager
  {
    /// <summary>
    /// Forces the current thread to sleep for the provided period of time.
    /// </summary>
    /// <param name="timeoutMilliseconds">Time to sleep in milliseconds</param>
    void Sleep(int timeoutMilliseconds);
  }
}