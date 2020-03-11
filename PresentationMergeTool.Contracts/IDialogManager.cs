namespace PresentationMergeTool.Contracts
{
  public interface IDialogManager
  {
    string ShowSaveAsDialog(string fileExtensionFilter);
    void ShowInfoMessage(string message);
    void ShowErrorMessage(string message);
    bool ShowConfirmationDialog(string message);
  }
}