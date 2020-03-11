using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Ninject;
using PresentationMergeTool.BusinessLayer;
using PresentationMergeTool.Contracts;
using PresentationMergeTool.FileAccess;
using PresentationMergeTool.Infrastructure;
using PresentationMergeTool.PresentationLayer.ViewModels;
using PresentationMergeTool.PresentationLayer.Views;

namespace PresentationMergeTool.PresentationLayer
{
  /// <summary>
  ///   Interaction logic for App.xaml
  /// </summary>
  [ExcludeFromCodeCoverage]
  public partial class App : Application
  {
    private IKernel container;

    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      this.ConfigureContainer();
      this.ComposeObjects();
      Current.MainWindow.Show();
    }

    private void ComposeObjects()
    {
      Current.MainWindow = this.container.Get<MainWindow>();
    }

    private void ConfigureContainer()
    {
      this.container = new StandardKernel();
      this.container.Bind<MainWindowViewModel>().ToSelf();
      this.container.Bind<IBusinessLogic>().To<BusinessLogic>().InSingletonScope();
      this.container.Bind<IFileAccessUtils>().To<FileAccessUtils>();
      this.container.Bind<IConfigManager>().To<ConfigManager>();
      this.container.Bind<IPowerPointAccess>().To<PowerPointAccess>();
      this.container.Bind<IDialogManager>().To<DialogManager>();
      this.container.Bind<ITaskManager>().To<TaskManager>();
      //TODO: Configure all dependencies here !
    }
  }
}