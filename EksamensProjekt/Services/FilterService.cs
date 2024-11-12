using EksamensProjekt.Model;

using System.Collections.ObjectModel; // Provides ObservableCollection for data binding.
using System.ComponentModel; // Provides ICollectionView and filtering capabilities.
using System.Linq;
using System.Windows.Data; // Provides CollectionViewSource to create ICollectionView.

namespace EksamensProjekt.Services
{
    public class FilterService
    {

        private readonly ICollectionView _tenancyCollectionView; // Read-only view used for filtering and sorting tenancies.
        private readonly ObservableCollection<Tenancy> _tenancies; // Collection of tenancies managed by this service.
'

        public FilterService(ObservableCollection<Tenancy> tenancies)
        {
            _tenancies = tenancies; 

            //GetDefaultView provides features that allows you to filter, sort etc. on collections without modifying original data
            _tenancyCollectionView = CollectionViewSource.GetDefaultView(_tenancies);
        }

        //this is for binding to viewModelLayer    
        public ICollectionView TenancyCollectionView => _tenancyCollectionView;

        public void ApplyTenancyFilters(ICollectionView collectionView, string zipCode, string street, string status)
        {
            collectionView.Filter = tenancy =>
            {
                // Ensure the tenancy is of the correct type
                if (tenancy is not Tenancy t) return false;

                // Filter by Zip Code if provided
                bool matchesZipCode = string.IsNullOrEmpty(zipCode) || t.StandardAddress.ZipCode.Contains(zipCode);

                // Filter by Street if provided
                bool matchesStreet = string.IsNullOrEmpty(street) || t.StandardAddress.StreetName.Contains(street);

                // Filter by Status if provided
                bool matchesStatus = string.IsNullOrEmpty(status) || t.TenancyStatus.ToString().Equals(status, StringComparison.OrdinalIgnoreCase);

                // Return true if any filter is null/empty or matches the corresponding field
                return matchesZipCode && matchesStreet && matchesStatus;
            };
        }


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
}

