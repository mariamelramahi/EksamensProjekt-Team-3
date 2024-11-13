using EksamensProjekt.Models;
using EksamensProjekt.Services;
using EksamensProjekt.Services.Navigation;
using EksamensProjekt.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EksamensProjekt.ViewModels
{
    public class TenancyViewModel : ViewModelBase
    {

        private readonly INavigationService _navigationService;
        private readonly TenancyService _tenancyService;


        // Constructor
        public TenancyViewModel(INavigationService navigationService, TenancyService tenancyService)
        {
            _navigationService = navigationService;
            _tenancyService = tenancyService;

            // Initialize ObservableCollections
            Tenancies = new ObservableCollection<Tenancy>();

            // Load initial data
            LoadTenancies();

            // Initialize commands
            GoToHistoryCommand = new RelayCommand(ExecuteGoToHistory);
            CreateTenancyCommand = new RelayCommand(ExecuteCreateTenancy);
            UpdateTenancyCommand = new RelayCommand(ExecuteUpdateTenancy, CanExecuteModifyTenancy);
            DeleteTenancyCommand = new RelayCommand(ExecuteDeleteTenancy, CanExecuteModifyTenancy);
            //UploadFileCommand = new RelayCommand(ExecuteUploadFile);
        }


        // Observable Collections
        public ObservableCollection<Tenancy> Tenancies { get; set; }


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


        // Commands
        public RelayCommand GoToHistoryCommand { get; }
        public RelayCommand CreateTenancyCommand { get; }
        public RelayCommand UpdateTenancyCommand { get; }
        public RelayCommand DeleteTenancyCommand { get; }
        public RelayCommand UploadFileCommand { get; }


        // Methods
        private void LoadTenancies()
        {
            var tenancies = _tenancyService.GetAllTenancies();
            Tenancies.Clear();
            foreach (var tenancy in tenancies)
            {
                Tenancies.Add(tenancy);
            }
        }

      
        private void ExecuteGoToHistory()
        {
            _navigationService.NavigateTo<HistoryView>();
        }

        
        private void ExecuteCreateTenancy()
        {
            Tenancy newTenancy = _tenancyService.CreateNewTenancy();
            if (newTenancy != null)
            {
                Tenancies.Add(newTenancy);
            }
        }

        
        private void ExecuteUpdateTenancy()
        {
            if (SelectedTenancy != null)
            {
                _tenancyService.UpdateTenancy(SelectedTenancy);
                LoadTenancies(); // Refresh the list to reflect changes
            }
        }

        
        private void ExecuteDeleteTenancy()
        {
            if (SelectedTenancy != null)
            {
                _tenancyService.DeleteTenancy(SelectedTenancy);
                Tenancies.Remove(SelectedTenancy);
            }
        }

        
        private bool CanExecuteModifyTenancy()
        {
            return SelectedTenancy != null;
        }


        // Command to upload a file, using the ExcelImporter or related service (accepting a parameter) --- Bind to CommandParameter in XAML to pass object. (?) 
        //private void ExecuteUploadFile(object parameter)
        //{
        //    if (parameter is string filePath)
        //    {
        //        _tenancyService.UploadFile(filePath);
        //        LoadTenancies(); // Reload the tenancies after import
        //    }
        //}
    }
}
