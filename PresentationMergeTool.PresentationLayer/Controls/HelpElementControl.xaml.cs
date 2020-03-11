using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PresentationMergeTool.PresentationLayer.Controls
{
  /// <summary>
  ///   Interaction logic for HelpElementControl.xaml
  /// </summary>
  public partial class HelpElementControl : UserControl
  {
    public static readonly DependencyProperty HeaderProperty =
      DependencyProperty.Register("Header", typeof(string), typeof(HelpElementControl));

    public static readonly DependencyProperty ImageSourceProperty =
      DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(HelpElementControl));

    public HelpElementControl()
    {
      this.InitializeComponent();
      this.DataContext = this;
    }

    public string Header
    {
      get => (string) this.GetValue(HeaderProperty);
      set => this.SetValue(HeaderProperty, value);
    }


    public ImageSource ImageSource
    {
      get => (ImageSource) this.GetValue(ImageSourceProperty);
      set => this.SetValue(ImageSourceProperty, value);
    }
  }
}