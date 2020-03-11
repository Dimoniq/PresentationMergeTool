using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PresentationMergeTool.BusinessObjects;
using PresentationMergeTool.Contracts;

namespace PresentationMergeTool.BusinessLayer
{
  public class BusinessLogic : IBusinessLogic
  {
    private readonly IConfigManager configurationManager;
    private readonly IPowerPointAccess powerPointAccess;
    private readonly IFileAccessUtils fileAccess;

    public BusinessLogic(IFileAccessUtils fileAccess, IConfigManager configurationManager,
      IPowerPointAccess powerPointAccess)
    {
      this.fileAccess = fileAccess;
      this.configurationManager = configurationManager;
      this.powerPointAccess = powerPointAccess;
    }

    public IEnumerable<PresentationModel> GetValidPresentationFiles(IEnumerable<string> inputFiles)
    {
      var response = new List<PresentationModel>();
      var allowedExtensions = this.configurationManager.GetAllowedPresentationExtensions().ToList();

      foreach (var inputFile in inputFiles)
      {
        if (this.fileAccess.FileExists(inputFile) &&
            allowedExtensions.Contains(this.fileAccess.GetFileExtensionWithoutDot(inputFile),
              StringComparer.InvariantCultureIgnoreCase))
        {
          response.Add(new PresentationModel(inputFile, this.fileAccess.GetFileName(inputFile)));
        }
        else if (this.fileAccess.DirectoryExists(inputFile))
        {
          var presentationFiles =
            this.fileAccess.GetFilesFromDirectory(inputFile, allowedExtensions, SearchOption.AllDirectories);
          foreach (var presentation in presentationFiles)
          {
            response.Add(new PresentationModel(presentation.FullName, presentation.Name));
          }
        }
      }

      return response;
    }

    public void SavePresentationsAs(IEnumerable<string> presentationFullPaths, string exportFullFileName)
    {
      this.powerPointAccess.SavePresentationsAs(presentationFullPaths, exportFullFileName);
    }
  }
}