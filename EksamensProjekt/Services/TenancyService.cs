using EksamensProjekt.Model;

namespace EksamensProjekt.Services
{
    public class TenancyService
    {
        public IRepo<Tenancy> tenancyRepo;
        public IRepo<Tenant> tenantRepo;

        public void CreateNewTenancy(
           Tenancy.Status tenancyStatus,
           DateTime moveInDate,
           DateTime moveOutDate,
           string squareMeter,
           int rent,
           int rooms,
           int bathRooms,
           bool petsAllowed,
           List<Tenant> tenants,
           Address standardAddress,
           Company company)
        {
            // Validate essential input fields
            if (standardAddress == null)
            {
                throw new ArgumentNullException(nameof(standardAddress), "Address cannot be null.");
            }

            if (company == null)
            {
                throw new ArgumentNullException(nameof(company), "Company cannot be null.");
            }

            // Use the constructor to create a new Tenancy object
            Tenancy tenancy = new Tenancy(
                tenancyStatus,
                moveInDate,
                moveOutDate,
                squareMeter,
                rent,
                rooms,
                bathRooms,
                petsAllowed,
                tenants ?? new List<Tenant>(), //made nullable field in case of no tenants registered
                standardAddress,
                company);

            tenancyRepo.Add(tenancy);
        }

        public void UpdateTenancyDetails(
            int tenancyID,
            Tenancy.Status? tenancyStatus = null,
            DateTime? moveInDate = null,
            DateTime? moveOutDate = null,
            string? squareMeter = null,
            int? rent = null,
            int? rooms = null,
            int? bathRooms = null,
            bool? petsAllowed = null,
            List<Tenant>? tenants = null,
            Address? standardAddress = null,
            Company? company = null)
        {
            // using GetById to retrieve tenancy
            Tenancy? tenancy = tenancyRepo.GetById(tenancyID);

            // Check if the tenancy exists
            if (tenancy == null)
            {
                Console.WriteLine($"Tenancy with ID {tenancyID} not found.");
                return;
            }

            // Checks each field for values
            if (tenancyStatus.HasValue)
                tenancy.TenancyStatus = tenancyStatus.Value;

            if (moveInDate.HasValue)
                tenancy.MoveInDate = moveInDate.Value;

            if (moveOutDate.HasValue)
                tenancy.MoveOutDate = moveOutDate.Value;

            if (!string.IsNullOrEmpty(squareMeter))
                tenancy.SquareMeter = squareMeter;

            if (rent.HasValue)
                tenancy.Rent = rent.Value;

            if (rooms.HasValue)
                tenancy.Rooms = rooms.Value;

            if (bathRooms.HasValue)
                tenancy.BathRooms = bathRooms.Value;

            if (petsAllowed.HasValue)
                tenancy.PetsAllowed = petsAllowed.Value;

            if (tenants != null)
                tenancy.Tenants = tenants;

            if (standardAddress != null)
                tenancy.StandardAddress = standardAddress;

            if (company != null)
                tenancy.Company = company;

            tenancyRepo.Update(tenancy);
        }


        public void CreateNewTenant(string firstName, string lastName, string phoneNumber, string email) 
        {

            // Validate essential input fields
            if (firstName == null)
            {
                throw new ArgumentNullException(nameof(firstName), "Firstname cannot be null.");
            }

            if (lastName == null)
            {
                throw new ArgumentNullException(nameof(lastName), "Lastname cannot be null.");
            }

            // Validate essential input fields
            if (phoneNumber == null)
            {
                throw new ArgumentNullException(nameof(phoneNumber), "Phonenumber cannot be null.");
            }

            if (email == null)
            {
                throw new ArgumentNullException(nameof(email), "Email cannot be null.");
            }
            Tenant tenant = new Tenant(
                firstName,
                lastName,
                phoneNumber,
                email
                );
            tenantRepo.Add(tenant);
        }

        public void CalculateAdressMatchScore() { }
    }
}
