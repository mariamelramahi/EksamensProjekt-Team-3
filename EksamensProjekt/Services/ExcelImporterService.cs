using EksamensProjekt.Models;
using ExcelDataReader;
using System.IO;

namespace EksamensProjekt.Services;

public class ExcelImportService
{
    public List<Address> ImportAddresses(string filePath)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        var addresses = new List<Address>();

        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                // Read the header row
                if (!reader.Read())
                    throw new Exception("The file contains no data.");

                // Map columns dynamically using the helper method
                var columnMap = MapColumns(reader);

                // Read the data rows and map to Address objects
                while (reader.Read())
                {
                    var address = new Address
                    {
                        Street = reader.GetValue(columnMap["Street"])?.ToString() ?? string.Empty,
                        Number = reader.GetValue(columnMap["Number"])?.ToString() ?? string.Empty,
                        FloorNumber = reader.GetValue(columnMap["FloorNumber"])?.ToString() ?? string.Empty,
                        Zipcode = reader.GetValue(columnMap["Zipcode"])?.ToString() ?? string.Empty,
                        City = reader.GetValue(columnMap["City"])?.ToString() ?? string.Empty,
                        Country = reader.GetValue(columnMap["Country"])?.ToString() ?? string.Empty
                    };

                    addresses.Add(address);
                }
            }

            return addresses;
        }
    }

    private Dictionary<string, int> MapColumns(IExcelDataReader reader)
    {
        // Define expected columns and allowed variations
        var expectedColumns = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
    {
        { "Street", new List<string> { "Street", "Street Name", "Vejnavn", "vej", "gade"} },
        { "Number", new List<string> { "Number", "House Number", "Husnummer" } },
        { "FloorNumber", new List<string> { "FloorNumber", "Floor", "Sal", "etage", "etagenummmer"} },
        { "Zipcode", new List<string> { "Zipcode", "Postcode", "Postnummer", "Postkode" } },
        { "City", new List<string> { "City", "By" } },
        { "Country", new List<string> { "Country", "Land" } }
    };

        var columnMap = new Dictionary<string, int>();

        for (int i = 0; i < reader.FieldCount; i++)
        {
            var header = reader.GetValue(i)?.ToString()?.Trim();
            if (header == null) continue;

            foreach (var expected in expectedColumns)
            {
                if (expected.Value.Any(variation => header.Equals(variation, StringComparison.OrdinalIgnoreCase)))
                {
                    columnMap[expected.Key] = i;
                    break;
                }
            }
        }

        // Ensure all required columns are present
        foreach (var column in expectedColumns.Keys)
        {
            if (!columnMap.ContainsKey(column))
                throw new FormatException($"Missing required column: {column}.");
        }

        return columnMap;
    }


}

