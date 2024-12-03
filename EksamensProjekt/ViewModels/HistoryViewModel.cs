using EksamensProjekt.Models;
using EksamensProjekt.Services;
using EksamensProjekt.Services.Navigation;
using EksamensProjekt.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace EksamensProjekt.ViewModels;

public class HistoryViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly HistoryService _historyService;
    private readonly SearchService _searchService;
    private ICollectionView _historyCollectionView;


    // Constructor
    public HistoryViewModel(INavigationService navigationService, HistoryService historyService, SearchService searchService)
    {
        _navigationService = navigationService;
        _historyService = historyService;
        _searchService = searchService;

        // Initialize ObservableCollection
        HistoryItems = new ObservableCollection<History>();

        // Load initial data
        LoadHistory();

        // Set up CollectionView for displaying items
        _historyCollectionView = CollectionViewSource.GetDefaultView(HistoryItems);
       

        // Initialize commands
        //GoToTenancyCommand = new RelayCommand(ExecuteGoToTenancy);
        
    }


    // Observable Collection
    public ObservableCollection<History> HistoryItems { get; set; }


    // Filtered view of history items
    public ICollectionView FilteredHistoryItems => _historyCollectionView;
    



    // Commands
    public RelayCommand GoToTenancyCommand => new RelayCommand(() => _navigationService.NavigateTo<TenancyView>());
    public RelayCommand GoToLoginCommand => new RelayCommand(() => _navigationService.NavigateTo<LoginView>());
    public RelayCommand ApplyFiltersCommand { get; }


    // Methods
    private void LoadHistory()
    {
        // Fetch all history items from the service
        var historyItems = _historyService.GetAllHistories();

        // Clear the existing collection
        HistoryItems.Clear();

        // Add the fetched items to the collection
        foreach (var item in historyItems)
        {
            HistoryItems.Add(item);
        }
    }

    public void RefreshHistory()
    {
        LoadHistory(); //public method to refresh the history when navigating from tnancyview
    }

    //private void ExecuteGoToTenancy()
    //{
    //    _navigationService.NavigateTo<TenancyView>();
    //}





}
