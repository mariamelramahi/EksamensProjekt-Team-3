namespace EksamensProjekt.Models;

public class Organization : Party
{
    public int OrganizationID { get; set; }
    public string OrganizationName { get; set; }


    // Parameterless constructor for object initialization
    public Organization() : base(string.Empty, string.Empty, "Organization")
    {
    }


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