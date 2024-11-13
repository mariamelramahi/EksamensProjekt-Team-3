using EksamensProjekt.Model;
using EksamensProjekt.Services;
using System.Collections.ObjectModel;

namespace EksamensProjekt.Tests
{
    [TestClass]
    public class FilterServiceTests
    {
        private ObservableCollection<Tenancy> _tenancies;
        private FilterService _filterService;


        [TestInitialize]
        public void SetupForTest()
        {
            // Sample Tenancy Data
            _tenancies = new ObservableCollection<Tenancy>
            {
                new Tenancy(Tenancy.Status.Occupied, new DateTime(2023, 6, 1), null, "100m2", 15000, 3, 1, true, new List<Tenant>(),
                    new Address { StreetName = "Main St", Number = "10", ZipCode = "12345", City = "Copenhagen", Country = "Denmark" }, new Company()),

                new Tenancy(Tenancy.Status.Vacant, new DateTime(2024, 1, 15), null, "85m2", 12000, 2, 1, false, new List<Tenant>(),
                    new Address { StreetName = "Elm St", Number = "5", ZipCode = "54321", City = "Aarhus", Country = "Denmark" }, new Company()),

                new Tenancy(Tenancy.Status.UnderRenovation, null, new DateTime(2025, 5, 20), "200m2", 18000, 5, 2, true, new List<Tenant>(),
                    new Address { StreetName = "Oak St", Number = "7B", ZipCode = "67890", City = "Odense", Country = "Denmark" }, new Company())
            };

            // Initialize the FilterService with the sample data
            _filterService = new FilterService(_tenancies);

        }

        [TestMethod]
        public void ApplyTenancyFilters_FilterByZipCode_ReturnsCorrectResults()
        {
            // Act: Apply filter by Zip Code
            _filterService.ApplyFilters("12345", null, null);

            // Assert: Only one tenancy should match the zip code "12345"
            var results = _filterService.TenancyCollectionView.Cast<Tenancy>().ToList();//Cast<> used to convert the item in iCollectionView to a tenancy object
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("12345", results.First().StandardAddress.ZipCode);//Since asserted one tenancy in the list, the first one should match filter
        }

        [TestMethod]
        public void ApplyTenancyFilters_FilterByStreetName_ReturnsCorrectResults()
        {
            // Act: Apply filter by Street Name
            _filterService.ApplyFilters(null, "Elm St", null);

            // Assert: Only one tenancy should match the street name "Elm St"
            var results = _filterService.TenancyCollectionView.Cast<Tenancy>().ToList();
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Elm St", results.First().StandardAddress.StreetName);
        }

        [TestMethod]
        public void ApplyTenancyFilters_FilterByStatus_ReturnsCorrectResults()
        {
            // Act: Apply filter by Status
            _filterService.ApplyFilters(null, null, "Vacant");

            // Assert: Only one tenancy should have the status "Vacant"
            var results = _filterService.TenancyCollectionView.Cast<Tenancy>().ToList();
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(Tenancy.Status.Vacant, results.First().TenancyStatus);
        }

        [TestMethod]
        public void ApplyTenancyFilters_FilterByMultipleCriteria_ReturnsCorrectResults()
        {
            // Act: Apply multiple filters (Street and Status)
            _filterService.ApplyFilters(null, "Oak St", "UnderRenovation");

            // Assert: Only one tenancy should match both criteria
            var results = _filterService.TenancyCollectionView.Cast<Tenancy>().ToList();
            Assert.AreEqual(1, results.Count);
            var result = results.First();
            Assert.AreEqual("Oak St", result.StandardAddress.StreetName);
            Assert.AreEqual(Tenancy.Status.UnderRenovation, result.TenancyStatus);
        }
    }
}
