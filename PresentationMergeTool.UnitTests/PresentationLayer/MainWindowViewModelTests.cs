using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PresentationMergeTool.BusinessObjects;
using PresentationMergeTool.Contracts;
using PresentationMergeTool.PresentationLayer.ViewModels;

namespace PresentationMergeTool.UnitTests.PresentationLayer
{
  [TestClass, ExcludeFromCodeCoverage]
  public class MainWindowViewModelTests
  {
    [TestMethod]
    public void ConstructorTest()
    {
      //Arrange
      var businessLogic = new Mock<IBusinessLogic>();
      var dialogManager = new Mock<IDialogManager>();

      //Act
      var viewModel = new MainWindowViewModel(businessLogic.Object, dialogManager.Object);

      //Assert
      Assert.IsNotNull(viewModel.AppVersion);
      Assert.IsNotNull(viewModel.DeleteSelectedItemCommand);
      Assert.IsNotNull(viewModel.ProcessDropCommand);
      Assert.IsNotNull(viewModel.SaveToPresentationCommand);
      Assert.IsFalse(viewModel.SaveToPresentationCommand.CanExecute(null));
    }

    [TestMethod]
    public void SaveToPresentationCommandTest_UserSelectsNoPath()
    {
      //Arrange
      var businessLogic = new Mock<IBusinessLogic>();
      var dialogManager = new Mock<IDialogManager>();
      dialogManager.Setup(dm => dm.ShowSaveAsDialog(It.IsAny<string>())).Returns((string) null);
      var viewModel = new MainWindowViewModel(businessLogic.Object, dialogManager.Object);
      viewModel.PresentationFiles = new ObservableCollection<PresentationModel> {new PresentationModel("path", "name")};

      //Act
      viewModel.SaveToPresentationCommand.Execute(null);

      //Assert
      dialogManager.Verify(dm => dm.ShowConfirmationDialog(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public void SaveToPresentationCommandTest_UserRejectsTheConfirmationDialog()
    {
      //Arrange
      var businessLogic = new Mock<IBusinessLogic>();
      var dialogManager = new Mock<IDialogManager>();
      dialogManager.Setup(dm => dm.ShowSaveAsDialog(It.IsAny<string>())).Returns("some path");
      dialogManager.Setup(dm => dm.ShowConfirmationDialog(It.IsAny<string>())).Returns(false);
      var viewModel = new MainWindowViewModel(businessLogic.Object, dialogManager.Object);
      viewModel.PresentationFiles = new ObservableCollection<PresentationModel> {new PresentationModel("path", "name")};

      //Act
      viewModel.SaveToPresentationCommand.Execute(null);

      //Assert
      dialogManager.Verify(dm => dm.ShowConfirmationDialog(It.IsAny<string>()), Times.Once);
      businessLogic.Verify(bl => bl.SavePresentationsAs(It.IsAny<List<string>>(), It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public void SaveToPresentationCommandTest_SuccessfulExport()
    {
      //Arrange
      var businessLogic = new Mock<IBusinessLogic>();
      var dialogManager = new Mock<IDialogManager>();
      dialogManager.Setup(dm => dm.ShowSaveAsDialog(It.IsAny<string>())).Returns("some path");
      dialogManager.Setup(dm => dm.ShowConfirmationDialog(It.IsAny<string>())).Returns(true);
      var viewModel = new MainWindowViewModel(businessLogic.Object, dialogManager.Object);
      viewModel.PresentationFiles = new ObservableCollection<PresentationModel> {new PresentationModel("path", "name")};

      //Act
      viewModel.SaveToPresentationCommand.Execute(null);

      //Assert
      dialogManager.Verify(dm => dm.ShowConfirmationDialog(It.IsAny<string>()), Times.Once);
      businessLogic.Verify(bl => bl.SavePresentationsAs(It.IsAny<List<string>>(), It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void SaveToPresentationCommandTest_ExportFails()
    {
      //Arrange
      var businessLogic = new Mock<IBusinessLogic>();
      var dialogManager = new Mock<IDialogManager>();
      dialogManager.Setup(dm => dm.ShowSaveAsDialog(It.IsAny<string>())).Returns("some path");
      dialogManager.Setup(dm => dm.ShowConfirmationDialog(It.IsAny<string>())).Returns(true);
      businessLogic.Setup(bl => bl.SavePresentationsAs(It.IsAny<List<string>>(), It.IsAny<string>()))
        .Throws(new Exception("TestMessage"));
      var viewModel = new MainWindowViewModel(businessLogic.Object, dialogManager.Object);
      viewModel.PresentationFiles = new ObservableCollection<PresentationModel> {new PresentationModel("path", "name")};

      //Act
      viewModel.SaveToPresentationCommand.Execute(null);

      //Assert
      dialogManager.Verify(dm => dm.ShowConfirmationDialog(It.IsAny<string>()), Times.Once);
      businessLogic.Verify(bl => bl.SavePresentationsAs(It.IsAny<List<string>>(), It.IsAny<string>()), Times.Once);
      dialogManager.Verify(dm => dm.ShowErrorMessage("TestMessage"), Times.Once);
    }

    [TestMethod]
    public void DeleteSelectedItemCommandTest()
    {
      //Arrange
      var businessLogic = new Mock<IBusinessLogic>();
      var dialogManager = new Mock<IDialogManager>();
      var viewModel = new MainWindowViewModel(businessLogic.Object, dialogManager.Object);
      var item1 = new PresentationModel("path", "name");
      var item2 = new PresentationModel("path2", "name2");
      var item3 = new PresentationModel("path3", "name3");
      viewModel.PresentationFiles = new ObservableCollection<PresentationModel>
      {
        item1, item2, item3
      };

      var selectedItems = new List<PresentationModel> {item2};


      //Act
      viewModel.DeleteSelectedItemCommand.Execute(selectedItems);

      //Assert
      Assert.AreEqual(2, viewModel.PresentationFiles.Count);
      Assert.AreEqual(item1.FullPath, viewModel.PresentationFiles[0].FullPath);
      Assert.AreEqual(item3.FullPath, viewModel.PresentationFiles[1].FullPath);
    }

    [TestMethod]
    public void ProcessDropCommandTest()
    {
      //Arrange
      var businessLogic = new Mock<IBusinessLogic>();
      var dialogManager = new Mock<IDialogManager>();
      int collectionChangedCount = 0;
      var viewModel = new MainWindowViewModel(businessLogic.Object, dialogManager.Object);
      viewModel.PresentationFiles.Add(new PresentationModel("File1", "Name1"));
      viewModel.PresentationFiles.Add(new PresentationModel("File3", "Name3"));
      viewModel.PresentationFiles.Add(new PresentationModel("File5", "Name5"));
      viewModel.PresentationFiles.CollectionChanged += (sender, args) => collectionChangedCount++;

      var fileList = new[] {"File1", "File2", "File3"};
      businessLogic.Setup(bl => bl.GetValidPresentationFiles(fileList)).Returns(
        new List<PresentationModel>
        {
          new PresentationModel("File1", "Name1"),
          new PresentationModel("File2", "Name2") //only this file should be added
        });


      //Act
      viewModel.ProcessDropCommand.Execute(fileList);

      //Assert
      Assert.AreEqual(4, viewModel.PresentationFiles.Count);
      Assert.AreEqual("File2", viewModel.PresentationFiles[3].FullPath);
      Assert.AreEqual(1, collectionChangedCount);
    }

    [TestMethod]
    public void ProcessDropCommandTest_NoFilesDropped()
    {
      //Arrange
      var businessLogic = new Mock<IBusinessLogic>();
      var dialogManager = new Mock<IDialogManager>();
      var viewModel = new MainWindowViewModel(businessLogic.Object, dialogManager.Object);
      viewModel.PresentationFiles.Add(new PresentationModel("File1", "Name1"));
      viewModel.PresentationFiles.Add(new PresentationModel("File3", "Name3"));
      viewModel.PresentationFiles.Add(new PresentationModel("File5", "Name5"));

      //Act
      viewModel.ProcessDropCommand.Execute(null);
      viewModel.ProcessDropCommand.Execute(new string[0]);

      //Assert
      Assert.AreEqual(3, viewModel.PresentationFiles.Count);
      businessLogic.Verify(bl => bl.GetValidPresentationFiles(It.IsAny<string[]>()), Times.Never);
    }

    [TestMethod]
    public void InvertHelpAreaVisibilityCommandTest()
    {
      //Arrange
      var businessLogic = new Mock<IBusinessLogic>();
      var dialogManager = new Mock<IDialogManager>();
      var viewModel = new MainWindowViewModel(businessLogic.Object, dialogManager.Object);

      //Act
      bool previousVisibility = viewModel.IsHelpAreaExpanded;
      viewModel.InvertHelpAreaVisibilityCommand.Execute(null);

      //Assert
      Assert.IsFalse(previousVisibility);
      Assert.IsTrue(viewModel.IsHelpAreaExpanded);
    }
  }
}