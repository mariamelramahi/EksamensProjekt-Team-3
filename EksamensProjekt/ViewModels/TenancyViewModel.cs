using EksamensProjekt.Models;
using EksamensProjekt.Services;
using EksamensProjekt.Services.Navigation;
using EksamensProjekt.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
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
            UpdateTenancyCommand = new RelayCommand(ExecuteUpdateTenancy, CanExecuteModifyTenancy);
            SoftDeleteTenancyCommand = new RelayCommand(ExecuteSoftDeleteTenancy, CanExecuteModifyTenancy);          
            DeleteTenancyTenantCommand = new RelayCommand(ExecuteDeleteTenancyTenant, CanExecuteDeleteTenancyTenant);
            TenantMessageboxInfoCommand = new RelayCommand(ExecuteTenantMessageboxInfo, CanExecuteTenantMessageboxInfo);
            CreateNewTenantCommand = new RelayCommand(ExecuteCreateNewTenant, CanExecuteCreateNewTenant);
            AddTenantToTenancyCommand = new RelayCommand(ExecuteAddTenantToTenancy, CanExecuteAddTenantToTenancy);
            UpdateTenantCommand = new RelayCommand(ExecuteUpdateTenant, CanExecuteUpdateTenant);
            DeleteTenantCommand = new RelayCommand(ExecuteDeleteTenant, CanExecuteDeleteTenant);
            GoToTenancyUploadCommand = new RelayCommand(ExecuteGoToTenancyUpload);
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
        public RelayCommand GoToHistoryCommand => new RelayCommand(() =>
        {
            _navigationService.NavigateToWithViewModel<HistoryView, HistoryViewModel>(vm => vm.RefreshHistory());
        });
        
        public RelayCommand GoToLoginCommand => new RelayCommand(() => _navigationService.NavigateTo<LoginView>());
        
        public RelayCommand CreateTenancyCommand { get; }
        public RelayCommand UpdateTenancyCommand { get; }
        public RelayCommand SoftDeleteTenancyCommand { get; }
        public RelayCommand UploadFileCommand { get; }

        public RelayCommand GoToTenancyUploadCommand { get;  }

        public RelayCommand DeleteTenancyTenantCommand { get; }
        public RelayCommand TenantMessageboxInfoCommand { get; }
        public RelayCommand CreateNewTenantCommand { get; }
        public RelayCommand AddTenantToTenancyCommand { get; }
        public RelayCommand UpdateTenantCommand { get; }
        public RelayCommand DeleteTenantCommand { get; }

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

        public void RefreshTenancies() //public method to be used in other viewmodels to refresh the tenancies using LoadTenancies
        {
            LoadTenancies();
        }

        private void LoadTenants()
        {
            AllTenants.Clear();
            var tenants = _tenancyService.GetAllTenants();
            foreach (var tenant in tenants)
            {
                AllTenants.Add(tenant);
            }
        }
        
        private void ExecuteDeleteTenant()
        {
            if (SelectedTenant != null)
            {
                _tenancyService.DeleteTenant(SelectedTenant);
                AllTenants.Remove(SelectedTenant);
                LoadTenants(); // Refresh the list to reflect changes
            }
        }


        private void ExecuteGoToTenancyUpload()
        {
            _navigationService.NavigateTo<TenancyUploadView>();
        }

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

        private bool CanExecuteDeleteTenant()
        {
            return SelectedTenant != null;
        }
        private void ExecuteDeleteTenancyTenant()
        {
            if (SelectedTenancy != null && SelectedTenancyTenant != null)
            {
                _tenancyService.RemoveTenancyTenant(SelectedTenancy, SelectedTenancyTenant);

                
                SelectedTenancy.Tenants.Remove(SelectedTenancyTenant);
                MessageBox.Show($"Lejer: {SelectedTenancyTenant.FirstName} {SelectedTenancyTenant.LastName}\n" +
                                $"Er blevet fjernet fra lejemål.\n" +
                                $"TenancyID: {SelectedTenancy.TenancyID}");
                SelectedTenancyTenant = null;

                
                OnPropertyChanged(nameof(SelectedTenancy));
                LoadTenancies();
            }
        }

        private bool CanExecuteModifyTenancy()
        {
            return SelectedTenancy != null;
        }

        private void ExecuteTenantMessageboxInfo()
        {
            if (SelectedTenancyTenant != null)
            {
                MessageBox.Show($"Tenant ID: {SelectedTenancyTenant.TenantID}\n" +
                                $"Name: {SelectedTenancyTenant.FirstName} {SelectedTenancyTenant.LastName}\n" +
                                $"Phone: {SelectedTenancyTenant.PhoneNum}\n" +
                                $"Email: {SelectedTenancyTenant.Email}\n");
            }
        }
        private bool CanExecuteTenantMessageboxInfo()
        {
            return SelectedTenancyTenant != null;
        }

        private void ExecuteCreateNewTenant()
        {
            Tenant newTenant = _tenancyService.CreateNewTenant();
            if (newTenant != null)
            {
                AllTenants.Add(newTenant);
            }
        }

        private bool CanExecuteCreateNewTenant()
        {
            return true;
        }

        private void ExecuteAddTenantToTenancy()
        {
            if (SelectedTenancy != null && SelectedTenant != null)
            {
                _tenancyService.AddTenantToTenancy(SelectedTenancy, SelectedTenant);
                SelectedTenancy.Tenants.Add(SelectedTenant);
                MessageBox.Show($"Lejer: {SelectedTenant.FirstName} {SelectedTenant.LastName}\n" +
                                $"Er blevet tilføjet til lejemål.\n" +
                                $"TenancyID: {SelectedTenancy.TenancyID}");
            }
            OnPropertyChanged(nameof(SelectedTenancy));
            LoadTenancies();
        }

        private bool CanExecuteAddTenantToTenancy()
        {
            return SelectedTenancy != null && SelectedTenant != null;
        }

        private void ExecuteUpdateTenant()
        {
            if (SelectedTenant != null)
            {
                _tenancyService.UpdateTenant(SelectedTenant);
                LoadTenants(); // Refresh the list to reflect changes
                LoadTenancies();
            }
        }

        private bool CanExecuteUpdateTenant()
        {
            return SelectedTenant != null;
        }

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
    }
}
