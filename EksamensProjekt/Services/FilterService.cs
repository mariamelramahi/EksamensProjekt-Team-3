using EksamensProjekt.Models;
using EksamensProjekt.Models.Models.DTO;



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
    public bool ApplyFilter(AddressMatchResult addressMatchResult)
    {
        if (addressMatchResult == null)
            return false;

        bool passesFilter = true;

        if (IsFilterAEnabled)
        {
            // Example filter: MatchScore is "Type A"
            passesFilter &= addressMatchResult.PotentialMatches.Any(match => match.MatchScore == "Type A");
        }

        if (IsFilterBEnabled)
        {
            // Example filter: MatchScore is "Type B"
            passesFilter &= addressMatchResult.PotentialMatches.Any(match => match.MatchScore == "Type B");
        }

        if (IsFilterCEnabled)
        {
            // Example filter: MatchScore is "Type C"
            passesFilter &= addressMatchResult.PotentialMatches.Any(match => match.MatchScore == "Type C");
        }

        return passesFilter;
    }
}




    


