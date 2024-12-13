using EksamensProjekt.Models.Models.DTO;
using EksamensProjekt.Services;
using EksamensProjekt.Services.Navigation;
using EksamensProjekt.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace EksamensProjekt.ViewModels
{
    public class TenancyUploadViewModel : ViewModelBase
    {

        // Dependencies
        private readonly INavigationService _navigationService;
        private readonly FilterService _filterService;
        private readonly SearchService _searchService;
        private readonly ExcelImportService _excelImportService;
        private readonly MatchService _matchService;

        // Collections and Views 
        private ICollectionView _importedAddressesCollectionView;

        // Observable Collections
        public ObservableCollection<AddressMatchResult> ImportedAddresses { get; set; }
        public ObservableCollection<AddressAndMatchScore> AddressMatches { get; set; }



        // Constructor
        public TenancyUploadViewModel(INavigationService navigationService, FilterService filterService, SearchService searchService, ExcelImportService excelImportService, DragAndDropService dragAndDropService, MatchService matchService)
        {
            _navigationService = navigationService;
            _filterService = filterService;
            _searchService = searchService;
            _excelImportService = excelImportService;
            _matchService = matchService;

            // Initialize ObservableCollection
            ImportedAddresses = new ObservableCollection<AddressMatchResult>();
            AddressMatches = new ObservableCollection<AddressAndMatchScore>();

            // Drag-and-Drop service
            DragAndDropService = dragAndDropService;
            DragAndDropService.FileDropped = OnFileDropped;

            // Set up CollectionView for displaying items when using filters
            _importedAddressesCollectionView = CollectionViewSource.GetDefaultView(ImportedAddresses);
            _importedAddressesCollectionView.Filter = item => ApplyCombinedFilter(item as AddressMatchResult);

            // Initialize commands
            ApproveAllMatchesCommand = new RelayCommand(ExecuteApproveAllMatches, CanExecuteApproveAllAddresses);
            GoToTenancyCommand = new RelayCommand(ExecuteGoToTenancyCommand);

            DeleteTenancyCommand = new RelayCommand(DeleteAddressCommand, CanExecuteDeleteAddress);

    }



        // Properties
        public DragAndDropService DragAndDropService { get; }

        // Filtered view of Addresses
        public ICollectionView FilteredImportedAddresses => _importedAddressesCollectionView;


        private string _searchInput;
        public string SearchInput
        {
            get => _searchInput;
            set
            {
                _searchInput = value;
                OnPropertyChanged();
                OnFilterChanged();
            }
        }

        private string _filepath;
        public string Filepath
        {
            get => _filepath;
            set
            {
                // Set the new value
                _filepath = value;

                // Only call LoadAndMatchImportedAddresses if the Filepath is not null or empty
                if (!string.IsNullOrEmpty(_filepath))
                {
                    OnPropertyChanged();
                    LoadAndMatchImportedAddresses();
                }
                else
                {
                    // Optionally, you can handle the null case here if needed
                    // For example, clear any imported addresses
                    ImportedAddresses.Clear();
                }
            }
        }

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

        private AddressMatchResult _selectedAddress;
        public AddressMatchResult SelectedAddress
        {
            get => _selectedAddress;
            set
            {
                _selectedAddress = value;
                FilterMatchesBySelectedAddress();
                OnPropertyChanged();
                DeleteTenancyCommand?.RaiseCanExecuteChanged();
            }
        }

        private AddressAndMatchScore _userSelectedMatch;
        public AddressAndMatchScore UserSelectedMatch
        {
            get => _userSelectedMatch;
            set
            {
                _userSelectedMatch = value;
                SetUserSelectedMatch();
                ApproveAllMatchesCommand?.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }


        // Delegated Filter Properties (From FilterService)
        public bool IsFilterAEnabled
        {
            get => _filterService.IsFilterAEnabled;
            set
            {
                _filterService.IsFilterAEnabled = value;
                OnPropertyChanged();
                OnFilterChanged();
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

        // IsFilterDEnabled
        // Not implemented yet
        public bool IsFilterDEnabled => _filterService.IsFilterDEnabled;



        // Commands
        public RelayCommand GoToLoginCommand => new RelayCommand(() => _navigationService.NavigateTo<LoginView>());
        public RelayCommand GoToTenancyCommand { get; }
        public RelayCommand ApproveAllMatchesCommand { get; }
        public RelayCommand DeleteTenancyCommand { get; }




        // Methods
        private void OnFileDropped(string filePath) => Filepath = filePath;


        private void ExecuteGoToTenancyCommand()
        {
            _navigationService.NavigateTo<TenancyView>();
        }


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


        private bool CanExecuteApproveAllAddresses()
        {
            return IsUserSelectionRequired != true && ImportedAddresses != null;
        }


        private bool CanExecuteDeleteAddress()
        {
            return SelectedAddress != null;
        }


        private bool ApplyCombinedFilter(AddressMatchResult addressMatchResult)
        {
            if (addressMatchResult == null)
                return false;

            return _filterService.ApplyFilter(addressMatchResult) &&
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


        public void DeleteAddressCommand()
        {
            if (SelectedAddress != null)
            {
                // Get the AddressMatchResult from the selected AddressMatchResult
                AddressMatchResult selectedAddress = SelectedAddress;

                // Remove the AddressMatchResult from the ImportedAddresses collection
                if (selectedAddress != null)
                {
                    ImportedAddresses.Remove(selectedAddress);
                    SelectedAddress = null;
                    CheckIfUserSelectionRequired();
                }
            }
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
            Filepath = null;
            _navigationService.NavigateToWithViewModel<TenancyView, TenancyViewModel>(vm => vm.RefreshTenancies());
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
                    _importedAddressesCollectionView.Refresh();
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
                AddressMatches.Clear();

                foreach (var match in SelectedAddress.PotentialMatches)
                {
                    AddressMatches.Add(match); // Add potential matches
                }

            }
            else if (SelectedAddress == null)
            {
                AddressMatches.Clear();
            }
        }
    }
}
