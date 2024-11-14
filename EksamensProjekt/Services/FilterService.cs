using EksamensProjekt.Models;
using EksamensProjekt.Models.Repositories;

namespace EksamensProjekt.Services
{
    public class FilterService
    {
        public FilterService()
        {

        }

        // Method to apply tenancy status filter
        public bool MatchesStatusFilter(Tenancy tenancy, string status)
        {
            if (tenancy == null) return false;

            // If status is null or empty, consider all tenancies as matching (no filter applied)
            if (string.IsNullOrEmpty(status)) return true;

            // Try to parse the status string into a TenancyStatus enum
            if (Enum.TryParse<TenancyStatus>(status, true, out TenancyStatus parsedStatus))
            {
                // Return whether the parsed status matches the tenancy's status
                return tenancy.TenancyStatus == parsedStatus;
            }
            else
            {
                // If the status is invalid (couldn't be parsed), return false
                return false;
            }
        }

            // Method that defines the actual filter criteria
            //public void ApplyTenancyFilters(ICollectionView collectionView, string zipCode, string street, string status)
            //{
            //    collectionView.Filter = tenancy =>
            //    {
            //        if (tenancy is not Tenancy t) return false;

            //        // Filter by Zip Code if provided. If not provided it returns true, meaning the filter will not sort tenancies on zipcode
            //        //if zipcode is provided it evaluates which of the tenancies that contains specific zipcode, returning them true.
            //        //tenancies not containing zipcode will return false
            //        bool matchesZipCode = string.IsNullOrEmpty(zipCode) || t.address.ZipCode.Contains(zipCode);

            //        // Filter by Street if provided
            //        bool matchesStreet = string.IsNullOrEmpty(street) || t.address.Street.Contains(street);

            //        // Filter by Status if provided
            //        bool matchesStatus = string.IsNullOrEmpty(status) || t.TenancyStatus.ToString().Equals(status, StringComparison.OrdinalIgnoreCase);

            //        return matchesZipCode && matchesStreet && matchesStatus;
            //    };
            //}

            //public void FilterTenancyMatchType(string matchType, List<MatchResult> matchResults)
            //{
            //    if (_tenancyCollectionView == null || matchResults == null) return;

            //    // Set the filter predicate based on the selected match type
            //    _tenancyCollectionView.Filter = tenancy =>
            //    {
            //        if (tenancy is Tenancy t)
            //        {
            //            // Find the corresponding match result for this tenancy
            //            var matchResult = matchResults.FirstOrDefault(r =>
            //                r.ImportedAddress.Equals(t.ImportedAddress, StringComparison.OrdinalIgnoreCase) &&
            //                r.DatabaseAddress.Equals(t.DatabaseAddress, StringComparison.OrdinalIgnoreCase));

            //            if (matchResult != null)
            //            {
            //                // If no specific match type is selected, show all tenancies
            //                if (string.IsNullOrEmpty(matchType))
            //                    return true;

            //                // Check if the match type matches the filter criteria
            //                return matchResult.MatchType.Equals(matchType, StringComparison.OrdinalIgnoreCase);
            //            }
            //        }

            //        return false;
            //    };

            //    // Refresh the view to apply the filter
            //    _tenancyCollectionView.Refresh();
            //}

        }
}

