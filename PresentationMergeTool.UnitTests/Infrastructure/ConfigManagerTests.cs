using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PresentationMergeTool.Infrastructure;

namespace PresentationMergeTool.UnitTests.Infrastructure
{
  [TestClass, ExcludeFromCodeCoverage]
  public class ConfigManagerTests
  {
    private const string ConfigFilePath = "./Infrastructure/ConfigFiles/";

    [TestMethod]
    public void GetAllowedPresentationExtensionsTest_Valid()
    {
      //Arrange
      string[] allowedPresentationExtensions;
      var configManager = new ConfigManager();

      //Act
      using (AppConfig.Change($"{ConfigFilePath}PresentationExtensions.config"))
      {
        allowedPresentationExtensions = configManager.GetAllowedPresentationExtensions().ToArray();
      }


      //Assert
      Assert.AreEqual(2, allowedPresentationExtensions.Length);
      Assert.AreEqual("test", allowedPresentationExtensions[0]);
      Assert.AreEqual("oneMoreTest", allowedPresentationExtensions[1]);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void GetAllowedPresentationExtensionsTest_ConfigMissing()
    {
      //Arrange
      var configManager = new ConfigManager();

      //Act
      using (AppConfig.Change($"{ConfigFilePath}Empty.config"))
      {
        configManager.GetAllowedPresentationExtensions();
      }
    }
  }
}