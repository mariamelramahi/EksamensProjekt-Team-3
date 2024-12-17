using EksamensProjekt.Services;
using EksamensProjekt.Models;

namespace EksamensProjektTest
{

    [TestClass]
    public class ExcelImporterServiceTest
    {
        ExcelImportService excelImporterService;

        
        [TestInitialize]
        public void TestInitialize() 
        {
            excelImporterService = new ExcelImportService();
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        [DeploymentItem("Resources/TestAddressesMissingColumns.xlsx", "Resources")]
        public void UploadAddresses_MissingColumns_ThrowsFormatException()
        {
            // Arrange: Fil med færre end forventede kolonner
            var filePath = Path.Combine("Resources", "TestAddressesMissingColumns.xlsx");

            // Act
            excelImporterService.ImportAddresses(filePath);

            // Assert: Håndteres af [ExpectedException]
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        [DeploymentItem("Resources/TestAddressesExtraColumns.xlsx", "Resources")]
        public void UploadAddresses_ExtraColumns_ThrowsFormatException()
        {
            // Arrange: Fil med flere end forventede kolonner
            var filePath = Path.Combine("Resources", "TestAddressesExtraColumns.xlsx");

            // Act
            excelImporterService.ImportAddresses(filePath);

            // Assert: Håndteres af [ExpectedException]
        }



        [TestMethod]
        [DeploymentItem("Resources/TestAddresses.xlsx", "Resources")]

        public void UploadAddresses_ValidFiles_ReturnsAddressList() 
        {
            //ARRANGE
            var filePath = Path.Combine("Resources", "TestAddresses.xlsx");
            
            //ACT
            List<Address> addresses = excelImporterService.ImportAddresses(filePath);
            
            //ASSERT
            Assert.AreEqual(addresses.Count, 3);
            //testing if address objects are equal using equals override
            Assert.AreEqual(addresses[0], CreateAddress("Main Street", "123", "1A", "1000", "København", "Denmark"));
            Assert.AreEqual(addresses[1], CreateAddress("Second Street", "456", "2B", "2000", "Malmö", "Sweden"));
            Assert.AreEqual(addresses[2], CreateAddress("Third Avenue", "789", "3C", "3000", "Oslo", "Norway"));
        }
        private Address CreateAddress(string street, string number, string floor, string zipcode, string city, string country)
        {
            return new Address()
            {
                Street = street,
                Number = number,
                FloorNumber = floor,
                Zipcode = zipcode,
                City = city,
                Country = country
            };
        }
        
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void UploadAddresses_FileNotFound_ThrowsException()
        {
            var filePath = Path.Combine("Resources", "NonExistentFile.xlsx");
            List<Address> addresses = excelImporterService.ImportAddresses(filePath);
        }

    }
}
