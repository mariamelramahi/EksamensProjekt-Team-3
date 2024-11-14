using EksamensProjekt.ViewModels;
using System.Windows;

namespace EksamensProjekt;

/// <summary>
/// Interaction logic for TenancyView.xaml
/// </summary>
public partial class TenancyView : Window
{
    public TenancyView(TenancyViewModel tvm)
    {
        InitializeComponent();
        this.DataContext = tvm;
    }
}
