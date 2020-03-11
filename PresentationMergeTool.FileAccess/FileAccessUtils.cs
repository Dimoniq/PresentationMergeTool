using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using PresentationMergeTool.Contracts;

namespace PresentationMergeTool.FileAccess
{
  [ExcludeFromCodeCoverage]
  public class FileAccessUtils : IFileAccessUtils
  {
    private const string ExtensionTemplate = "*.{0}";
    public bool FileExists(string inputFile)
    {
      return File.Exists(inputFile);
    }

    public bool DirectoryExists(string inputFile)
    {
      return Directory.Exists(inputFile);
    }

    public string GetFileExtensionWithoutDot(string inputFile)
    {
      var extension = Path.GetExtension(inputFile);
      if (string.IsNullOrWhiteSpace(extension))
      {
        return extension;
      }

      return extension.Substring(1);
    }

    public IEnumerable<FileInfo> GetFilesFromDirectory(string inputFile, IEnumerable<string> validExtensions, SearchOption searchOption)
    {
      var response = new List<FileInfo>();
      var dirInfo = new DirectoryInfo(inputFile);

      foreach (var extension in validExtensions)
      {
        var files = dirInfo.GetFiles(this.CreateSearchPatternFromExtension(extension), searchOption);
        response.AddRange(files);
      }

      return response;
    }

    public string GetFileName(string fullPath)
    {
      return Path.GetFileName(fullPath);
    }

    private string CreateSearchPatternFromExtension(string extension)
    {
      return string.Format(ExtensionTemplate, extension);
    }
  }
}