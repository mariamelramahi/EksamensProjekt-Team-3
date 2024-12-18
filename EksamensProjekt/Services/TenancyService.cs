using EksamensProjekt.Models;
using EksamensProjekt.Repos;
using EksamensProjekt.Repositories;
using Microsoft.Data.SqlClient;
using System.Windows;

namespace EksamensProjekt.Services
{
    public class TenancyService
    {
        private readonly IRepo<Tenancy> tenancyRepo;
        private readonly IRepo<Tenant> tenantRepo;
        private readonly IRepo<Address> AddressRepo;
        private readonly ITenancyTenant tenancyTenantRepo;

        // Constructor or property injection 
        public TenancyService(IRepo<Tenancy> tenancyRepo, IRepo<Tenant> tenantRepo, IRepo<Address> addressRepo, ITenancyTenant tenancyTenantRepo)
        {
            this.tenancyRepo = tenancyRepo;
            this.tenantRepo = tenantRepo;
            this.AddressRepo = addressRepo;
            this.tenancyTenantRepo = tenancyTenantRepo;
        }
        public void CreateNewTenancy(
                TenancyStatus tenancyStatus,
                DateTime? moveInDate,
                DateTime? moveOutDate,
                int squareMeter,
                int rent,
                int rooms,
                int bathRooms,
                bool petsAllowed,
                List<Tenant> tenants,
                Address Address,
                Company? company,
                Organization organization)
        {
            // Validate essential input fields
            if (Address == null)
            {
                throw new ArgumentNullException(nameof(Address), "Address cannot be null.");
            }

            if (company == null)
            {
                throw new ArgumentNullException(nameof(company), "Company cannot be null.");
            }

            // Use the empty constructor to create a new Tenancy object
            Tenancy newTenancy = new Tenancy
            {
                TenancyStatus = tenancyStatus,
                MoveInDate = moveInDate ?? DateTime.Now,
                MoveOutDate = moveOutDate ?? DateTime.Now.AddYears(1),
                SquareMeter = squareMeter,
                Rent = rent,
                Rooms = rooms,
                Bathrooms = bathRooms,
                PetsAllowed = petsAllowed,
                Tenants = tenants ?? new List<Tenant>(),
                Address = Address,
                Company = company,
                Organization = organization
            };

            // Save the new tenancy using the repository
            tenancyRepo.Create(newTenancy);
        }
        public List<Tenancy> GetAllTenancies()
        {
            // Fetch all tenancies from the repository
            return tenancyRepo.ReadAll().ToList();
        }

        public void UpdateTenancy(Tenancy selectedTenancy)
        {
            // Fetch the existing tenancy from the repository using its ID
            Tenancy? existingTenancy = tenancyRepo.GetByID(selectedTenancy.TenancyID);

            // Check if the tenancy exists
            if (existingTenancy == null)
            {
                MessageBox.Show($"Lejemål med ID {selectedTenancy.TenancyID} finnes ikke.");
                return;
            }

            // Update only non-null properties
            if (selectedTenancy.TenancyStatus.HasValue)
                existingTenancy.TenancyStatus = selectedTenancy.TenancyStatus;

            if (selectedTenancy.MoveInDate.HasValue)
                existingTenancy.MoveInDate = selectedTenancy.MoveInDate;

            if (selectedTenancy.MoveOutDate.HasValue)
                existingTenancy.MoveOutDate = selectedTenancy.MoveOutDate;

            if (selectedTenancy.SquareMeter != 0)
                existingTenancy.SquareMeter = selectedTenancy.SquareMeter;

            if (selectedTenancy.Rent.HasValue)
                existingTenancy.Rent = selectedTenancy.Rent;

            if (selectedTenancy.Rooms.HasValue)
                existingTenancy.Rooms = selectedTenancy.Rooms;

            if (selectedTenancy.Bathrooms.HasValue)
                existingTenancy.Bathrooms = selectedTenancy.Bathrooms;

            if (selectedTenancy.PetsAllowed.HasValue)
                existingTenancy.PetsAllowed = selectedTenancy.PetsAllowed;

            if (selectedTenancy.Tenants != null && selectedTenancy.Tenants.Count > 0)
                existingTenancy.Tenants = selectedTenancy.Tenants;

            if (selectedTenancy.Address != null)
                existingTenancy.Address = selectedTenancy.Address;

            if (selectedTenancy.Company != null)
                existingTenancy.Company = selectedTenancy.Company;

            if (selectedTenancy.Organization != null)
                existingTenancy.Organization = selectedTenancy.Organization;

            // Save the updated tenancy
            tenancyRepo.Update(existingTenancy);
            MessageBox.Show($"Lejemål med ID {selectedTenancy.TenancyID} er blevet opdateret.");
        }


