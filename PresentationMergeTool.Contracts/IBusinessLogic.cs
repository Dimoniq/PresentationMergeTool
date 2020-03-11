using System.Collections.Generic;
using PresentationMergeTool.BusinessObjects;

namespace PresentationMergeTool.Contracts
{
  public interface IBusinessLogic
  {
    /// <summary>
    /// Returns only those presentations from the input list, that match the business rules.
    /// </summary>
    /// <param name="inputFiles">List  of full paths to the presentations </param>
    /// <returns></returns>Filtered list of full paths to the presentations
    IEnumerable<PresentationModel> GetValidPresentationFiles(IEnumerable<string> inputFiles);

    /// <summary>
    /// Copies all slides from source presentations to the destination presentation.
    /// </summary>
    /// <param name="presentationFullPaths"> List of full paths to the source presentations</param>
    /// <param name="exportFullFileName">Path under which the destination presentation will be created</param>
    void SavePresentationsAs(IEnumerable<string> presentationFullPaths, string exportFullFileName);
  }
}