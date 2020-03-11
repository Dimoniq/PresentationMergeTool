using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Interactivity;

namespace PresentationMergeTool.PresentationLayer.Behaviors
{
  // It's not reasonable to unit test this behavior
  [ExcludeFromCodeCoverage]
  public class WindowReceiveFilesBehavior : Behavior<Window>
  {
    public static readonly DependencyProperty FileDropReceptionAreaProperty =
      DependencyProperty.Register(
        "FileDropReceptionArea", typeof(UIElement), typeof(WindowReceiveFilesBehavior));


    public UIElement FileDropReceptionArea
    {
      get => (UIElement) this.GetValue(FileDropReceptionAreaProperty);
      set => this.SetValue(FileDropReceptionAreaProperty, value);
    }

    protected override void OnAttached()
    {
      this.AssociatedObject.DragEnter += this.OnDragEntered;
      this.AssociatedObject.DragLeave += this.OnDragLeft;
      this.AssociatedObject.Drop += this.OnDragLeft;
    }

    private void OnDragLeft(object sender, DragEventArgs e)
    {
      this.FileDropReceptionArea.Visibility = Visibility.Collapsed;
    }

    private void OnDragEntered(object sender, DragEventArgs e)
    {
      if (e.Data.GetData(DataFormats.FileDrop) != null)
      {
        this.FileDropReceptionArea.Visibility = Visibility.Visible;
      }
    }

    protected override void OnDetaching()
    {
      this.AssociatedObject.DragEnter -= this.OnDragEntered;
      this.AssociatedObject.DragLeave -= this.OnDragLeft;
      this.AssociatedObject.Drop -= this.OnDragLeft;
    }
  }
}