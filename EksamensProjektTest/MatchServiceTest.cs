using EksamensProjekt.Models.Models.DTO;
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
        // Initialize the mock repositories
        tenancyRepo = new Mock<IRepo<Tenancy>>();
        addressRepo = new Mock<IRepo<Address>>();

        // Inject mocks into the service
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
        Assert.AreEqual(2, matchResults.Count);

        var expectedResults = new List<(string street, string matchScore, bool isUserSelectionRequired)>
        {
            //Type A match, så IsUserSelectionRequiered should be false
            ("Høvejen", "Type A", false),
            //No selectedMatch, so IsUserSelectionRequiered should be true
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
                Assert.IsNotNull(result.SelectedMatch, $"Expected a SelectedMatch for {expected.street}, but none was found.");
                Assert.AreEqual(expected.matchScore, result.SelectedMatch.MatchScore, $"Match score for {expected.street} does not match.");
            }
            else
            {
                Assert.IsNull(result.SelectedMatch, $"Expected no SelectedMatch for {expected.street}, but one was found.");
            }
        }
    }
    [TestMethod]
    public void ApproveMatches_ValidMatches_CreatesAndUpdatesTenancies()
    {
        // ARRANGE
        var databaseAddresses = new List<Address>
        {
            new Address { Street = "Høvejen", Number = "12", Zipcode = "9000", Country = "Danmark", FloorNumber = "1" }
        };

        var importedAddresses = new List<Address>
        {
            new Address { Street = "Høvejen", Number = "12", Zipcode = "9000", Country = "Danmark", FloorNumber = "1" },
            new Address { Street = "Fasta vej", Number = "46", Zipcode = "9000", Country = "Danmark", FloorNumber = "2" }
        };

        var matchResults = new List<AddressMatchResult>
        {
            new AddressMatchResult
            {
                ImportedAddress = importedAddresses[0],
                PotentialMatches = new List<AddressAndMatchScore>
                {
                    new AddressAndMatchScore
                    {
                        PotentialAddressMatch = databaseAddresses[0],
                        MatchScore = "Type A"
                    }
                },
                SelectedMatch = new AddressAndMatchScore
                {
                    PotentialAddressMatch = databaseAddresses[0],
                    MatchScore = "Type A"
                },
                IsUserSelectionRequired = false
            },
            new AddressMatchResult
            {
                ImportedAddress = importedAddresses[1],
                PotentialMatches = new List<AddressAndMatchScore>
                {
                    new AddressAndMatchScore
                    {
                        PotentialAddressMatch = databaseAddresses[0],
                        MatchScore = "Type C"
                    }
                },
                SelectedMatch = new AddressAndMatchScore // Set SelectedMatch 
                {
                    PotentialAddressMatch = new Address { Street = "Fasta vej", Number = "46", Zipcode = "9000", Country = "Danmark" },
                    MatchScore = "Type C"
                },
                IsUserSelectionRequired = true
            }
        };

        // Mock the tenancyRepo to return a list of existing tenancies
        tenancyRepo.Setup(repo => repo.ReadAll()).Returns(new List<Tenancy>
        {
            new Tenancy
            {
                Address = new Address { Street = "Høvejen", Number = "12", Zipcode = "9000", Country = "Danmark", FloorNumber = "1" }
            }
        });

        // ACT
        matchService.ApproveMatches(matchResults);

        // ASSERT
        // Ensure the tenancyRepo Update and Create methods are called correctly.

        // The first address (Høvejen) should result in an Update
        tenancyRepo.Verify(repo => repo.Update(It.Is<Tenancy>(t =>
            t.Address.Street == "Høvejen" &&
            t.Address.Number == "12" &&
            t.Address.Zipcode == "9000")), Times.Once);

        // The second address (Fasta vej) should result in a Create
        tenancyRepo.Verify(repo => repo.Create(It.Is<Tenancy>(t =>
            t.Address.Street == "Fasta vej" &&
            t.Address.Number == "46" &&
            t.Address.Zipcode == "9000")), Times.Once);
    }
}
