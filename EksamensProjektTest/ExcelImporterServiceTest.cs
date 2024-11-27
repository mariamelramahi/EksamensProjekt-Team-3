using EksamensProjekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EksamensProjekt.Models;
using Moq;

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
        public void UploadAddresses_ValidFiles_ReturnsAddressList() 
        {
            //ARRANGE

            //ACT
            List<Address> addresses = excelImporterService.ImportAddresses();
            //ASSERT
        }
    }
}
