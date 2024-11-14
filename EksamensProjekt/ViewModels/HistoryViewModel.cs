//using EksamensProjekt.Services;
//using EksamensProjekt.Services.Navigation;
//using EksamensProjekt.Utilities;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Windows.Data;

//namespace EksamensProjekt.ViewModels;

//public class HistoryViewModel : ViewModelBase
//{
//    private readonly INavigationService _navigationService;
//    private readonly HistoryService _historyService;
//    private readonly FilterService _filterService;
//    private readonly SearchService _searchService;
//    private ICollectionView _historyCollectionView;


//    // Constructor
//    public HistoryViewModel(INavigationService navigationService, HistoryService historyService, FilterService filterService, SearchService searchService)
//    {
//        _navigationService = navigationService;
//        _historyService = historyService;
//        _filterService = filterService;
//        _searchService = searchService;

//        // Initialize ObservableCollection
//        HistoryItems = new ObservableCollection<History>();

//        // Load initial data
//        LoadHistory();

//        // Set up CollectionView for displaying items
//        _historyCollectionView = CollectionViewSource.GetDefaultView(HistoryItems);
//        _historyCollectionView.Filter = item => _filterService.ApplyHistoryFilters(item as History);

//        // Initialize commands
//        GoToTenancyCommand = new RelayCommand(ExecuteGoToTenancy);
//    }


//    // Observable Collection
//    public ObservableCollection<History> HistoryItems { get; set; }


//    // Filtered view of history items
//    public ICollectionView FilteredHistoryItems => _historyCollectionView;


//    // Properties
//    private string _searchInput;
//    public string SearchInput
//    {
//        get => _searchInput;
//        set
//        {
//            _searchInput = value;
//            OnPropertyChanged();
//            ExecuteApplyFilters(); // Automatically apply filters when search input changes
//        }
//    }



//    // Delegated Filter Properties (delegates to FilterService)
//    public bool IsFilterAEnabled
//    {
//        get => _filterService.IsFilterAEnabled;
//        set
//        {
//            _filterService.IsFilterAEnabled = value;
//            OnPropertyChanged();
//            ExecuteApplyFilters(); // Apply filters whenever value changes
//        }
//    }

//    public bool IsFilterBEnabled
//    {
//        get => _filterService.IsFilterBEnabled;
//        set
//        {
//            _filterService.IsFilterBEnabled = value;
//            OnPropertyChanged();
//            ExecuteApplyFilters();
//        }
//    }

//    public bool IsFilterCEnabled
//    {
//        get => _filterService.IsFilterCEnabled;
//        set
//        {
//            _filterService.IsFilterCEnabled = value;
//            OnPropertyChanged();
//            ExecuteApplyFilters();
//        }
//    }


//    // Commands
//    public RelayCommand GoToTenancyCommand { get; }
//    public RelayCommand ApplyFiltersCommand { get; }


//    // Methods
//    private void LoadHistory()
//    {
//        HistoryItems.Clear();
//        var historyItems = _historyService.GetAllHistoryItems();
//        foreach (var history in historyItems)
//        {
//            HistoryItems.Add(history);
//        }
//    }

//    private void ExecuteGoToTenancy()
//    {
//        _navigationService.NavigateTo<TenancyView>();
//    }


//    private void ExecuteApplyFilters()
//    {
//        _historyCollectionView.Refresh(); // Refresh the view to apply updated filters
//    }

//}
