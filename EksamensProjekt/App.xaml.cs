using EksamensProjekt.Models;
using EksamensProjekt.Models.Repositories;
using EksamensProjekt.Services;
using EksamensProjekt.Services.Navigation;
using EksamensProjekt.ViewModels;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows;

namespace EksamensProjekt;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    public static IConfiguration AppConfig { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Load configuration
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        AppConfig = builder.Build();

        // Initialize dependencies
        //string connectionString = AppConfig.GetConnectionString("DefaultConnection");
        string connectionString = AppConfig.GetConnectionString("LocalConnection");

        // Repositories
        IRepo<User> userRepo = new UserRepo(connectionString);
        IRepo<Tenancy> tenancyRepo = new TenancyRepo(connectionString);
        IRepo<Tenant> tenantRepo = new TenantRepo(connectionString);
        IRepo<Address> AddressRepo = new AddressRepo(connectionString);

        // Services
        AuthLogin authLoginService = new AuthLogin(userRepo);
        TenancyService tenancyService = new TenancyService(tenancyRepo, tenantRepo, AddressRepo);
        SearchService searchService = new SearchService();
        FilterService filterService = new FilterService();
        HistoryService historyService = new HistoryService();
        ExcelImportService excelImportService = new ExcelImportService();
        INavigationService navigationService = new NavigationService();

        //if (authLoginService == null || navigationService == null) { MessageBox.Show("Error: One or more services failed to initialize."); return; }

        // Create ViewModels
        LoginViewModel loginViewModel = new LoginViewModel(authLoginService, navigationService);
        TenancyViewModel tenancyViewModel = new TenancyViewModel(navigationService, tenancyService, filterService, searchService);
        TenancyUploadViewModel tenancyUploadViewModel = new TenancyUploadViewModel(navigationService, tenancyService, filterService, searchService, excelImportService);
        //HistoryViewModel historyViewModel = new HistoryViewModel(navigationService, historyService, filterService, searchService);

        // Set up factory methods for creating views
        navigationService.RegisterFactory( () => new LoginView(loginViewModel));
        navigationService.RegisterFactory( () => new TenancyView(tenancyViewModel));
        navigationService.RegisterFactory(() => new TenancyUploadView(tenancyUploadViewModel));
        //navigationService.RegisterFactory( () => new HistoryView(historyViewModel));

        // Set up the initial window
        //LoginView loginView = new LoginView(loginViewModel);
        //loginView.Show();

        //skip login
        TenancyView tenancyView = new TenancyView(tenancyViewModel);
        tenancyView.Show();






        //// Create repository - here we instantiate a concrete implementation of IRepo<User>
        //IRepo<User> userRepo = new UserRepository();

        //// Create service using repository - instantiate AuthLogin with the userRepository
        //AuthLogin authLoginService = new AuthLogin(userRepository);

        //// Create navigation service - instantiate it (assuming it's a singleton or can be reused)
        //INavigationService navigationService = new NavigationService();

        //// Create ViewModel using the service
        //var loginViewModel = new LoginViewModel(authLoginService, navigationService);

        //// Instantiate and show LoginView with its ViewModel
        //var loginView = new LoginView(loginViewModel);
        //loginView.Show();




    }



}
