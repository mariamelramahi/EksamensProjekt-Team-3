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
        if (lvm == null)
        {
            //throw new ArgumentNullException(nameof(lvm), "LoginViewModel cannot be null.");
        }

        //MessageBox.Show("LoginViewModel is null. Please check the dependency injection.");
        InitializeComponent();
        this.DataContext = lvm;
    }

}