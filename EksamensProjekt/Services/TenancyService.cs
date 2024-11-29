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

        //public void DeleteTenancy(Tenancy selectedTenancy)
        //{
        //    // Fetch the existing tenancy from the repository using its ID
        //    Tenancy? tenancyToDelete = tenancyRepo.GetByID(selectedTenancy.TenancyID);

        //    // Check if the tenancy exists
        //    if (tenancyToDelete == null)
        //    {
        //        MessageBox.Show($"Lejemål med ID {selectedTenancy.TenancyID} blev ikke fundet.");
        //        return;
        //    }

        //    // Delete the tenancy from the repository
        //    tenancyRepo.Delete(tenancyToDelete);
        //    MessageBox.Show($"Lejemål med ID {selectedTenancy.TenancyID} er blevet slettet.");
        //}


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
            // Create a new Tenant object with default values (not saved yet)
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











        //private void UpdateTenancyDetailsFromExcel(Tenancy tenancy, ModifiedExcelAddress importedAddress)
        //{
        //    // Update the address fields if available
        //    if (!string.IsNullOrEmpty(importedAddress.StreetName))
        //        tenancy.StandardAddress.StreetName = importedAddress.StreetName;

        //    if (!string.IsNullOrEmpty(importedAddress.Number))
        //        tenancy.StandardAddress.Number = importedAddress.Number;

        //    if (!string.IsNullOrEmpty(importedAddress.Floor))
        //        tenancy.StandardAddress.Floor = importedAddress.Floor;

        //    if (!string.IsNullOrEmpty(importedAddress.ZipCode))
        //        tenancy.StandardAddress.ZipCode = importedAddress.ZipCode;

        //    if (!string.IsNullOrEmpty(importedAddress.City))
        //        tenancy.StandardAddress.City = importedAddress.City;

        //    if (!string.IsNullOrEmpty(importedAddress.Country))
        //        tenancy.StandardAddress.Country = importedAddress.Country;

        //    // Update tenancy-specific fields if available
        //    if (importedAddress.MoveInDate.HasValue)
        //        tenancy.MoveInDate = importedAddress.MoveInDate.Value;

        //    if (importedAddress.MoveOutDate.HasValue)
        //        tenancy.MoveOutDate = importedAddress.MoveOutDate.Value;

        //    if (importedAddress.Rent > 0)
        //        tenancy.Rent = importedAddress.Rent;
        //}




        //public List<MatchResult> CompareImportedAddressesWithDatabase(List<ModifiedExcelAddress> importedAddresses)
        //{
        //    // Liste til at gemme resultaterne
        //    List<MatchResult> matchResults = new List<MatchResult>();
        //    List<Tenancy> tenancies = tenancyRepo.GetAllTenancies();
        //    List<StandardAddress> databaseAddresses = standardAddressRepo.GetAllStandardAddresses();

        //    // Trin 1: Standardiser og match adresser mod database-adresser først
        //    foreach (var importedAddress in importedAddresses)
        //    {
        //        string bestMatchType = "Type D"; // Default til "Type D" hvis intet match findes
        //        StandardAddress bestMatch = null;
        //        double bestScore = 0;

        //        // Find det bedste match i database-adresserne
        //        foreach (var dbAddress in databaseAddresses)
        //        {
        //            // Beregn match-scoren mellem database-adressen og den importerede adresse
        //            string matchType = CalculateAddressMatchScore(dbAddress, importedAddress);
        //            double score = GetMatchScoreValue(matchType);

        //            // Opdater det bedste match, hvis scoren er højere
        //            if (score > bestScore)
        //            {
        //                bestScore = score;
        //                bestMatchType = matchType;
        //                bestMatch = dbAddress;
        //            }
        //        }

        //        // Hvis der ikke findes nogen match i databasen, tildeles "Type D"
        //        if (bestMatch == null)
        //        {
        //            bestMatchType = "Type D";
        //        }

        //        // Tilføj match-resultatet til listen
        //        matchResults.Add(new MatchResult
        //        {
        //            ImportedAddress = importedAddress,
        //            DatabaseAddress = bestMatch,
        //            MatchType = bestMatchType
        //        });
        //    }

        //    // Trin 2: Efter at have fundet de bedste matches, tjek om de allerede er oprettet som en tenancy
        //    foreach (var match in matchResults)
        //    {
        //        if (match.MatchType == "Type A")
        //        {
        //            // Find eksisterende tenancy baseret på det bedste match
        //            var existingTenancy = tenancies.FirstOrDefault(t =>
        //                CalculateAddressMatchScore(t.StandardAddress, match.ImportedAddress) == "Type A");

        //            if (existingTenancy != null)
        //            {
        //                // Opdater eksisterende tenancy med de nye data fra den importerede adresse
        //                existingTenancy.TenancyStatus = match.ImportedAddress.TenancyStatus ?? existingTenancy.TenancyStatus;
        //                existingTenancy.MoveInDate = match.ImportedAddress.MoveInDate ?? existingTenancy.MoveInDate;
        //                existingTenancy.MoveOutDate = match.ImportedAddress.MoveOutDate ?? existingTenancy.MoveOutDate;
        //                existingTenancy.Rent = match.ImportedAddress.Rent > 0 ? match.ImportedAddress.Rent : existingTenancy.Rent;
        //                existingTenancy.SquareMeter = match.ImportedAddress.SquareMeter ?? existingTenancy.SquareMeter;
        //                existingTenancy.Rooms = match.ImportedAddress.Rooms > 0 ? match.ImportedAddress.Rooms : existingTenancy.Rooms;
        //                existingTenancy.BathRooms = match.ImportedAddress.BathRooms > 0 ? match.ImportedAddress.BathRooms : existingTenancy.BathRooms;
        //                existingTenancy.PetsAllowed = match.ImportedAddress.PetsAllowed ?? existingTenancy.PetsAllowed;
        //                existingTenancy.Tenants = match.ImportedAddress.Tenants ?? existingTenancy.Tenants;
        //                existingTenancy.Company = match.ImportedAddress.Company ?? existingTenancy.Company;
        //            }
        //        }
        //    }

        //    return matchResults;
        //}


        //public static string CalculateAddressMatchScore(Tenancy standardAddress, Tenancy importedAddress)
        //{
        //    double streetMatchScore = CalculateLevenshteinMatchScore(standardAddress.StreetName, importedAddress.StreetName) * 0.4;
        //    double zipCodeMatchScore = CalculateLevenshteinMatchScore(standardAddress.ZipCode, importedAddress.ZipCode) * 0.3;
        //    double cityMatchScore = CalculateLevenshteinMatchScore(standardAddress.City, importedAddress.City) * 0.1;
        //    double numberMatchScore = CalculateLevenshteinMatchScore(standardAddress.Number, importedAddress.Number) * 0.05;
        //    double floorMatchScore = CalculateLevenshteinMatchScore(standardAddress.Floor, importedAddress.Floor) * 0.05;
        //    double countryMatchScore = CalculateLevenshteinMatchScore(standardAddress.Country, importedAddress.Country) * 0.1;

        //    double totalMatchScore = streetMatchScore + zipCodeMatchScore + cityMatchScore + numberMatchScore + floorMatchScore + countryMatchScore;

        //    if (totalMatchScore >= 90) return "Type A";
        //    else if (totalMatchScore >= 75) return "Type B";
        //    else if (totalMatchScore >= 50) return "Type C";
        //    else return "Type D";
        //}

        //// Method to convert match type to numerical score for sorting
        //private double GetMatchScoreValue(string matchType)//can be removed if matchType are enums
        //{
        //    return matchType switch
        //    {
        //        "Type A" => 100,
        //        "Type B" => 75,
        //        "Type C" => 50,
        //        "Type D" => 25,
        //        _ => 0
        //    };
        //}

        //// Helper method to calculate Levenshtein match score
        //public static double CalculateLevenshteinMatchScore(string standardValue, string importedValue)
        //{
        //    if (string.IsNullOrEmpty(standardValue) || string.IsNullOrEmpty(importedValue))
        //    {
        //        return 0.0; // If one of the values is empty, return no match.
        //    }

        //    // Calculate the Levenshtein distance between the two values.
        //    int distance = LevenshteinDistance(standardValue, importedValue);

        //    // Normalize the Levenshtein distance to a match percentage.
        //    int maxLength = Math.Max(standardValue.Length, importedValue.Length);
        //    double matchPercentage = (1 - (double)distance / maxLength) * 100;

        //    return matchPercentage;
        //}

        //// Levenshtein Distance algorithm
        //public static int LevenshteinDistance(string source, string target)
        //{
        //    int n = source.Length;
        //    int m = target.Length;
        //    int[,] d = new int[n + 1, m + 1];

        //    // Initialize the matrix
        //    for (int i = 0; i <= n; d[i, 0] = i++) ;
        //    for (int j = 0; j <= m; d[0, j] = j++) ;

        //    // Fill the matrix with the minimum edit distance calculations
        //    for (int i = 1; i <= n; i++)
        //    {
        //        for (int j = 1; j <= m; j++)
        //        {
        //            int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;
        //            d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
        //        }
        //    }

        //    return d[n, m];
        //}

    }
}
