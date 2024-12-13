using EksamensProjekt.Models;
using EksamensProjekt.Models.Models.DTO;
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
            //Fetching all current tenancies
            var tenancies = tenancyRepo.ReadAll();
           
            foreach (var match in addressMatches)
            {

                // Check if the selected match corresponds to an existing tenancy
                var existingTenancy = tenancies.FirstOrDefault(t =>
                    t.Address.Street == match.SelectedMatch?.PotentialAddressMatch.Street &&
                    t.Address.Number == match.SelectedMatch?.PotentialAddressMatch.Number &&
                    t.Address.Zipcode == match.SelectedMatch?.PotentialAddressMatch.Zipcode &&
                    t.Address.City == match.SelectedMatch?.PotentialAddressMatch.City &&
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
            // List for results
            List<AddressMatchResult> matchResults = new List<AddressMatchResult>();
            //Fetching all standardized addresses
            List<Address> databaseAddresses = addressRepo.ReadAll().ToList();


            // Parallel foreach, for performance when handling large amount of data
            Parallel.ForEach(importedAddresses, importedAddress =>
            {
                // List of potential matches
                List<AddressAndMatchScore> potentialMatches = new List<AddressAndMatchScore>();

                // Find potential matches in database
                foreach (var dbAddress in databaseAddresses)
                {
                    string matchType = CalculateAddressMatchScore(dbAddress, importedAddress);
                    double score = GetMatchScoreValue(matchType);

                    // Add matches with score 
                    if (score > 0)
                    {
                        potentialMatches.Add(new AddressAndMatchScore
                        {
                            PotentialAddressMatch = dbAddress,
                            MatchScore = matchType
                        });
                    }
                }

                // Sort matches by score, highest first (Type A)
                potentialMatches = potentialMatches
                    .OrderByDescending(pm => GetMatchScoreValue(pm.MatchScore))
                    .ToList();

                // Add results to list. Lock ensures no results is compromized during execution.
                lock (matchResults)
                {
                    var automaticMatchScore = FindAutomaticMatchScore(potentialMatches);
                    matchResults.Add(new AddressMatchResult
                    {
                        ImportedAddress = importedAddress,
                        PotentialMatches = potentialMatches,
                        SelectedMatch = automaticMatchScore, 
                        //If user selection is requiered (automaticMatchScore == null),
                        //it sets IsUserSelectionRequiered to true. 
                        //If user selection is not requiered it is because the criteria of being
                        //Type A or B is met, therefore it is set to false.
                        IsUserSelectionRequired = automaticMatchScore == null
                    });
                }
            });
            return matchResults;
        }

        //Method for criteria to determine if user selection requiered are set to true or false
        private AddressAndMatchScore? FindAutomaticMatchScore(List<AddressAndMatchScore> potentialMatches)
        {
            // First result of list is stored as bestmatch
            var bestMatch = potentialMatches.First();
            //No user selection is requiered by these types
            if (bestMatch.MatchScore == "Type A" || bestMatch.MatchScore == "Type B")
            {
                return bestMatch;
            }
            // Assumes match is Type C or D, which means user selection is requiered, boolean will be set to true. 
            else
            {
                return null;
            }
        }
        public static string CalculateAddressMatchScore(Address databaseAddress, Address importedAddress)
        {
            double streetMatchScore = CalculateDamerauLevenshteinMatchScore(databaseAddress.Street, importedAddress.Street) * 0.5;
            double numberCodeMatchScore = CalculateDamerauLevenshteinMatchScore(databaseAddress.Number, importedAddress.Number) * 0.15;
            double floorNumberMatchScore = CalculateDamerauLevenshteinMatchScore(databaseAddress.FloorNumber, importedAddress.FloorNumber) * 0.05;
            double zipCodeMatchScore = CalculateDamerauLevenshteinMatchScore(databaseAddress.Zipcode, importedAddress.Zipcode) * 0.10;
            double cityCodeMatchScore = CalculateDamerauLevenshteinMatchScore(databaseAddress.City, importedAddress.City) * 0.10;
            double countryMatchScore = CalculateDamerauLevenshteinMatchScore(databaseAddress.Country, importedAddress.Country) * 0.10;

            double totalMatchScore = streetMatchScore + zipCodeMatchScore + cityCodeMatchScore + floorNumberMatchScore + numberCodeMatchScore + countryMatchScore;

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
        public static double CalculateDamerauLevenshteinMatchScore(string standardAddress, string importedAddress)
        {
            if (string.IsNullOrEmpty(standardAddress) && string.IsNullOrEmpty(importedAddress))
            {
                return 100.0; // Match, if both values are empty.
            }

            if (string.IsNullOrEmpty(standardAddress) || string.IsNullOrEmpty(importedAddress))
            {
                return 0.0; // No match, if one of the values are empty.
            }

            // Use of Damerau-Levenshtein-distance
            int distance = DamerauLevenshteinDistance(NormalizeString(standardAddress), NormalizeString(importedAddress));

            //Finds the longest string. This is the value that is going to be used to 
            //normalize into percent
            int maxLength = Math.Max(standardAddress.Length, importedAddress.Length);

            // Normalize score to percent
            return (1 - (double)distance / maxLength) * 100;
        }

        // DamerauLevenshtein Distance algorithm  
        public static int DamerauLevenshteinDistance(string source, string target)
        {
            int n = source.Length;
            int m = target.Length;

            //If source or target euqals 0, return the length of the other string,
            //because all characters must be added or removed.  
            if (n == 0) return m;
            if (m == 0) return n;

            // Matrix for distancecalculation
            int[,] distance = new int[n + 1, m + 1];

            // Initializing
            for (int i = 0; i <= n; i++) distance[i, 0] = i;
            for (int j = 0; j <= m; j++) distance[0, j] = j;

            // Calculation Damerau-Levenshtein distance. For each position cost is 0
            // if the characters are the same, source [i-1] and target [j-1]. 
            // Otherwise cost is 1. 
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = source[i - 1] == target[j - 1] ? 0 : 1;

                    distance[i, j] = Math.Min(
                        Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                        distance[i - 1, j - 1] + cost
                    );

                    // Damerau-transposition: exchange of two characters beside each other
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
            //returns empty string if string is null or empty
            if (string.IsNullOrEmpty(input)) return string.Empty;
            //trim removes spaces before or after string, also ensures case-insensitivity
            //by converting all characters to lower
            return input.Trim().ToLowerInvariant();
        }
    }

}
