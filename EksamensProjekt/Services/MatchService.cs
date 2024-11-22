using EksamensProjekt.Models;
using EksamensProjekt.Models.Repositories;

namespace EksamensProjekt.Services;

public class MatchService
{

    // Use only our address model? If so:
    // Implement bool IsStandardized
    // retrieve excel addresses from ExelImportService - set IsStandardized = false on the addresses we get in, in the importer
    // retrieve standardized addresses from DB - set IsStandardized = true
    // logic for manipulating and matching the two (leverpostej algorithm) :P
    // Assign match type (Perfect match etc.)
    // if perfect, create new tenancy, somewhere (which class?), with the standardized address
    // if not perfect/good, user manually look them over (choose a standardized address ?) and system then creates the tenancy?
    // what happens when tenancy is created, does it need to be marked for the user, so they can easily identify newly added tenancies?

    // ... - that is the question(s) - Shakespeare


    // Fandt også en Nuget der kan finde best matches på en string. Men vi kan godt prøve vores egen algo i stedet. 
    // https://github.com/JakeBayer/FuzzySharp





    // Nedenstående er bare en hurtig ChatGPT løsning ud fra hvad jeg skrev ovenfor --- kun til inspiration, ikke korrekt løsning: 


    private readonly IRepo<Address> _standardAddressRepo;
    private readonly ExcelImportService _excelImportService;

    public MatchService(IRepo<Address> addressRepo, ExcelImportService excelImportService)
    {
        _standardAddressRepo = addressRepo;
        _excelImportService = excelImportService;
    }

    public void MatchAddresses(string filePath)
    {
        // Retrieve Excel addresses
        var excelAddresses = _excelImportService.ImportAddresses(filePath);

        // Retrieve standardized addresses from the database
        var standardizedAddresses = _standardAddressRepo.ReadAll().Where(a => a.IsStandardized).ToList();

        foreach (var excelAddress in excelAddresses)
        {
            var matchedAddress = FindBestMatch(excelAddress, standardizedAddresses);

            if (matchedAddress != null && IsPerfectMatch(excelAddress, matchedAddress))
            {
                // Perfect match found, create new tenancy
                CreateTenancyWithStandardizedAddress(matchedAddress);
            }
            else
            {
                // Not a perfect match, requires user review
                // Trigger user interface interaction to review addresses
            }
        }
    }

    private Address? FindBestMatch(Address importedAddress, List<Address> standardizedAddresses)
    {
        // Placeholder for matching logic
        return standardizedAddresses.FirstOrDefault(s => s.Zipcode == importedAddress.Zipcode);
    }

    private bool IsPerfectMatch(Address importedAddress, Address standardizedAddress)
    {
        // Placeholder for perfect match logic (consider exact match criteria)
        return importedAddress.Street == standardizedAddress.Street &&
               importedAddress.Number == standardizedAddress.Number &&
               importedAddress.Zipcode == standardizedAddress.Zipcode;
    }

    private void CreateTenancyWithStandardizedAddress(Address standardizedAddress)
    {
        // Placeholder for creating a new tenancy
        // Tenancy creation can be handled via a separate service, e.g., TenancyService
    }
}



