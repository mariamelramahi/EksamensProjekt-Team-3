using EksamensProjekt.Models;
using EksamensProjekt.Services;
using EksamensProjekt.Services.Navigation;
using EksamensProjekt.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace EksamensProjekt.ViewModels
{
    public class TenancyViewModel : ViewModelBase
    {

        private readonly INavigationService _navigationService;
        private readonly TenancyService _tenancyService;
        private readonly FilterService _filterService;
        private readonly SearchService _searchService;
        private ICollectionView _tenancyCollectionView;


        // Constructor
        public TenancyViewModel(INavigationService navigationService, TenancyService tenancyService, FilterService filterService, SearchService searchService)
        {
            _navigationService = navigationService;
            _tenancyService = tenancyService;
            _filterService = filterService;
            _searchService = searchService;

            // Initialize ObservableCollection
            Tenancies = new ObservableCollection<Tenancy>();

            // Load initial data
            LoadTenancies();

            // Set up CollectionView for displaying items
            _tenancyCollectionView = CollectionViewSource.GetDefaultView(Tenancies);

            _tenancyCollectionView.Filter = item => ApplyCombinedFilter(item as Tenancy);


            // Initialize commands
            //GoToHistoryCommand = new RelayCommand(ExecuteGoToHistory);
            //CreateTenancyCommand = new RelayCommand(ExecuteCreateTenancy);
            //UpdateTenancyCommand = new RelayCommand(ExecuteUpdateTenancy, CanExecuteModifyTenancy);
            //DeleteTenancyCommand = new RelayCommand(ExecuteDeleteTenancy, CanExecuteModifyTenancy);
            //UploadFileCommand = new RelayCommand(ExecuteUploadFile);
        }


        // Observable Collections
        public ObservableCollection<Tenancy> Tenancies { get; set; }


        // Filtered view of tenancies
        public ICollectionView FilteredTenancies => _tenancyCollectionView;


        // Properties
        private Tenancy _selectedTenancy;
        public Tenancy SelectedTenancy
        {
            get => _selectedTenancy;
            set
            {
                _selectedTenancy = value;
                OnPropertyChanged();
                // Raise CanExecuteChanged on commands depending on SelectedTenancy
                UpdateTenancyCommand.RaiseCanExecuteChanged();
                DeleteTenancyCommand.RaiseCanExecuteChanged();
            }
        }

        private Tenant _selectedTenant;
        public Tenant SelectedTenant
        {
            get => _selectedTenant;
            set
            {
                _selectedTenant = value;
                OnPropertyChanged();
            }
        }

        private string _searchInput;
        public string SearchInput
        {
            get => _searchInput;
            set
            {
                _searchInput = value;
                OnPropertyChanged();
                RefreshFilteredView(); // Automatically apply filters when search input changes
            }
        }


        // Delegated Filter Properties (delegates to FilterService) exposer
        public bool IsFilterAEnabled
        {
            get => _filterService.IsFilterAEnabled;
            set
            {
                _filterService.IsFilterAEnabled = value;
                OnPropertyChanged();
                RefreshFilteredView(); // Apply filters whenever value changes
            }
        }

        public bool IsFilterBEnabled
        {
            get => _filterService.IsFilterBEnabled;
            set
            {
                _filterService.IsFilterBEnabled = value;
                OnPropertyChanged();
                RefreshFilteredView();
            }
        }

        public bool IsFilterCEnabled
        {
            get => _filterService.IsFilterCEnabled;
            set
            {
                _filterService.IsFilterCEnabled = value;
                OnPropertyChanged();
                RefreshFilteredView();
            }
        }


        // Commands
        public RelayCommand GoToHistoryCommand { get; }
        public RelayCommand CreateTenancyCommand { get; }
        public RelayCommand UpdateTenancyCommand { get; }
        public RelayCommand DeleteTenancyCommand { get; }
        public RelayCommand UploadFileCommand { get; }


        // Methods
        private void LoadTenancies()
        {
            Tenancies.Clear();
            var tenancies = _tenancyService.GetAllTenancies();
            foreach (var tenancy in tenancies)
            {
                Tenancies.Add(tenancy);
            }
        }

        //private void ExecuteGoToHistory()
        //{
        //    _navigationService.NavigateTo<HistoryView>();
        //}

        //private void ExecuteCreateTenancy()
        //{
        //    Tenancy newTenancy = _tenancyService.CreateNewTenancy();
        //    if (newTenancy != null)
        //    {
        //        Tenancies.Add(newTenancy);
        //    }
        //}

        //private void ExecuteUpdateTenancy()
        //{
        //    if (SelectedTenancy != null)
        //    {
        //        _tenancyService.UpdateTenancy(SelectedTenancy);
        //        LoadTenancies(); // Refresh the list to reflect changes
        //    }
        //}

        //private void ExecuteDeleteTenancy()
        //{
        //    if (SelectedTenancy != null)
        //    {
        //        _tenancyService.DeleteTenancy(SelectedTenancy);
        //        Tenancies.Remove(SelectedTenancy);
        //    }
        //}

        private bool CanExecuteModifyTenancy()
        {
            return SelectedTenancy != null;
        }

        //private void ExecuteUploadFile(object parameter)
        //{
        //    if (parameter is string filePath)
        //    {
        //        _tenancyService.UploadFile(filePath);
        //        LoadTenancies(); // Reload the tenancies after import
        //    }
        //}

        private void RefreshFilteredView()
        {
            _tenancyCollectionView.Refresh(); // Refresh the view to apply updated filters
        }


        private bool ApplyCombinedFilter(Tenancy tenancy)
        {
            return _filterService.ApplyCheckboxFilter(tenancy) &&
                   _searchService.ApplySearchFilter(tenancy, SearchInput);
        }

    }
}
