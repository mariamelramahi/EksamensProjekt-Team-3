//using EksamensProjekt.Model;
//using EksamensProjekt.Model.Repositories;
//using EksamensProjekt.Services;
//using Moq;
//namespace EksamensProjekt.Tests
//{
//    [TestClass]
//    public class TenancyServiceTests
//    {
//        //mock repo using moq NuGet package
//        private Mock<IRepo<Tenancy>> _mockTenancyRepo;
//        private Mock<IRepo<Tenant>> _mockTenantRepo;
//        private Mock<IRepo<StandardAddress>> _mockAddressRepo;
//        private TenancyService _service;

//        [TestInitialize]
//        public void SetupForTest()
//        {
//            // Initialize mocks
//            _mockTenancyRepo = new Mock<IRepo<Tenancy>>();
//            _mockTenantRepo = new Mock<IRepo<Tenant>>();
//            _mockAddressRepo = new Mock<IRepo<StandardAddress>>();

//            // Initialize the service with the mocked repositories via constructor
//            _service = new TenancyService(
//                _mockTenancyRepo.Object,
//                _mockTenantRepo.Object,
//                _mockAddressRepo.Object
//            );
//        }

//        [TestMethod]
//        public void CreateNewTenancy_ShouldAddTenancy()
//        {
//            // Arrange
//            var tenants = new List<Tenant>
//            {
//                new Tenant("John", "Doe", "1234567890", "john.doe@email.com")
//            };

//            var address = new Address
//            {
//                StreetName = "Oak St",
//                ZipCode = "12345",
//                City = "City",
//                Country = "Country"
//            };

//            var company = new Company(); // Assuming Company is a valid class

//            // Act
//            _service.CreateNewTenancy(
//                Tenancy.Status.Occupied,
//                DateTime.Now,
//                DateTime.Now.AddMonths(12),
//                "100",
//                1200,
//                3,
//                2,
//                true,
//                tenants,
//                address,
//                company
//            );

//            // Assert: Verify that the Add method of tenancyRepo was called once
//            _mockTenancyRepo.Verify(repo => repo.Add(It.IsAny<Tenancy>()), Times.Once);
//        }
//    }
//}
