//The drag&drop code copied from https://www.dotnetcurry.com/wpf/677/wpf-data-grid-row-drag-drop

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using PresentationMergeTool.BusinessObjects;
using PresentationMergeTool.PresentationLayer.Views;

namespace PresentationMergeTool.PresentationLayer.Behaviors
{
  // It's not reasonable to unit test this behavior
  [ExcludeFromCodeCoverage]
  public class DataGridItemDragNDropBehavior : Behavior<DataGrid>
  {
    private int prevRowIndex = -1;

    protected override void OnAttached()
    {
      this.AssociatedObject.PreviewMouseLeftButtonDown += this.PreviewMouseLeftButtonDown;
      this.AssociatedObject.Drop += this.ItemDropped;
    }

    protected override void OnDetaching()
    {
      this.AssociatedObject.PreviewMouseLeftButtonDown -= this.PreviewMouseLeftButtonDown;
      this.AssociatedObject.Drop -= this.ItemDropped;
    }

    private void PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var dataGrid = sender as DataGrid;
      if (dataGrid == null)
      {
        return;
      }

      this.prevRowIndex = this.GetDataGridItemCurrentRowIndex(dataGrid, e.GetPosition);
      if (this.prevRowIndex < 0)
      {
        return;
      }


      dataGrid.SelectedIndex = this.prevRowIndex;

      var selectedItem = dataGrid.Items[this.prevRowIndex] as PresentationModel;
      if (selectedItem == null)
      {
        return;
      }

      if (DragDrop.DoDragDrop(dataGrid, selectedItem, DragDropEffects.Move) != DragDropEffects.None)
      {
        dataGrid.SelectedItem = selectedItem;
      }
    }


    private void ItemDropped(object sender, DragEventArgs e)
    {
      var dataGrid = sender as DataGrid;

      if (this.prevRowIndex < 0 || dataGrid == null)
      {
        return;
      }

      var index = this.GetDataGridItemCurrentRowIndex(dataGrid, e.GetPosition);
      if (index < 0 || index == this.prevRowIndex)
      {
        return;
      }

      if (dataGrid.ItemsSource is IList items)
      {
        var itemMoved = items[this.prevRowIndex];
        items.RemoveAt(this.prevRowIndex);
        items.Insert(index, itemMoved);
      }
    }


    private int GetDataGridItemCurrentRowIndex(DataGrid grid, MainWindow.GetDragDropPosition pos)
    {
      var curIndex = -1;
      for (var i = 0; i < grid.Items.Count; i++)
      {
        var itm = this.GetDataGridRowItem(grid, i);
        if (this.IsTheMouseOnTargetRow(itm, pos))
        {
          curIndex = i;
          break;
        }
      }

      return curIndex;
    }


    private bool IsTheMouseOnTargetRow(Visual target, MainWindow.GetDragDropPosition position)
    {
      if (target == null)
      {
        return false;
      }

      var posBounds = VisualTreeHelper.GetDescendantBounds(target);
      var mousePos = position((IInputElement) target);
      return posBounds.Contains(mousePos);
    }

    private DataGridRow GetDataGridRowItem(DataGrid grid, int index)
    {
      if (grid.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
      {
        return null;
      }

      return grid.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;
    }
  }
}