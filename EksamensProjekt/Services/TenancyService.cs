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

        public List<MatchResult> CompareImportedAddressesWithDatabase(List<string> importedAddresses, List<string> databaseAddresses)
        {
            List<MatchResult> matchResults = new List<MatchResult>();

            // Loop through each imported address
            foreach (var importedAddress in importedAddresses)
            {
                // Compare the imported address with each database address
                foreach (var dbAddress in databaseAddresses)
                {
                    // Calculate the match score using the Levenshtein distance algorithm
                    string matchType = CalculateAddressMatchScore(dbAddress, importedAddress);

                    // Store the result with the imported address, database address, and match type
                    matchResults.Add(new MatchResult
                    {
                        ImportedAddress = importedAddress,
                        DatabaseAddress = dbAddress,
                        MatchType = matchType
                    });
                }
            }

            // Sort the results by match type to rank the best matches first
            var sortedResults = matchResults.OrderByDescending(r => GetMatchScore(r.MatchType)).ToList();

            return sortedResults;
        }

        // Method to calculate the address match score based on Levenshtein distance
        public string CalculateAddressMatchScore(string standardStreet, string importedAddress)
        {
            // Calculate the Levenshtein distance between the two addresses
            int distance = LevenshteinDistance(standardStreet, importedAddress);

            // Normalize the Levenshtein distance to get a match percentage
            int maxLength = Math.Max(standardStreet.Length, importedAddress.Length);
            double matchPercentage = (1 - (double)distance / maxLength) * 100;

            // Classify the result based on the match percentage into types A, B, or C
            string matchType;
            if (matchPercentage >= 90)
            {
                matchType = "Type A";  // 90% or greater (perfect or near-perfect match)
            }
            else if (matchPercentage >= 75)
            {
                matchType = "Type B";  // 75%-89% (close match, minor differences)
            }
            else if (matchPercentage >= 50)
            {
                matchType = "Type C";  // 50%-74% (substantial differences)
            }
            else
            {
                matchType = "Type D";  // below 50% (poor match)
            }

            return matchType;
        }

        // Levenshtein Distance algorithm to calculate the number of edits needed to transform one string into another
        public static int LevenshteinDistance(string source, string target)
        {
            int n = source.Length;
            int m = target.Length;
            int[,] d = new int[n + 1, m + 1];

            // If one string is empty, return the length of the other string (all insertions)
            if (n == 0) return m;
            if (m == 0) return n;

            // Initialize the matrix
            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 0; j <= m; d[0, j] = j++) ;

            // Fill the matrix with the minimum edit distance calculations
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }

        // Helpermethod to convert match type to a numerical score (for sorting purposes)
        private int GetMatchScore(string matchType)
        {
            switch (matchType)
            {
                case "Type A":
                    return 5;
                case "Type B":
                    return 3;
                case "Type C":
                    return 1;
                default:
                    return 0;  // For "Type D" or no match
            }
        }
    }

          

    
}
