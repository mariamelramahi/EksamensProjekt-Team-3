using EksamensProjekt.Models;
using EksamensProjekt.Repos;

namespace EksamensProjekt.Services
{
    public class MatchService
    {
        IRepo<Tenancy> tenancyRepo;
        IRepo<Address> addressRepo;
        public MatchService(IRepo<Tenancy> tenancyRepo, IRepo<Address> addressRepo)
        {
            this.tenancyRepo = tenancyRepo;
            this.addressRepo = addressRepo;
        }

        public void ApproveMatches(List<AddressMatchResult> addressMatches)
        {
            var tenancies = tenancyRepo.ReadAll();
           
            foreach (var match in addressMatches)
            {

                // Check if the selected match corresponds to an existing tenancy
                var existingTenancy = tenancies.FirstOrDefault(t =>
                    t.Address.Street == match.SelectedMatch?.PotentialAddressMatch.Street &&
                    t.Address.Number == match.SelectedMatch?.PotentialAddressMatch.Number &&
                    t.Address.Zipcode == match.SelectedMatch?.PotentialAddressMatch.Zipcode &&
                    t.Address.Country == match.SelectedMatch?.PotentialAddressMatch.Country);

                if (existingTenancy != null)
                {
                    // Update the existing tenancy if a match is found
                    tenancyRepo.Update(existingTenancy);
                }
                else if (existingTenancy == null && match.SelectedMatch != null)
                {
                    // Create a new tenancy if no existing tenancy is found
                    tenancyRepo.Create(new Tenancy
                    {
                        Address = match.SelectedMatch.PotentialAddressMatch
                    });
                }

                // If no match is selected, mark it for user intervention
                if (match.SelectedMatch == null)
                {
                    match.IsUserSelectionRequired = true;
                }
            }
        }

        public List<AddressMatchResult> CompareImportedAddressesWithDatabase(List<Address> importedAddresses)
        {
            // Liste til at gemme resultaterne
            List<AddressMatchResult> matchResults = new List<AddressMatchResult>();
            List<Tenancy> tenancies = tenancyRepo.ReadAll().ToList();
            List<Address> databaseAddresses = addressRepo.ReadAll().ToList();
            //Samplelist for testing
            //List<Address> databaseAddresses = GetSampleDatabaseAddresses();

            // Paralleliser matchning af adresser for ydeevne
            Parallel.ForEach(importedAddresses, importedAddress =>
            {
                // Liste over potentielle matches
                List<AddressAndMatchScore> potentialMatches = new List<AddressAndMatchScore>();

                // Find potentielle matches i database-adresser
                foreach (var dbAddress in databaseAddresses)
                {
                    string matchType = CalculateAddressMatchScore(dbAddress, importedAddress);
                    double score = GetMatchScoreValue(matchType);

                    // Tilføj kun matches, der har en relevant score (eksempel: Type A-C)
                    if (score > 0)
                    {
                        potentialMatches.Add(new AddressAndMatchScore
                        {
                            PotentialAddressMatch = dbAddress,
                            MatchScore = matchType
                        });
                    }
                }

                // Sortér de potentielle matches efter score (højeste først)
                potentialMatches = potentialMatches
                    .OrderByDescending(pm => GetMatchScoreValue(pm.MatchScore))
                    .ToList();

                // Tilføj resultaterne til den samlede liste
                lock (matchResults)
                {
                    var automaticMatchScore = FindAutomaticMatchScore(potentialMatches);
                    matchResults.Add(new AddressMatchResult
                    {
                        ImportedAddress = importedAddress,
                        PotentialMatches = potentialMatches,
                        SelectedMatch = automaticMatchScore, 
                        IsUserSelectionRequired = automaticMatchScore == null
                    });
                }
            });

            
            return matchResults;
        }

        private AddressAndMatchScore? FindAutomaticMatchScore(List<AddressAndMatchScore> potentialMatches)
        {
            var bestMatch = potentialMatches.First();
            if (bestMatch.MatchScore == "Type A" || bestMatch.MatchScore == "Type B")
            {
                return bestMatch;
            }
            else
            {
                return null;
            }
        }


        public static string CalculateAddressMatchScore(Address databaseAddress, Address importedAddress)
        {
            double streetMatchScore = CalculateDamerauLevenshteinMatchScore(databaseAddress.Street, importedAddress.Street) * 0.5;
            double numberCodeMatchScore = CalculateDamerauLevenshteinMatchScore(databaseAddress.Number, importedAddress.Number) * 0.25;
            double floorNumberMatchScore = CalculateDamerauLevenshteinMatchScore(databaseAddress.FloorNumber, importedAddress.FloorNumber) * 0.05;
            double zipCodeMatchScore = CalculateDamerauLevenshteinMatchScore(databaseAddress.Zipcode, importedAddress.Zipcode) * 0.15;
            double countryMatchScore = CalculateDamerauLevenshteinMatchScore(databaseAddress.Country, importedAddress.Country) * 0.05;

            double totalMatchScore = streetMatchScore + zipCodeMatchScore + floorNumberMatchScore + numberCodeMatchScore + countryMatchScore;

            if (totalMatchScore >= 90) return "Type A";
            else if (totalMatchScore >= 75) return "Type B";
            else if (totalMatchScore >= 50) return "Type C";
            else return "Type D";
        }

