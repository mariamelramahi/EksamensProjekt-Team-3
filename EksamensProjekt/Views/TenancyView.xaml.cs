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
//AuthLogin authLoginService = new AuthLogin(userRepo);
//TenancyService tenancyService = new TenancyService(tenancyRepo, tenantRepo, standardAddressRepo);
//SearchService searchService = new SearchService();
//FilterService filterService = new FilterService();
//HistoryService historyService = new HistoryService();
//INavigationService navigationService = new NavigationService();
