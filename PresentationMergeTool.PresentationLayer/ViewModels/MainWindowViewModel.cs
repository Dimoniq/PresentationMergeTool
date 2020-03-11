using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight.Command;
using PresentationMergeTool.BusinessObjects;
using PresentationMergeTool.Contracts;
using PresentationMergeTool.PresentationLayer.Annotations;
using PresentationMergeTool.PresentationLayer.Properties;

namespace PresentationMergeTool.PresentationLayer.ViewModels
{
  public class MainWindowViewModel : INotifyPropertyChanged
  {
    private readonly IBusinessLogic businessLogic;
    private readonly IDialogManager dialogManager;
    private bool isHelpAreaExpanded;

    public MainWindowViewModel(IBusinessLogic businessLogic, IDialogManager dialogManager)
    {
      this.businessLogic = businessLogic;
      this.dialogManager = dialogManager;
      this.PresentationFiles = new ObservableCollection<PresentationModel>();
      this.PresentationFiles.CollectionChanged +=
        (sender, args) => this.SaveToPresentationCommand.RaiseCanExecuteChanged();

      this.ProcessDropCommand = new RelayCommand<string[]>(this.ProcessDropEvent);
      this.DeleteSelectedItemCommand = new RelayCommand<IEnumerable>(this.DeleteSelectedPresentationItem);
      this.SaveToPresentationCommand = new RelayCommand(this.SaveToPresentation, () => this.PresentationFiles.Any());
      this.InvertHelpAreaVisibilityCommand = new RelayCommand(this.InvertHelpAreaVisibility);
      this.AppVersion = $"{Resources.AppVersionTemplate} - {Assembly.GetExecutingAssembly().GetName().Version}";
    }

    public RelayCommand<string[]> ProcessDropCommand { get; }
    public RelayCommand<IEnumerable> DeleteSelectedItemCommand { get; }
    public RelayCommand SaveToPresentationCommand { get; }
    public RelayCommand InvertHelpAreaVisibilityCommand { get; }
    public ObservableCollection<PresentationModel> PresentationFiles { get; set; }
    public string AppVersion { get; }

    public bool IsHelpAreaExpanded
    {
      get => this.isHelpAreaExpanded;
      set
      {
        this.isHelpAreaExpanded = value;
        this.OnPropertyChanged();
      }
    }

    private void InvertHelpAreaVisibility()
    {
      this.IsHelpAreaExpanded = !this.IsHelpAreaExpanded;
    }

    private void SaveToPresentation()
    {
      var exportFullFileName = this.dialogManager.ShowSaveAsDialog(Resources.SaveAs_ExtensionFilter);
      if (string.IsNullOrEmpty(exportFullFileName))
      {
        return;
      }

      if (!this.dialogManager.ShowConfirmationDialog(Resources.Warning_DoNotUseKeyboardOrMouse))
      {
        return;
      }

      try
      {
        this.businessLogic.SavePresentationsAs(this.PresentationFiles.Select(p => p.FullPath).ToList(),
          exportFullFileName);
        this.dialogManager.ShowInfoMessage(Resources.Export_Successful);
      }
      catch (Exception e)
      {
        this.dialogManager.ShowErrorMessage(e.Message);
      }
    }

    private void DeleteSelectedPresentationItem(IEnumerable selectedItems)
    {
      var list = selectedItems.Cast<PresentationModel>().ToList();
      list.ForEach(f => this.PresentationFiles.Remove(f));
      this.SaveToPresentationCommand.RaiseCanExecuteChanged();
    }

    private void ProcessDropEvent(string[] droppedFiles)
    {
      if (droppedFiles == null || !droppedFiles.Any())
      {
        return;
      }


      var validFilesToProcess = this.businessLogic.GetValidPresentationFiles(droppedFiles);
      foreach (var presentation in validFilesToProcess)
      {
        if (!this.PresentationFiles.Contains(presentation))
        {
          this.PresentationFiles.Add(presentation);
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator, ExcludeFromCodeCoverage]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}