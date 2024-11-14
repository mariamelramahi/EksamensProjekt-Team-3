using EksamensProjekt.Services;
using EksamensProjekt.Services.Navigation;
using EksamensProjekt.Utilities;

namespace EksamensProjekt.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly AuthLogin _authLogin;
    private readonly INavigationService _navigationService;

    // Constructor - Dependency injection in App
    public LoginViewModel(AuthLogin authLogin, INavigationService navigationService)
    {
        _authLogin = authLogin;
        _navigationService = navigationService;

        // Initialize commands
        LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
        LogoutCommand = new RelayCommand(ExecuteLogout); // OBS: MOVE THIS TO THE OTHER VIEWMODELS
    }

    // Fields
    private string _usernameInput;
    private string _passwordInput;
    private string _loginErrorMessage;
    private bool _isLoginErrorVisible;

    // Properties
    public string UsernameInput
    {
        get => _usernameInput;
        set
        {
            _usernameInput = value;
            OnPropertyChanged();                   // Update UI about changes in value
            LoginCommand.RaiseCanExecuteChanged(); // Update CanExecute state for LoginCommand (CanExecuteLogin)
        }
    }

    public string PasswordInput
    {
        get => _passwordInput;
        set
        {
            _passwordInput = value;
            OnPropertyChanged();                   
            LoginCommand.RaiseCanExecuteChanged(); 
        }
    }

    public string LoginErrorMessage
    {
        get => _loginErrorMessage;
        set
        {
            _loginErrorMessage = value;
            OnPropertyChanged();
        }
    }

    public bool IsLoginErrorVisible
    {
        get => _isLoginErrorVisible;
        set
        {
            _isLoginErrorVisible = value;
            OnPropertyChanged();
        }
    }

    // Commands
    public RelayCommand LoginCommand { get; }
    public RelayCommand LogoutCommand { get; }

    // Command Methods
    private void ExecuteLogin()
    {
        if (_authLogin.ValidateLogin(UsernameInput, PasswordInput))
        {
            IsLoginErrorVisible = false;
            _navigationService.NavigateTo<TenancyView>();
        }
        else
        {
            LoginErrorMessage = "Invalid username or password. Please try again.";
            IsLoginErrorVisible = true;
        }
    }

    private bool CanExecuteLogin()
    {
        return !string.IsNullOrEmpty(UsernameInput) && !string.IsNullOrEmpty(PasswordInput);
    }

    private void ExecuteLogout()
    {
        // Pass a factory method that creates a new instance of LoginView with dependencies
        _navigationService.NavigateTo(() => new LoginView(new LoginViewModel(_authService, _navigationService)));
    }
}