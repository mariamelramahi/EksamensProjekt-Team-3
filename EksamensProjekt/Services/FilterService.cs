using EksamensProjekt.Models;
using EksamensProjekt.Services;
using System.Windows.Navigation;



public class FilterService
{
    // Auto-implemented property to store whether the filter is enabled or not
    public bool IsFilterAEnabled { get; set; }
    public bool IsFilterBEnabled { get; set; }
    public bool IsFilterCEnabled { get; set; }
    public bool IsFilterDEnabled => true;



    public bool ApplyFilter(Tenancy tenancy)
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
            // Filter B: Only include tenancies that are "Vacant"
            passesFilter &= tenancy.TenancyStatus == TenancyStatus.Vacant;
        }

        if (IsFilterCEnabled)
        {
            // Filter C: Only include tenancies that are "UnderRenovation"
            passesFilter &= tenancy.TenancyStatus == TenancyStatus.UnderRenovation;
        }

        if (IsFilterDEnabled)
        {
            // Filter D: Only include tenancies with IsDeleted set to false
            passesFilter &= tenancy.IsDeleted == false;
        }

        return passesFilter;
    }
    public bool ApplyFilter(AddressAndMatchScore addressAndScore)
    {
        if (addressAndScore == null)
            return false;

        bool passesFilter = true;

        if (IsFilterAEnabled)
        {
            // Filter A: Only include tenancies that are "Occupied"
            passesFilter &= addressAndScore.MatchScore == "Type A"; // shorthand for passesFilter = passesFilter && condition
        }

        if (IsFilterBEnabled)
        {
            // Filter B: Only include tenancies that are "Vacant"
            passesFilter &= addressAndScore.MatchScore == "Type B";
        }

        if (IsFilterCEnabled)
        {
            // Filter C: Only include tenancies that are "UnderRenovation"
            passesFilter &= addressAndScore.MatchScore == "Type C";
        }

        if (IsFilterDEnabled)
        {
            // Filter D: Only include tenancies with IsDeleted set to false
            passesFilter &= addressAndScore.MatchScore == "Type D";
        }

        return passesFilter;
    }
}




    


