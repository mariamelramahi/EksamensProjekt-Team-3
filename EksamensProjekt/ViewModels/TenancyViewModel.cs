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
            AllTenants = new ObservableCollection<Tenant>();

            // Load initial data
            LoadTenancies();
            LoadTenants();

            // Set up CollectionView for displaying items
            _tenancyCollectionView = CollectionViewSource.GetDefaultView(Tenancies);

            _tenancyCollectionView.Filter = item => ApplyCombinedFilter(item as Tenancy);


            // Initialize commands
            //GoToHistoryCommand = new RelayCommand(ExecuteGoToHistory);
            //CreateTenancyCommand = new RelayCommand(ExecuteCreateTenancy);
            UpdateTenancyCommand = new RelayCommand(ExecuteUpdateTenancy, CanExecuteModifyTenancy);
            SoftDeleteTenancyCommand = new RelayCommand(ExecuteSoftDeleteTenancy, CanExecuteModifyTenancy);
            //UploadFileCommand = new RelayCommand(ExecuteUploadFile);
            DeleteTenancyTenantCommand = new RelayCommand(ExecuteDeleteTenancyTenant, CanExecuteDeleteTenancyTenant);
        }


        // Observable Collections
        public ObservableCollection<Tenancy> Tenancies { get; set; }
        public ObservableCollection<Tenant> AllTenants { get; set; }


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
                // Raise CanExecuteChanged on commands depending on SelectedAddress
                UpdateTenancyCommand?.RaiseCanExecuteChanged();
                SoftDeleteTenancyCommand?.RaiseCanExecuteChanged();
            }
        }

        private Tenant _selectedTenancyTenant;
        public Tenant SelectedTenancyTenant
        {
            get => _selectedTenancyTenant;
            set
            {
                _selectedTenancyTenant = value;
                OnPropertyChanged();
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
                OnFilterChanged(); // Automatically apply filters when search input changes
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
                OnFilterChanged(); // Apply filters whenever value changes
            }
        }

        public bool IsFilterBEnabled
        {
            get => _filterService.IsFilterBEnabled;
            set
            {
                _filterService.IsFilterBEnabled = value;
                OnPropertyChanged();
                OnFilterChanged();
            }
        }

        public bool IsFilterCEnabled
        {
            get => _filterService.IsFilterCEnabled;
            set
            {
                _filterService.IsFilterCEnabled = value;
                OnPropertyChanged();
                OnFilterChanged();
            }
        }

        //IsFilterDEnabled always true. ICollectionView returns it false when deciding if it should show in the View
        public bool IsFilterDEnabled
        {
            get => _filterService.IsFilterDEnabled;
        }


        // Commands
        public RelayCommand GoToHistoryCommand { get; }
        public RelayCommand CreateTenancyCommand { get; }
        public RelayCommand UpdateTenancyCommand { get; }
        public RelayCommand SoftDeleteTenancyCommand { get; }
        public RelayCommand UploadFileCommand { get; }
        public RelayCommand GoToTenancyUploadCommand => new RelayCommand(() => _navigationService.NavigateTo<TenancyUploadView>());
        public RelayCommand DeleteTenancyTenantCommand { get; }

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

        private void LoadTenants()
        {
            //Tenants.Clear();
            var tenants = _tenancyService.GetAllTenants();
            foreach (var tenant in tenants)
            {
                AllTenants.Add(tenant);
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
        //        ImportedAddresses.Add(newTenancy);
        //    }
        //}

        private void ExecuteUpdateTenancy()
        {
            if (SelectedTenancy != null)
            {
                _tenancyService.UpdateTenancy(SelectedTenancy);
                LoadTenancies(); // Refresh the list to reflect changes
            }
        }

        private void ExecuteSoftDeleteTenancy()
        {
            if (SelectedTenancy != null)
            {
                _tenancyService.SoftDeleteTenancy(SelectedTenancy);
                Tenancies.Remove(SelectedTenancy);
            }
        }

        private bool CanExecuteDeleteTenancyTenant()
        {
            return SelectedTenancy != null && SelectedTenancyTenant != null;
        }

        private void ExecuteDeleteTenancyTenant()
        {
            if (SelectedTenancy != null && SelectedTenancyTenant != null)
            {
                _tenancyService.DeleteTenancyTenant(SelectedTenancy.TenancyID, SelectedTenancyTenant.TenantID);

                
                SelectedTenancy.Tenants.Remove(SelectedTenancyTenant);
                SelectedTenancyTenant = null;

                
                OnPropertyChanged(nameof(SelectedTenancy));
                LoadTenancies();
            }
        }

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

        private bool ApplyCombinedFilter(Tenancy tenancy)
        {
            return _filterService.ApplyFilter(tenancy) &&
                   _searchService.ApplySearchFilter(tenancy, SearchInput);
        }
        
        
        // Refresh new way: Threads (quicker / snappy UI)
        private async void OnFilterChanged()
        {
            await Task.Run(() =>
            {
                // Perform filtering on a background thread
                App.Current.Dispatcher.Invoke(() =>
                {
                    // Refresh the ICollectionView on the UI thread
                    _tenancyCollectionView.Refresh();
                });
            });
        }

        // Refresh old way
        //private void RefreshFilteredView()
        //{
        //    _importedAddressesCollectionView.Refresh(); // Refresh the view to apply updated filters
        //}


    }
}
