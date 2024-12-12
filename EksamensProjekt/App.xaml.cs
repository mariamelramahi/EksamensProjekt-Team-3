using EksamensProjekt.Models;
using EksamensProjekt.Repos;
using EksamensProjekt.Repositories;
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
        IUserRepo<User> userRepo = new UserRepo(connectionString);
        IRepo<Tenancy> tenancyRepo = new TenancyRepo(connectionString);
        ITenancyTenant tenancyTenantRepo = (TenancyRepo)tenancyRepo;
        IRepo<Tenant> tenantRepo = new TenantRepo(connectionString);
        IRepo<Address> addressRepo = new AddressRepo(connectionString);
        IHistoryRepo historyRepo = new HistoryRepo(connectionString);

        // Services
        AuthLogin authLoginService = new AuthLogin(userRepo);
        TenancyService tenancyService = new TenancyService(tenancyRepo, tenantRepo, addressRepo, tenancyTenantRepo);
        SearchService searchService = new SearchService();
        FilterService filterService = new FilterService();
        HistoryService historyService = new HistoryService(historyRepo);
        ExcelImportService excelImportService = new ExcelImportService();
        MatchService matchService = new MatchService(tenancyRepo, addressRepo); 
        INavigationService navigationService = new NavigationService();
        DragAndDropService dragAndDropService = new DragAndDropService();       

        // Create ViewModels
        LoginViewModel loginViewModel = new LoginViewModel(authLoginService, navigationService);
        TenancyViewModel tenancyViewModel = new TenancyViewModel(navigationService, tenancyService, filterService, searchService);
        HistoryViewModel historyViewModel = new HistoryViewModel(navigationService, historyService, searchService);
        TenancyUploadViewModel tenancyUploadViewModel = new TenancyUploadViewModel(navigationService, filterService, searchService, excelImportService, dragAndDropService, matchService);
        
        // Set up factory methods for creating views
        navigationService.RegisterFactory( () => new LoginView(loginViewModel));
        navigationService.RegisterFactory( () => new TenancyView(tenancyViewModel));
        navigationService.RegisterFactory(() => new TenancyUploadView(tenancyUploadViewModel));
        navigationService.RegisterFactory(() => new HistoryView(historyViewModel));

        // Set up the initial window
        LoginView loginView = new LoginView(loginViewModel);
        loginView.Show();

        // Allow drag and drop
        EventManager.RegisterClassHandler(typeof(UIElement),
        UIElement.PreviewDragOverEvent,
        new DragEventHandler((sender, args) => args.Handled = true));


    }

}
