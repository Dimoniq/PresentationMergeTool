using System.Collections.Generic;

namespace PresentationMergeTool.Contracts
{
  public interface IConfigManager
  {
    /// <summary>
    /// Retrieves the list of presentation extensions that are currently supported.
    /// </summary>
    /// <returns>Array of file extensions</returns>
    IEnumerable<string> GetAllowedPresentationExtensions();
  }
}