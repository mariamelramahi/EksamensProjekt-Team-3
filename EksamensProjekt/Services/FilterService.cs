using EksamensProjekt.Model;

using System.Collections.ObjectModel; // Provides ObservableCollection for data binding.
using System.ComponentModel; // Provides ICollectionView and filtering capabilities.
using System.Windows.Data; // Provides CollectionViewSource to create ICollectionView.

namespace EksamensProjekt.Services
{
    public class FilterService
    {
        private readonly ICollectionView _tenancyCollectionView; // Read-only view used for filtering and sorting tenancies.
        private readonly ObservableCollection<Tenancy> _tenancies; // Collection of tenancies managed by this service.


        public FilterService(ObservableCollection<Tenancy> tenancies)
        {
            _tenancies = tenancies; 

            //GetDefaultView provides features that allows you to filter, sort etc. on collections without modifying original data
            _tenancyCollectionView = CollectionViewSource.GetDefaultView(_tenancies);
        }

        //this is for binding to viewModelLayer    
        public ICollectionView TenancyCollectionView => _tenancyCollectionView;

        public void FilterTenancyMatchType(string matchType, List<MatchResult> matchResults)
        {
            if (_tenancyCollectionView == null || matchResults == null) return;

            // Set the filter predicate based on the selected match type
            _tenancyCollectionView.Filter = tenancy =>
            {
                if (tenancy is Tenancy t)
                {
                    // Find the corresponding match result for this tenancy
                    var matchResult = matchResults.FirstOrDefault(r =>
                        r.ImportedAddress.Equals(t.ImportedAddress, StringComparison.OrdinalIgnoreCase) &&
                        r.DatabaseAddress.Equals(t.DatabaseAddress, StringComparison.OrdinalIgnoreCase));

                    if (matchResult != null)
                    {
                        // If no specific match type is selected, show all tenancies
                        if (string.IsNullOrEmpty(matchType))
                            return true;

                        // Check if the match type matches the filter criteria
                        return matchResult.MatchType.Equals(matchType, StringComparison.OrdinalIgnoreCase);
                    }
                }

                return false;
            };

            // Refresh the view to apply the filter
            _tenancyCollectionView.Refresh();
        }
    }

    public void FilterTenancyZipCode(string zipCode)
        {
            if (_tenancyCollectionView == null) return;

            // Sets a filter predicate on the collection view for zip code.
            _tenancyCollectionView.Filter = tenancy =>
            {
                if (tenancy is Tenancy t)
                {
                    // Check if the zip code is provided and matches the tenancy's zip code.
                    return string.IsNullOrEmpty(zipCode) || t.StandardAddress.ZipCode == zipCode;
                }
                return false;
            };

            // Refresh the view to apply the newly set filter.
            _tenancyCollectionView.Refresh();
        }
        public void FilterTenancyStreet(string street)
        {
            if (_tenancyCollectionView == null) return;

            // Set a filter predicate on the collection view for zip code.
            _tenancyCollectionView.Filter = tenancy =>
            {
                if (tenancy is Tenancy t)
                {
                    // Check if the zip code is provided and matches the tenancy's zip code.
                    return string.IsNullOrEmpty(street) || t.StandardAddress.Street == street;
                }
                return false;
            };

            // Refresh the view to apply the newly set filter.
            _tenancyCollectionView.Refresh();
        }
        public void FilterTenancyStatus(string status)
        {
            if (_tenancyCollectionView == null) return;

            // Set a filter predicate on the collection view for TenancyStatus.
            _tenancyCollectionView.Filter = tenancy =>
            {
                if (tenancy is Tenancy t)
                {
                    // Check if the status is provided and matches the tenancy's TenancyStatus.
                    // If status is provided, check if it matches, otherwise show all items.
                    if (string.IsNullOrEmpty(status))
                    {
                        return true; // If no status is provided, show all tenancies.
                    }

                    // Parse the string status to the corresponding enum value and compare.
                    if (Enum.TryParse(typeof(Tenancy.Status), status, true, out var parsedStatus))
                    {
                        return t.TenancyStatus == (Tenancy.Status)parsedStatus;
                    }

                    return false; // Return false if the status doesn't match any known value.
                }
                return false; // If the tenancy is not of type Tenancy, exclude it.
            };

            // Refresh the view to apply the newly set filter.
            _tenancyCollectionView.Refresh();
        }

    }
}

