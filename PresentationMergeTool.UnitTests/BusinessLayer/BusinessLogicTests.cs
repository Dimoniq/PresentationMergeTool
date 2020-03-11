using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PresentationMergeTool.BusinessLayer;
using PresentationMergeTool.Contracts;

namespace PresentationMergeTool.UnitTests.BusinessLayer
{
  [TestClass]
  [ExcludeFromCodeCoverage]
  public class BusinessLogicTests
  {
    [TestMethod]
    public void SavePresentationsAsTest()
    {
      //Arrange
      var fileAccess = new Mock<IFileAccessUtils>();
      var configurationManager = new Mock<IConfigManager>();
      var powerPointAccess = new Mock<IPowerPointAccess>();

      var presentations = new List<string> {"Presentation 1", "Presentation 2"};
      var exportFilePath = "SavePath";

      //Act
      var businessLogic = new BusinessLogic(fileAccess.Object, configurationManager.Object, powerPointAccess.Object);
      businessLogic.SavePresentationsAs(presentations, exportFilePath);

      //Assert
      powerPointAccess.Verify(p => p.SavePresentationsAs(presentations, exportFilePath), Times.Once);
    }

    [TestMethod]
    public void GetValidPresentationFilesTest()
    {
      //Arrange
      var fileAccess = new Mock<IFileAccessUtils>();
      var configurationManager = new Mock<IConfigManager>();
      var powerPointAccess = new Mock<IPowerPointAccess>();

      configurationManager.Setup(cm => cm.GetAllowedPresentationExtensions()).Returns(new[] {"ppt", "pptx"});
      fileAccess.Setup(fa => fa.FileExists("File1")).Returns(true);
      fileAccess.Setup(fa => fa.FileExists("File2")).Returns(true);
      fileAccess.Setup(fa => fa.FileExists("InvalidFile1")).Returns(true);

      fileAccess.Setup(fa => fa.GetFileExtensionWithoutDot("File1")).Returns("pptx"); // will be added
      fileAccess.Setup(fa => fa.GetFileExtensionWithoutDot("File2")).Returns("ppt"); // will be added
      fileAccess.Setup(fa => fa.GetFileExtensionWithoutDot("InvalidFile1")).Returns("mp3"); // invalid extension

      fileAccess.Setup(fa => fa.DirectoryExists("Folder1")).Returns(true);
      fileAccess.Setup(fa => fa.DirectoryExists("InvalidFolder2")).Returns(true);

      fileAccess.Setup(fa =>
          fa.GetFilesFromDirectory("Folder1", It.IsAny<IEnumerable<string>>(), SearchOption.AllDirectories))
        .Returns(new List<FileInfo> {new FileInfo("FileNameFromFolder1"), new FileInfo("FileNameFromFolder2")});

      fileAccess.Setup(fa =>
          fa.GetFilesFromDirectory("InvalidFolder2", It.IsAny<IEnumerable<string>>(), SearchOption.AllDirectories))
        .Returns(new List<FileInfo>());

      var inputPresentations = new List<string>
        {"File1", "File2", "Folder1", "InvalidFolder2", "InvalidFile1", "NonExistingFile"};

      //Act
      var businessLogic = new BusinessLogic(fileAccess.Object, configurationManager.Object, powerPointAccess.Object);
      var validPresentations = businessLogic.GetValidPresentationFiles(inputPresentations);

      //Assert
      Assert.AreEqual(4, validPresentations.Count());
    }
  }
}