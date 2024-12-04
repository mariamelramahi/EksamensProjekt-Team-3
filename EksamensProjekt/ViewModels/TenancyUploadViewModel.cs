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
    public class TenancyUploadViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
 
        private readonly FilterService _filterService;
        private readonly SearchService _searchService;
        private readonly ExcelImportService _excelImportService;
        private readonly MatchService _matchService;
        private ICollectionView _importedAddressesCollectionView;
        private ICollectionView _addressMatchesCollectionView;


        // Constructor
        public TenancyUploadViewModel(INavigationService navigationService, FilterService filterService, SearchService searchService, ExcelImportService excelImportService, DragAndDropService dragAndDropService, MatchService matchService)
        {
            _navigationService = navigationService;
            _filterService = filterService;
            _searchService = searchService;
            _excelImportService = excelImportService;
            _matchService = matchService;

            // Initialize ObservableCollection
            //ImportedAddresses = new ObservableCollection<Address>(_matchService.GetAddressesWithTypos());
            //ImportedAddresses = new ObservableCollection<Address>();
            ImportedAddresses = new ObservableCollection<AddressMatchResult>();
            FilteredMatches = new ObservableCollection<AddressAndMatchScore>();
            ExcelAddresses = new ObservableCollection<Address>();

            // Drag-and-Drop service
            DragAndDropService = dragAndDropService;
            DragAndDropService.FileDropped = OnFileDropped;

            //// Set up CollectionView for displaying items
            //_importedAddressesCollectionView = CollectionViewSource.GetDefaultView(ImportedAddresses);
            //_addressMatchesCollectionView = CollectionViewSource.GetDefaultView(FilteredMatches);


             // Initialize commands
            ApproveAllMatchesCommand = new RelayCommand(ExecuteApproveAllMatches);
            //GoToHistoryCommand = new RelayCommand(ExecuteGoToHistory);
            //CreateTenancyCommand = new RelayCommand(ExecuteCreateTenancy);
            //UploadFileCommand = new RelayCommand(ExecuteUploadFile);
        }

        // Observable Collections
        private ObservableCollection<Address> _excelAddresses;
        private ObservableCollection<AddressMatchResult> _importedAddresses;
        private ObservableCollection<AddressAndMatchScore> _filteredMatches;
        public ObservableCollection<Address> ExcelAddresses
        {
            get => _excelAddresses;
            set
            {
                _excelAddresses = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<AddressAndMatchScore> FilteredMatches
        {
            get => _filteredMatches;
            set
            {
                _filteredMatches = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<AddressMatchResult> ImportedAddresses
        {
            get => _importedAddresses;
            set
            {
                if (_importedAddresses != value)
                {
                    _importedAddresses = value;
                    OnPropertyChanged();
                }
            }
        }
        //// Filtered view of Addresses
        //public ICollectionView FilteredImportedAddresses => _importedAddressesCollectionView;
        //public ICollectionView FilteredImportedMatches => _addressMatchesCollectionView;


        // Properties
        private AddressMatchResult _selectedAddress;
        private string _searchInput;
        private string _filepath;
        public DragAndDropService DragAndDropService { get; }
        private AddressAndMatchScore _userSelectedMatch;
        private bool _isUserSelectionRequired = true;

        public bool IsUserSelectionRequired
        {
            get => _isUserSelectionRequired;
            set
            {
                _isUserSelectionRequired = value;
                OnPropertyChanged();
            }
        }
        public AddressAndMatchScore UserSelectedMatch

        {
            get => _userSelectedMatch;
            set
            {
                if (_userSelectedMatch != value)
                {
                    _userSelectedMatch = value;
                    SetUserSelectedMatch();
                    OnPropertyChanged();
                }
            }
        }
        public AddressMatchResult SelectedAddress
        {
            get => _selectedAddress;
            set
            {
                _selectedAddress = value;
                FilterMatchesBySelectedAddress(); // Re-filter the matches based on the new selected address
                OnPropertyChanged();

                // Raise CanExecuteChanged on commands depending on SelectedAddress
                SoftDeleteTenancyCommand?.RaiseCanExecuteChanged();
            }
        }
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

        public string Filepath
        {
            get => _filepath;
            set
            {
                _filepath = value;
                OnPropertyChanged();
                LoadAndMatchImportedAddresses();
            }
        }

        private void OnFileDropped(string filePath)
        {
            Filepath = filePath; // Opdate property in ViewModel
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
        public RelayCommand CreateTenancyCommand { get; }
        public RelayCommand SoftDeleteTenancyCommand { get; }
        public RelayCommand UploadFileCommand { get; }
        public RelayCommand GoToTenancyCommand => new RelayCommand(() => _navigationService.NavigateTo<TenancyView>());
        public RelayCommand ApproveAllMatchesCommand { get;  }


        //Methods
        private void LoadAndMatchImportedAddresses()
        {
            ImportedAddresses.Clear();
            var importedAddresses = _excelImportService.ImportAddresses(Filepath);
            var addressMatches = _matchService.CompareImportedAddressesWithDatabase(importedAddresses);
            foreach (var addressMatch in addressMatches)
            {
                ImportedAddresses.Add(addressMatch);
            }
            // After importing and matching, check if user selection is required for any match
            CheckIfUserSelectionRequired();
        }

        private bool CanExecuteModifyTenancy()
        {
            return SelectedAddress != null;
        }


        private bool ApplyCombinedFilter(AddressMatchResult addressMatchResult, AddressAndMatchScore addressAndMatchScore)
        {
            return _filterService.ApplyFilter(addressAndMatchScore) &&
                   _searchService.ApplySearchFilter(addressMatchResult, SearchInput);
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

        // Method to trigger the approval of all matches
        public void ExecuteApproveAllMatches()
        {
            // Call the service layer to approve the matches
            _matchService.ApproveMatches(ImportedAddresses.ToList());

            // Check if any matches require user selection
            foreach (var match in ImportedAddresses)
            {
                if (match.IsUserSelectionRequired == true)
                {
                    // Trigger UI logic to prompt user for selection
                    PromptUserForSelection(match);
                }
            }
            _navigationService.NavigateTo<TenancyView>();
        }   
        private void SetUserSelectedMatch()
        {
            // Ensure that SelectedAddress is not null and has potential matches
            if (SelectedAddress != null && SelectedAddress.PotentialMatches.Any())
            {
                // If the user has selected a match, use that
                if (UserSelectedMatch != null)
                {
                    // User has selected a match, so use the selected one
                    SelectedAddress.SelectedMatch = UserSelectedMatch;
                    SelectedAddress.IsUserSelectionRequired = false;
                    CheckIfUserSelectionRequired();
                }
            }
        }
        // Method to prompt the user for selecting a match (could trigger a UI interaction)
        private void PromptUserForSelection(AddressMatchResult match)
        {
            // Show the message box to prompt the user
            MessageBox.Show($"Venligst vælg en match for addresse: {match.ImportedAddress}", "Bruger skal vælge en match");
        }
        // This method checks if any address has an unselected match and requires user action
        private void CheckIfUserSelectionRequired()
        {
            // Set flag if any address has IsUserSelectionRequired = true
            IsUserSelectionRequired = ImportedAddresses.Any(address => address.IsUserSelectionRequired);
        }
        private void FilterMatchesBySelectedAddress()
        {

            if (SelectedAddress != null)
            {
                // Clear the existing matches
                FilteredMatches.Clear();

                foreach (var match in SelectedAddress.PotentialMatches)
                {
                    FilteredMatches.Add(match); // Add potential matches
                }

            }
        }

    }
}