        // Method to convert match type to numerical score for sorting
        private double GetMatchScoreValue(string matchType)//can be removed if matchType are enums
        {
            return matchType switch
            {
                "Type A" => 100,
                "Type B" => 75,
                "Type C" => 50,
                "Type D" => 25,
                _ => 0
            };
        }

        // Helper method to calculate Damerau-Levenshtein match score
        public static double CalculateDamerauLevenshteinMatchScore(string standardValue, string importedValue)
        {
            if (string.IsNullOrEmpty(standardValue) && string.IsNullOrEmpty(importedValue))
            {
                return 100.0; // Fuldt match, hvis begge værdier er tomme.
            }

            if (string.IsNullOrEmpty(standardValue) || string.IsNullOrEmpty(importedValue))
            {
                return 0.0; // Ingen match, hvis kun én værdi er tom.
            }

            // Brug Damerau-Levenshtein-afstand
            int distance = DamerauLevenshteinDistance(NormalizeString(standardValue), NormalizeString(importedValue));

            int maxLength = Math.Max(standardValue.Length, importedValue.Length);

            // Normaliser scoren til en procentværdi
            return (1 - (double)distance / maxLength) * 100;
        }


        // DamerauLevenshtein Distance algorithm
        public static int DamerauLevenshteinDistance(string source, string target)
        {
            int n = source.Length;
            int m = target.Length;

            if (n == 0) return m;
            if (m == 0) return n;

            // Matrix til distanceberegning
            int[,] distance = new int[n + 1, m + 1];

            // Initialisering
            for (int i = 0; i <= n; i++) distance[i, 0] = i;
            for (int j = 0; j <= m; j++) distance[0, j] = j;

            // Beregning af Damerau-Levenshtein-afstand
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = source[i - 1] == target[j - 1] ? 0 : 1;

                    distance[i, j] = Math.Min(
                        Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                        distance[i - 1, j - 1] + cost
                    );

                    // Damerau-transposition: bytte af to nærliggende tegn
                    if (i > 1 && j > 1 &&
                        source[i - 1] == target[j - 2] &&
                        source[i - 2] == target[j - 1])
                    {
                        distance[i, j] = Math.Min(
                            distance[i, j],
                            distance[i - 2, j - 2] + cost
                        );
                    }
                }
            }

            return distance[n, m];
        }
        public static string NormalizeString(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            return input.Trim().ToLowerInvariant();
        }
        //public List<Address> GetSampleDatabaseAddresses()
        //{
        //    return new List<Address>
        //    {
        //        new Address
        //        {
        //            AddressID = 1,
        //            Street = "Østerbrogade",
        //            Number = "12",
        //            FloorNumber = "2. tv.",
        //            Zipcode = "2100",
        //            Country = "Danmark",
        //            IsStandardized = false // This will be ignored if you choose to exclude it
        //        },
        //        new Address
        //    {
        //        AddressID = 2,
        //        Street = "Vesterbrogade",
        //        Number = "56",
        //        FloorNumber = "1. th.",
        //        Zipcode = "1620",
        //        Country = "Danmark",
        //        IsStandardized = false // This will be ignored
        //    },
        //    new Address
        //    {
        //        AddressID = 3,
        //        Street = "Amagerbrogade",
        //        Number = "101",
        //        FloorNumber = "",
        //        Zipcode = "2300",
        //        Country = "Danmark",
        //        IsStandardized = false // This will be ignored
        //    },
        //    new Address
        //    {
        //        AddressID = 4,
        //        Street = "Sundholmsvej",
        //        Number = "2",
        //        FloorNumber = "3. tv.",
        //        Zipcode = "2300",
        //        Country = "Danmark",
        //        IsStandardized = false // This will be ignored
        //    },
        //    new Address
        //    {
        //        AddressID = 5,
        //        Street = "Nørrebrogade",
        //        Number = "45",
        //        FloorNumber = "4. th.",
        //        Zipcode = "2200",
        //        Country = "Danmark",
        //        IsStandardized = false // This will be ignored
        //    },
        //    new Address
        //    {
        //        AddressID = 6,
        //        Street = "H.C. Andersens Boulevard",
        //        Number = "27",
        //        FloorNumber = "2. tv.",
        //        Zipcode = "1553",
        //        Country = "Danmark",
        //        IsStandardized = false // This will be ignored
        //    },
        //    new Address
        //    {
        //        AddressID = 7,
        //        Street = "Teglholmsgade",
        //        Number = "23",
        //        FloorNumber = "",
        //        Zipcode = "2450",
        //        Country = "Danmark",
        //        IsStandardized = false // This will be ignored
        //    },
        //    new Address
        //    {
        //        AddressID = 8,
        //        Street = "Ballerup Boulevard",
        //        Number = "43",
        //        FloorNumber = "",
        //        Zipcode = "2750",
        //        Country = "Danmark",
        //        IsStandardized = false // This will be ignored
        //    },
        //    new Address
        //    {
        //        AddressID = 9,
        //        Street = "Møllebakken",
        //        Number = "8",
        //        FloorNumber = "1. tv.",
        //        Zipcode = "4000",
        //        Country = "Danmark",
        //        IsStandardized = false // This will be ignored
        //    },
        //    new Address
        //    {
        //        AddressID = 10,
        //        Street = "Højbro Plads",
        //        Number = "10",
        //        FloorNumber = "5. tv.",
        //        Zipcode = "1200",
        //        Country = "Danmark",
        //        IsStandardized = false // This will be ignored
        //    }
        //};
        //}


    }

}
