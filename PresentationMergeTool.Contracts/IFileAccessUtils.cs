using System.Collections.Generic;
using System.IO;

namespace PresentationMergeTool.Contracts
{
  public interface IFileAccessUtils
  {
    /// <summary>
    /// Checks if the file under the path exists.
    /// </summary>
    /// <param name="inputFile">Full path to the file</param>
    /// <returns> false if any error occurs while trying to determine if the specified file exists and true if the file was found</returns>
    bool FileExists(string inputFile);

    /// <summary>
    /// Determines whether the given path refers to an existing directory on disk.
    /// </summary>
    /// <param name="inputFile">Full path to the directory</param>
    /// <returns> true if <paramref name="inputFile" /> refers to an existing directory, otherwise false</returns>
    bool DirectoryExists(string inputFile);

    /// <summary>
    /// Returns the extension of the specified path string.
    /// </summary>
    /// <param name="inputFile">The path string from which to get the extension.</param>
    /// <returns>Extension without the starting `.` (dot) character. If the <paramref name="inputFile" /> is NULL returns NULL. If the <paramref name="inputFile" /> is empty or has no extension returns String.Empty </returns>
    string GetFileExtensionWithoutDot(string inputFile);

    /// <summary>
    /// Returns all files within a <param name="inputFile"/> folder that match any of the <param name="validExtensions"/> extensions. 
    /// </summary>
    /// <param name="inputFile">Full path to the directory</param>
    /// <param name="validExtensions">A collection of extensions without empty spaces or dots. Entry example "PPT"</param>
    /// <param name="searchOption">Specifies whether the subfolders should be included</param>
    /// <returns>Returns an empty collection of FileInfo if <param name="validExtensions"/> is empty, otherwise all files as FileInfo that match the search criteria</returns>
    IEnumerable<FileInfo> GetFilesFromDirectory(string inputFile, IEnumerable<string> validExtensions, SearchOption searchOption);

    /// <summary>
    /// Returns the file name to the given full path.
    /// </summary>
    /// <param name="fullFileName">Full path to the file.</param>
    /// <returns>File name</returns>
    string GetFileName(string fullFileName);
  }
}