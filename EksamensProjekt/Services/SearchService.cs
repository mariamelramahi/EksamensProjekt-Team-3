using EksamensProjekt.Models;
using EksamensProjekt.Models.Models.DTO;

namespace EksamensProjekt.Services
{

    public class SearchService
    {

        // Search logic for tenancies
        public bool ApplySearchFilter(Tenancy tenancy, string searchInput)
        {
            if (string.IsNullOrEmpty(searchInput))
                return true; // No search input means all tenancies match

            var searchTerms = searchInput
                .Split(',')
                .Select(term => term.Trim())
                .Where(term => !string.IsNullOrEmpty(term))
                .ToList();

            // Check if all of the search terms match either the address or tenant name
            bool allTermsMatch = searchTerms.All(term =>
                (tenancy.Address?.ToString().Contains(term, StringComparison.OrdinalIgnoreCase) == true) ||
                (tenancy.Tenants?.Any(tenant =>
                    tenant.ToString().Contains(term, StringComparison.OrdinalIgnoreCase)) == true)
            );

            return allTermsMatch;
        }

        public bool ApplySearchFilter(AddressMatchResult addressMatchResult, string searchInput)
        {
            if (string.IsNullOrEmpty(searchInput))
                return true; // No search input means all uploaded addresses match

            // Split the search input into individual terms
            var searchTerms = searchInput
                .Split(',')
                .Select(term => term.Trim())
                .Where(term => !string.IsNullOrEmpty(term))
                .ToList();

            // Check if all search terms match the ImportedAddress only
            bool allTermsMatch = searchTerms.All(term =>
                addressMatchResult.ImportedAddress?.ToString().Contains(term, StringComparison.OrdinalIgnoreCase) == true);

            return allTermsMatch;
        }


    }

}
