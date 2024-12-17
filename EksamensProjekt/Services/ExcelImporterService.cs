using EksamensProjekt.Models;
using ExcelDataReader;
using System.IO;

namespace EksamensProjekt.Services;

public class ExcelImportService
{
    //public string FilePath { get; set; }

    public List<Address> ImportAddresses(string filePath)
    {

        // Register the encoding provider to support older Excel files (.xls)
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        var addresses = new List<Address>();
        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {

                // Skip the header row
                reader.Read();
                try
                {
                    while (reader.Read())
                    {
                        // Columns: Street, Number, FloorNumber, Zipcode, City, Country
                        var address = new Address
                        {
                            // AddressID is auto-incremented in DB, so it's not set here
                            Street = reader.GetValue(0)?.ToString() ?? string.Empty,
                            Number = reader.GetValue(1)?.ToString() ?? string.Empty,
                            FloorNumber = reader.GetValue(2)?.ToString() ?? string.Empty,
                            Zipcode = reader.GetValue(3)?.ToString() ?? string.Empty,
                            City = reader.GetValue(4)?.ToString() ?? string.Empty,
                            Country = reader.GetValue(5)?.ToString() ?? string.Empty
                        };

                        addresses.Add(address);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"The file is missing necessary data");
                }
                
            }
            return addresses;
        }

    }
}

