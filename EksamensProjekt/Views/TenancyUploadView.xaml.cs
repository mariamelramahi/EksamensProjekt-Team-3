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
}
