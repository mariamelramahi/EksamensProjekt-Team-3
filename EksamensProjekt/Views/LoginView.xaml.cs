using EksamensProjekt.ViewModels;
using System.Windows;

namespace EksamensProjekt;

/// <summary>
/// Interaction logic for LoginView.xaml
/// </summary>
public partial class LoginView : Window

{
    public LoginView(LoginViewModel lvm)
    {
        InitializeComponent();
        this.DataContext = lvm;
    }




    private void Button_Click(object sender, RoutedEventArgs e)
    {
        TenancyView _TenancyView = new();
        this.Close();
        _TenancyView.Show();
    }

}