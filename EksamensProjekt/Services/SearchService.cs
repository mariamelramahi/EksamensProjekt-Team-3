using EksamensProjekt.Models;
using EksamensProjekt.Models.Repositories;

namespace EksamensProjekt.Services
{
    public class SearchService
    {
        private readonly IRepo<Tenancy> _tenancyRepo;
        private readonly IRepo<Tenant> _tenantRepo;

        // Constructor with repository injection
        public SearchService(IRepo<Tenancy> tenancyRepo, IRepo<Tenant> tenantRepo)
        {
            _tenancyRepo = tenancyRepo;
            _tenantRepo = tenantRepo;
        }

        // Search for tenancies by zip code and/or street
        public List<Tenancy> SearchTenancies(string zipCode, string street)
        {
            // Fetch all tenancies from the repository
            var allTenancies = _tenancyRepo.ReadAll().ToList();

            // Filter tenancies based on the provided zipCode and street
            var filteredTenancies = allTenancies.Where(t =>
                (string.IsNullOrEmpty(zipCode) || t.address?.ZipCode.Contains(zipCode) == true) &&
                (string.IsNullOrEmpty(street) || t.address?.Street.Contains(street) == true)
            ).ToList();

            return filteredTenancies;
        }
        // Search for tenants by FirstName, LastName, PhoneNumber, or Email
        public List<Tenant> SearchTenants(string firstName, string lastName, string phoneNumber, string email)
        {
            // Fetch all tenants from the repository
            var allTenants = _tenantRepo.ReadAll().ToList();

            // Filter tenants based on the provided firstName, lastName, phoneNumber, and email. string.IsNullOrEmpty ensures that the filter will not
            //be enabled if not applied
            var filteredTenants = allTenants.Where(t =>
                (string.IsNullOrEmpty(firstName) || t.FirstName.Contains(firstName, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(lastName) || t.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(phoneNumber) || t.PhoneNumber.Contains(phoneNumber)) &&
                (string.IsNullOrEmpty(email) || t.Email.Contains(email, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            return filteredTenants;
        }
    }
}
