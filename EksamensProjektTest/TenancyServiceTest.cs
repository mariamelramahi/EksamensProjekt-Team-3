using EksamensProjekt.Models;
using EksamensProjekt.Repos;
using EksamensProjekt.Repositories;
using EksamensProjekt.Services;
using Moq; // Moq library for mocking dependencies

[TestClass] // Marks this class as containing unit tests
public class TenancyServiceTests
{
    // Using Mock for the repositories to simulate repository behaviour
    private Mock<IRepo<Tenancy>> _mockTenancyRepo;
    private Mock<IRepo<Tenant>> _mockTenantRepo;
    private Mock<IRepo<Address>> _mockAddressRepo;
    private Mock<ITenancyTenant> _mockTenancyTenantRepo;
    private TenancyService _service;

    // This method runs before each test and sets up the mocks and service
    [TestInitialize]
    public void TestInitialize()
    {
        // Initialize the mocks
        _mockTenancyRepo = new Mock<IRepo<Tenancy>>();
        _mockTenantRepo = new Mock<IRepo<Tenant>>();
        _mockAddressRepo = new Mock<IRepo<Address>>();
        _mockTenancyTenantRepo = new Mock<ITenancyTenant>();

        // Pass the mocked dependencies to the service constructor
        _service = new TenancyService(
            _mockTenancyRepo.Object,    // Mocked tenancy repository
            _mockTenantRepo.Object,    // Mocked tenant repository
            _mockAddressRepo.Object,    // Mocked address repository
            _mockTenancyTenantRepo.Object
        );
    }

    [TestMethod] // Marks this method as a test
    public void UpdateTenancy_WithNewValues_UpdatesSuccessfully()
    {
        // Arrange: Set up the existing tenancy in the mock repository
        var existingTenancy = new Tenancy
        {
            TenancyID = 1,
            TenancyStatus = TenancyStatus.Occupied, // Existing status
            MoveInDate = new DateTime(2023, 1, 1),         // Existing move-in date
            SquareMeter = 50                                // Existing square meter size
        };

        // Arrange: Create a tenancy object with updated values
        var updatedTenancy = new Tenancy
        {
            TenancyID = 1,                                // Same ID as the existing tenancy
            TenancyStatus = TenancyStatus.Vacant, // New status
            MoveInDate = new DateTime(2024, 1, 1),       // Updated move-in date
            SquareMeter = 60                              // Updated square meter size
        };

        // Mock the repository to return the existing tenancy when GetByID is called
        _mockTenancyRepo.Setup(repo => repo.GetByID(1)).Returns(existingTenancy);

        // Act: Call the service method to update the tenancy
        _service.UpdateTenancy(updatedTenancy);

        // Assert: Verify that the repository's Update method was called with the correct updated tenancy
        _mockTenancyRepo.Verify(repo => repo.Update(It.Is<Tenancy>(t =>
            t.TenancyID == 1 &&                                  // Assert ID matches
            t.TenancyStatus == TenancyStatus.Vacant &&    // Assert status updated
            t.MoveInDate == new DateTime(2024, 1, 1) &&          // Assert move-in date updated
            t.SquareMeter == 60                                  // Assert square meter updated
        )), Times.Once); // Verify the Update method was called exactly once
    }
}