        public void SoftDeleteTenancy(Tenancy selectedTenancy)
        {
            // Fetch the existing tenancy from the repository using its ID
            Tenancy? tenancyToDelete = tenancyRepo.GetByID(selectedTenancy.TenancyID);

            // Check if the tenancy exists
            if (tenancyToDelete == null)
            {
                MessageBox.Show($"Lejemål med ID {selectedTenancy.TenancyID} blev ikke fundet.");
                return;
            }

            // Soft delete the tenancy by setting the IsDeleted flag to true
            tenancyToDelete.IsDeleted = true;

            // Save the updated tenancy (soft delete)
            tenancyRepo.Update(tenancyToDelete);
            MessageBox.Show($"Lejemål med ID {selectedTenancy.TenancyID} er blevet slettet.");
        }

        public List<Tenant> GetAllTenants()
        {
            // Fetch all tenants from the repository
            return tenantRepo.ReadAll().ToList();
        }
        public Tenant CreateNewTenant()
        {
            // Create a new Tenant object with default values
            Tenant tenant = new Tenant
            {
                FirstName = "Fornavn",
                LastName = "Efternavn",
                PhoneNum = "PhoneNum",
                Email = "Email"
            };

            // Use tenantRepo to save the new Tenant object to the database
            tenantRepo.Create(tenant);

            // Return the newly created Tenant object
            return tenant;
        }


        // Method to save the tenant after editing in the ViewModel
        public void SaveTenant(Tenant tenant)
        {
            if (tenant != null)
            {
                tenantRepo.Create(tenant);
            }
        }


        
        public void DeleteTenant(Tenant tenant)
        {
            tenantRepo.Delete(tenant.TenantID);
        }


        
        public void AddTenantToTenancy(Tenancy tenancy, Tenant tenant)
        {
            if (tenancy == null || tenant == null)
                throw new ArgumentNullException("Tenancy or Tenant cannot be null.");

            tenancyTenantRepo.AddTenantToTenancy(tenancy.TenancyID, tenant.TenantID);
        }

        //remove from tenancytenant table
        public void RemoveTenancyTenant(Tenancy tenancy, Tenant tenant)
        {
            if (tenancy == null || tenant == null)
                throw new ArgumentNullException("Tenancy or Tenant cannot be null.");

            tenancyTenantRepo.RemoveTenantFromTenancy(tenancy.TenancyID, tenant.TenantID);
        }


        public void UpdateTenant(Tenant tenant)
        {
            try
            {
                // Fetch the existing tenant from the repository using its ID
                Tenant? existingTenant = tenantRepo.GetByID(tenant.TenantID);

                // Check if the tenant exists
                if (existingTenant == null)
                {
                    MessageBox.Show($"Lejer med ID {tenant.TenantID} blev ikke fundet.");
                    return;
                }

                // Update only non-null properties
                if (!string.IsNullOrEmpty(tenant.FirstName))
                    existingTenant.FirstName = tenant.FirstName;

                if (!string.IsNullOrEmpty(tenant.LastName))
                    existingTenant.LastName = tenant.LastName;

                if (!string.IsNullOrEmpty(tenant.PhoneNum))
                {
                    if (tenant.PhoneNum.Length != 8)
                    {
                        throw new ArgumentException("Telefonnummeret skal være 8 tal.");
                    }
                    existingTenant.PhoneNum = tenant.PhoneNum;
                }

                if (!string.IsNullOrEmpty(tenant.Email))
                    existingTenant.Email = tenant.Email;

                // Save the updated tenant
                tenantRepo.Update(existingTenant);
                MessageBox.Show($"Lejer med ID {tenant.TenantID} er blevet opdateret.");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("truncated"))
                {
                    MessageBox.Show("Telefonnummeret eller en anden værdi er for lang. Kontrollér længden og prøv igen.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("En fejl opstod: " + ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
