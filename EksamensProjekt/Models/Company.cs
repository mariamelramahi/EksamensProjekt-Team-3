using EksamensProjekt.Models;

public class Company : Party
{

    public string CompanyName { get; set; }


    // Constructor to initialize the Company object, including properties inherited from Party.
    public Company(string companyName, string phoneNum, string email)
        : base(phoneNum, email, "Company")  // Calls the base class constructor to initialize common properties, with PartyRole set to 'Company'.
    {
        CompanyName = companyName;
    }


    
    public override string ToString()
    {
        return CompanyName;
    }
}
