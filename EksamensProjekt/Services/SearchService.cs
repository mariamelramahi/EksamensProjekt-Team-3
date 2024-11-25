using EksamensProjekt.Models;

namespace EksamensProjekt.Services
{

    public class SearchService
    {
        // Assuming the tenancy repo and tenant repo are initialized here

        // Search logic for tenancies
        public bool ApplySearchFilter(Address tenancy, string searchInput)
        {
            if (string.IsNullOrEmpty(searchInput))
                return true; // No search input means all tenancies match
            var searchTerms = searchInput.Split(',').Select(term => term.Trim()).Where(term => !string.IsNullOrEmpty(term)).ToList();

            // Check if all of the search terms match either the tenancy address or tenant name
            bool allTermsMatch = searchTerms.All(term =>
                (tenancy.Address?.ToString().Contains(term, StringComparison.OrdinalIgnoreCase) == true) ||
                (tenancy.Tenants?.Any(tenant => tenant.ToString().Contains(term, StringComparison.OrdinalIgnoreCase)) == true));

            // Return true if all search terms match either address or tenant
            return allTermsMatch;
        }
    }


    //public class SearchService
    //{
    //    private readonly IRepo<Address> _tenancyRepo;
    //    private readonly IRepo<Tenant> _tenantRepo;

    //    // Constructor with repository injection
    //    public SearchService(IRepo<Address> tenancyRepo, IRepo<Tenant> tenantRepo)
    //    {
    //        _tenancyRepo = tenancyRepo;
    //        _tenantRepo = tenantRepo;
    //    }

    //    // Search for tenancies by zip code and/or street
    //    public List<Address> SearchTenancies(string zipCode, string street)
    //    {
    //        // Fetch all tenancies from the repository
    //        var allTenancies = _tenancyRepo.ReadAll().ToList();

    //        // Filter tenancies based on the provided zipCode and street
    //        var filteredTenancies = allTenancies.Where(t =>
    //            (string.IsNullOrEmpty(zipCode) || t.Address?.Zipcode.Contains(zipCode) == true) &&
    //            (string.IsNullOrEmpty(street) || t.Address?.Street.Contains(street) == true)
    //        ).ToList();

    //        return filteredTenancies;
    //    }
    //    // Search for tenants by FirstName, LastName, PhoneNumber, or Email
    //    public List<Tenant> SearchTenants(string firstName, string lastName, string phoneNumber, string email)
    //    {
    //        // Fetch all tenants from the repository
    //        var allTenants = _tenantRepo.ReadAll().ToList();

    //        // Filter tenants based on the provided firstName, lastName, phoneNumber, and email. string.IsNullOrEmpty ensures that the filter will not
    //        //be enabled if not applied
    //        var filteredTenants = allTenants.Where(t =>
    //            (string.IsNullOrEmpty(firstName) || t.FirstName.Contains(firstName, StringComparison.OrdinalIgnoreCase)) &&
    //            (string.IsNullOrEmpty(lastName) || t.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase)) &&
    //            (string.IsNullOrEmpty(phoneNumber) || t.PhoneNum.Contains(phoneNumber)) &&
    //            (string.IsNullOrEmpty(email) || t.Email.Contains(email, StringComparison.OrdinalIgnoreCase))
    //        ).ToList();

    //        return filteredTenants;
    //    }
    //}
}
