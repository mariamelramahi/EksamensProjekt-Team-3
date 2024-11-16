using EksamensProjekt.Models;
using EksamensProjekt.Services;
using System.Windows.Navigation;



public class FilterService
{
    // Auto-implemented property to store whether the filter is enabled or not
    public bool IsFilterAEnabled { get; set; }
    public bool IsFilterBEnabled { get; set; }
    public bool IsFilterCEnabled { get; set; }



    public bool ApplyCheckboxFilter(Tenancy tenancy)
    {
        if (tenancy == null)
            return false;

        bool passesFilter = true;

        if (IsFilterAEnabled)
        {
            // Filter A: Only include tenancies that are "Occupied"
            passesFilter &= tenancy.TenancyStatus == TenancyStatus.Occupied; // shorthand for passesFilter = passesFilter && condition
        }

        if (IsFilterBEnabled)
        {
            // Filter B: Only include tenancies that allow pets
            passesFilter &= tenancy.PetsAllowed == true;
        }

        if (IsFilterCEnabled)
        {
            // Filter C: Only include tenancies with rent below 5000 (example threshold)
            passesFilter &= tenancy.Rent.HasValue && tenancy.Rent < 10000;
        }

        return passesFilter;
    }
}




    // Apply the filter and set the IsFilterAEnabled property directly within this method
    //public bool ApplyTenancyFilter(Tenancy tenancy)
    //{
        //    if (tenancy == null)
        //    {
        //        IsFilterAEnabled = false; // If tenancy is null, filter is disabled
        //        return IsFilterAEnabled;
        //    }

        //    // If status is null or empty, consider all tenancies as matching (no filter applied)
        //    if (string.IsNullOrEmpty(status))
        //    {
        //        IsFilterAEnabled = true; // No filter applied, all tenancies are considered matching
        //        return IsFilterAEnabled;
        //    }

        //    // Try to parse the status string into a TenancyStatus enum
        //    if (Enum.TryParse<TenancyStatus>(status, true, out TenancyStatus parsedStatus))
        //    {
        //        // Check if the parsed status matches the tenancy's status
        //        IsFilterAEnabled = tenancy.TenancyStatus == parsedStatus;
        //    }
        //    else
        //    {
        //        // If the status is invalid (couldn't be parsed), disable the filter
        //        IsFilterAEnabled = false;
    //    return true;
    //}
    
        // Return the result of IsFilterAEnabled
        //return IsFilterAEnabled;
//}



    }// Method that defines the actual filter criteria
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


