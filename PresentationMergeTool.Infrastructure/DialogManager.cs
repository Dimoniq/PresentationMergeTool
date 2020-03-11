using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Microsoft.Win32;
using PresentationMergeTool.Contracts;
using PresentationMergeTool.Infrastructure.Properties;

namespace PresentationMergeTool.Infrastructure
{
  [ExcludeFromCodeCoverage]
  public class DialogManager : IDialogManager
  {
    public string ShowSaveAsDialog(string fileExtensionFilter)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog {Filter = fileExtensionFilter};
      if (saveFileDialog.ShowDialog() == true)
      {
        return saveFileDialog.FileName;
      }

      return null;
    }

    public void ShowInfoMessage(string message)
    {
      MessageBox.Show(message, Resources.DialMng_MessageBox_Information_Title, MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public void ShowErrorMessage(string message)
    {
      MessageBox.Show(message, Resources.DialMng_MessageBox_Error_Title, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public bool ShowConfirmationDialog(string message)
    {
      MessageBoxResult messageBoxResult = MessageBox.Show(message, Resources.DialMng_MessageBox_Confirmation_Title, MessageBoxButton.YesNo, MessageBoxImage.Question);
      return messageBoxResult == MessageBoxResult.Yes;
    }
  }
}