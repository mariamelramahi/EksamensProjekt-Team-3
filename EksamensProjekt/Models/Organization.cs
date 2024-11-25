namespace EksamensProjekt.Models;

public class Organization : Party
{

    public string OrganizationName { get; set; }


    // Constructor to initialize the Organization object, including properties inherited from Party.
    public Organization(string organizationName, string phoneNum, string email)
        : base(phoneNum, email, "Organization")  // Calls the base class constructor to initialize common properties, with PartyRole set to 'Organization'.
    {
        OrganizationName = organizationName;
    }


    
    public override string ToString()
    {
        return OrganizationName;
    }
}