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

    }
}
