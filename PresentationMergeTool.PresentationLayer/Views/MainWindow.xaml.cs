using System.Diagnostics.CodeAnalysis;
using System.Windows;
using PresentationMergeTool.PresentationLayer.ViewModels;

namespace PresentationMergeTool.PresentationLayer.Views
{
  /// <summary>
  ///   Interaction logic for MainWindow.xaml
  /// </summary>
  [ExcludeFromCodeCoverage]
  public partial class MainWindow : Window
  {
    //The delegate will return the position of the DragDropEventArgs and the MouseButtonEventArgs event objects
    public delegate Point GetDragDropPosition(IInputElement inputElement);

    public MainWindow(MainWindowViewModel viewModel)
    {
      this.DataContext = viewModel;
      this.InitializeComponent();
    }
  }
}