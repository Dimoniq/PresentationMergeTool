using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PresentationMergeTool.BusinessObjects;

namespace PresentationMergeTool.UnitTests.BusinessObjects
{
  [TestClass, ExcludeFromCodeCoverage]
  public class PresentationModelTests
  {
    [TestMethod]
    public void ConstructorTest()
    {
      //Assert
      var path = "FullPath";
      var name = "Name";
      //Act
      var model = new PresentationModel(path, name);

      //Assert
      Assert.AreEqual(path, model.FullPath);
      Assert.AreEqual(name, model.Name);
    }

    [TestMethod]
    public void EqualsTest()
    {
      //Arrange
      var modelOne = new PresentationModel("Path", "Name");
      var modelTwo = new PresentationModel("Path", "Name");
      var modelThree = new PresentationModel("AnotherPath", "Name");

      //Assert
      Assert.IsTrue(modelOne.Equals(modelTwo));
      Assert.IsFalse(modelOne.Equals(modelThree));
      Assert.IsFalse(modelThree.Equals(modelTwo));
      Assert.IsFalse(modelOne.Equals(null));
    }

    [TestMethod]
    public void GetHashCodeTest()
    {
      //Arrange
      var modelOne = new PresentationModel("Path", "Name");
      var modelTwo = new PresentationModel("Path", "Name");

      //Assert
      Assert.AreEqual(modelOne.GetHashCode(), modelTwo.GetHashCode());
      Assert.AreEqual(0, new PresentationModel(null, null).GetHashCode());
    }
  }
}