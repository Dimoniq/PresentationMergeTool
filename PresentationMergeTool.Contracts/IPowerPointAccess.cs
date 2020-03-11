using System.Collections.Generic;

namespace PresentationMergeTool.Contracts
{
  public interface IPowerPointAccess
  {
    /// <summary>
    /// Copies all slides from the source presentations to the destination presentation, created under the provided path.
    /// </summary>
    /// <param name="presentations">A Collection of full paths to the source presentation files</param>
    /// <param name="exportFullFileName">Full path of the destination presentation, that will be created</param>
    void SavePresentationsAs(IEnumerable<string> presentations, string exportFullFileName);
  }
}