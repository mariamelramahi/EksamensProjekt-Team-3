using EksamensProjekt.Models;

public class Company : Party
{
    public int CompanyID { get; set; }
    public string CompanyName { get; set; }


    // Parameterless constructor for creating a Tenant with default values.
    public Company() : base(string.Empty, string.Empty, "Company")
    {
        CompanyName = string.Empty;
    }


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
