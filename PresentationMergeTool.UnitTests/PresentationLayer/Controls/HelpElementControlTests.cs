using System.Diagnostics.CodeAnalysis;
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PresentationMergeTool.PresentationLayer.Controls;

namespace PresentationMergeTool.UnitTests.PresentationLayer.Controls
{
  [TestClass, ExcludeFromCodeCoverage]
  public class HelpElementControlTests
  {
    [TestMethod]
    public void ConstructorTest()
    {
      //Act
      HelpElementControl control = new HelpElementControl();
      control.ImageSource = new BitmapImage();
      control.Header = "Header";

      //Assert
      Assert.IsNotNull(control);
      Assert.IsNotNull(control.ImageSource);
      Assert.AreEqual("Header", control.Header);
    }
  }
}