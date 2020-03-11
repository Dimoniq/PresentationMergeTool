using System.Diagnostics.CodeAnalysis;
using System.Windows;
using GalaSoft.MvvmLight.Command;

namespace PresentationMergeTool.PresentationLayer.Converter
{
    /// <summary>
    /// The DragEventArgsToDroppedFilesConverter extracts the dropped files from teh DragEventArgs and returns them.
    /// </summary>
    [ExcludeFromCodeCoverage] //Not unit-testable since we cannot create DragEventArgs
    public class DragEventArgsToDroppedFilesConverter : IEventArgsConverter
  {
    public object Convert(object value, object parameter)
    {
      if (value is DragEventArgs dropParams)
      {
        return dropParams.Data.GetData(DataFormats.FileDrop) as string[];
      }

      return default(string[]);
    }
  }
}