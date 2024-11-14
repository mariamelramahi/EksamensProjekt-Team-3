using EksamensProjekt.Services.Navigation;
using EksamensProjekt.Services;
using EksamensProjekt.ViewModels;
using System.Windows;

namespace EksamensProjekt;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Create dependencies manually
        INavigationService navigationService = new NavigationService();
        IRepo<User> userRepo = new UserRepo(); 
        AuthLogin authLogin = new AuthLogin(userRepo);

        // Create the LoginViewModel with manually injected dependencies
        var loginViewModel = new LoginViewModel(authLogin, navigationService);

        // Create the LoginView with the ViewModel
        LoginView loginView = new LoginView(loginViewModel);
        loginView.Show();
        
        //Note til LoginView codebehind:
        //public partial class LoginView : Window
        //{
        //    public LoginView(LoginViewModel loginViewModel)
        //    {
        //        InitializeComponent();
        //        DataContext = loginViewModel;
        //    }
        //}

    }



}
