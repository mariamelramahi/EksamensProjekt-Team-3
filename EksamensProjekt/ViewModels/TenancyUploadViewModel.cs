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
    public class TenancyUploadViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly TenancyService _tenancyService;
        private readonly FilterService _filterService;
        private readonly SearchService _searchService;
        private readonly ExcelImportService _excelImportService;
        private ICollectionView _importedAddressesCollectionView;

        // Constructor
        public TenancyUploadViewModel(INavigationService navigationService, TenancyService tenancyService, FilterService filterService, SearchService searchService, ExcelImportService excelImportService, DragAndDropService dragAndDropService)
        {
            _navigationService = navigationService;
            _tenancyService = tenancyService;
            _filterService = filterService;
            _searchService = searchService;
            _excelImportService = excelImportService;
            // Initialize ObservableCollection
            ImportedAddresses = new ObservableCollection<Address>();

            //LoadTestData
            //LoadImportedAddresses();

            // Drag-and-Drop service
            DragAndDropService = dragAndDropService;
            DragAndDropService.FileDropped = OnFileDropped;

            // Set up CollectionView for displaying items
            _importedAddressesCollectionView = CollectionViewSource.GetDefaultView(ImportedAddresses);

            //_importedAddressesCollectionView.Filter = item => ApplyCombinedFilter(item as Tenancy);
            //Address is converted to a mock Tenancy object for filtering
            _importedAddressesCollectionView.Filter = item =>
            {
                if (item is Address address)
                {
                    var mockTenancy = new Tenancy
                    {
                        Address = new Address
                        {
                            Street = address.Street,
                            Number = address.Number,
                            FloorNumber = address.FloorNumber,
                            Zipcode = address.Zipcode,
                            Country = address.Country
                        }
                    };

                    return ApplyCombinedFilter(mockTenancy);
                }
                return false;
            };


            // Initialize commands
            //GoToHistoryCommand = new RelayCommand(ExecuteGoToHistory);
            //CreateTenancyCommand = new RelayCommand(ExecuteCreateTenancy);
            //UploadFileCommand = new RelayCommand(ExecuteUploadFile);
        }

        // Observable Collections
        public ObservableCollection<Address> ImportedAddresses { get; set; }


        // Filtered view of tenancies
        public ICollectionView FilteredImportedAddresses => _importedAddressesCollectionView;

        // Properties
        private Address _selectedAddress;
        public Address SelectedAddress
        {
            get => _selectedAddress;
            set
            {
                _selectedAddress = value;
                OnPropertyChanged();

                // Raise CanExecuteChanged on commands depending on SelectedAddress
                SoftDeleteTenancyCommand?.RaiseCanExecuteChanged();
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


        private string _filepath;
        public string Filepath
        {
            get => _filepath;
            set
            {
                _filepath = value;
                OnPropertyChanged();
                LoadImportedAddresses();
            }
        }

        //Service Properties
        public DragAndDropService DragAndDropService { get; }


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
        public RelayCommand SoftDeleteTenancyCommand { get; }
        public RelayCommand UploadFileCommand { get; }
        public RelayCommand GoToTenancyCommand => new RelayCommand(() => _navigationService.NavigateTo<TenancyView>());


        // Methods
        private void LoadImportedAddresses()
        {
            ImportedAddresses.Clear();

            var importedAddresses = _excelImportService.ImportAddresses(Filepath);
            foreach (var address in importedAddresses)
            {
                ImportedAddresses.Add(address);
            }
        }

        //private void ExecuteGoToHistory()
        //{
        //    _navigationService.NavigateTo<HistoryView>();
        //}


        private bool CanExecuteModifyTenancy()
        {
            return SelectedAddress != null;
        }

        //private void ExecuteUploadFile(object parameter)
        //{
        //    if (parameter is string filePath)
        //    {
        //        _tenancyService.UploadFile(filePath);
        //        LoadTenancies(); // Reload the tenancies after import
        //    }
        //}

        private bool ApplyCombinedFilter(Tenancy importedAddress)
        {
            return _filterService.ApplyFilter(importedAddress) &&
                   _searchService.ApplySearchFilter(importedAddress, SearchInput);
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
                    _importedAddressesCollectionView.Refresh();
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
