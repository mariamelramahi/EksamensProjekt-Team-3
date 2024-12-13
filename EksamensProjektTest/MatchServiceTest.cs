using EksamensProjekt.Models;
using EksamensProjekt.Repos;
using EksamensProjekt.Services;
using Moq;

[TestClass]
public class MatchServiceTest
{
    private Mock<IRepo<Address>> addressRepo;
    private Mock<IRepo<Tenancy>> tenancyRepo;
    private MatchService matchService;

    [TestInitialize]
    public void TestInitialize()
    {
        // Initialize the mock repository
        tenancyRepo = new Mock<IRepo<Tenancy>>();
        addressRepo = new Mock<IRepo<Address>>();


        // Inject the mock into the service
        matchService = new MatchService(tenancyRepo.Object, addressRepo.Object);
    }

    [TestMethod]
    public void ComparingImportedAddress_WithDatabase_ReturnsMatch()
    {
        // ARRANGE
        var databaseAddresses = new List<Address>
        {
            new Address { Street = "Høvejen", Number = "12", Zipcode = "9000", City = "Aalborg", Country = "Danmark", FloorNumber = "1" },
            new Address { Street = "Kastanjevej", Number = "456", Zipcode = "9000", Country = "Danmark", FloorNumber = "2" }
        };

        var importedAddresses = new List<Address>
        {
            new Address { Street = "Høvejen", Number = "12", Zipcode = "9000", City = "Aalborg", Country = "Danmark", FloorNumber = "1" },
            new Address { Street = "Fasta vej", Number = "46", Zipcode = "8000", Country = "Danmark", FloorNumber = "2" }
        };

        // Mock the repository to return the database addresses
        addressRepo.Setup(repo => repo.ReadAll()).Returns(databaseAddresses);

        // ACT
        var matchResults = matchService.CompareImportedAddressesWithDatabase(importedAddresses);

        // ASSERT
        //There should be 2 results
        Assert.AreEqual(2, matchResults.Count);

        // Assert that both results are present without assuming order
        var expectedResults = new List<(string street, string matchScore, bool isUserSelectionRequired)>
        {
            // Type A match, so it should have a SelectedMatch
            ("Høvejen", "Type A", false), 
             // Type C match, no SelectedMatch, requires user selection
            ("Kastanjevej", null, true)  
        };

        foreach (var expected in expectedResults)
        {
            var result = matchResults.FirstOrDefault(r =>
                r.PotentialMatches.Any(pm => pm.PotentialAddressMatch.Street == expected.street) &&
                r.IsUserSelectionRequired == expected.isUserSelectionRequired);

            Assert.IsNotNull(result, $"Expected match for {expected.street} was not found.");

            if (expected.matchScore != null)
            {
                // If a match score is expected, verify the SelectedMatch
                Assert.IsNotNull(result.SelectedMatch, $"Expected a SelectedMatch for {expected.street}, but none was found.");
                Assert.AreEqual(expected.matchScore, result.SelectedMatch.MatchScore, $"Match score for {expected.street} does not match.");
            }
            else
            {
                // If no match score is expected, ensure SelectedMatch is null
                Assert.IsNull(result.SelectedMatch, $"Expected no SelectedMatch for {expected.street}, but one was found.");
            }
        }
    }



}
