using EksamensProjekt.Models;
using System.ComponentModel; // filtering capabilities.

namespace EksamensProjekt.Services;

public class FilterService
{

    public bool IsFilterAEnabled { get; set; }
    public bool IsFilterBEnabled { get; set; }
    public bool IsFilterCEnabled { get; set; }

    // metoder til checkbox
    // A checkbox
    // B
    // C






    // OLD



    ////private readonly ICollectionView _tenancyCollectionView;
    ////private readonly ObservableCollection<Tenancy> _tenancies;

    //public FilterService(ObservableCollection<Tenancy> tenancies)
    //{
    //    _tenancies = tenancies;
    //    _tenancyCollectionView = CollectionViewSource.GetDefaultView(_tenancies);
    //}
    //// This property is used to expose the CollectionView for binding
    //public ICollectionView TenancyCollectionView => _tenancyCollectionView;
    //// Method to apply the filters and refresh the view
    //public void ApplyFilters(string zipCode, string street, string status)
    //{
    //    // Apply the filter logic
    //    ApplyTenancyFilters(_tenancyCollectionView, zipCode, street, status);

    //    // Refresh the view to reflect the changes
    //    _tenancyCollectionView.Refresh();
    //}

    // Method that defines the actual filter criteria
    //public void ApplyTenancyFilters(ICollectionView collectionView, string zipCode, string street, string status)
    //{
    //    //collectionView.Filter = tenancy =>
    //    {
    //        if (tenancy is not Tenancy t) return false;

    //        // Filter by Zip Code if provided. If not provided it returns true, meaning the filter will not sort tenancies on zipcode
    //        //if zipcode is provided it evaluates which of the tenancies that contains specific zipcode, returning them true.
    //        //tenancies not containing zipcode will return false
    //        bool matchesZipCode = string.IsNullOrEmpty(zipCode) || t.StandardAddress.ZipCode.Contains(zipCode);

    //        // Filter by Street if provided
    //        bool matchesStreet = string.IsNullOrEmpty(street) || t.StandardAddress.StreetName.Contains(street);

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

