using EksamensProjekt.ViewModels;
using System.Windows;

namespace EksamensProjekt;

/// <summary>
/// Interaction logic for TenancyUpload.xaml
/// </summary>
public partial class TenancyUploadView : Window
{
    public TenancyUploadView(TenancyUploadViewModel tuvm)
    {
        InitializeComponent();
        this.DataContext = tuvm;
    }
    private void DropTextBox_DragOver(object sender, DragEventArgs e)
    {
        if (DataContext is TenancyUploadViewModel viewModel)
        {
            viewModel.DragAndDropService.HandleDragOver(sender, e);
        }
    }

    private void DropTextBox_Drop(object sender, DragEventArgs e)
    {
        if (DataContext is TenancyUploadViewModel viewModel)
        {
            viewModel.DragAndDropService.HandleDrop(sender, e);
        }
    }
}
