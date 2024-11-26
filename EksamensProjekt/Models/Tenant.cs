using EksamensProjekt.Models;

public class Tenant : Party
{
    public int TenantID { get; set; }
    public string FirstName { get; set; } 
    public string LastName { get; set; }


    // Parameterless constructor for creating a Tenant with default values.
    public Tenant() : base(string.Empty, string.Empty, "Tenant")
    {
        FirstName = string.Empty;
        LastName = string.Empty;
    }

    // Constructor to initialize the Tenant object, including properties inherited from Party.
    public Tenant(string firstName, string lastName, string phoneNum, string email)
        : base(phoneNum, email, "Tenant")  // Calls the base class constructor to initialize common properties.
    {
        FirstName = firstName;
        LastName = lastName;
    }


    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }

}
