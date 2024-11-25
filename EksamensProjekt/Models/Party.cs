namespace EksamensProjekt.Models;

public abstract class Party
{

    public int PartyID { get; set; } 
    public string PhoneNum { get; set; } 
    public string Email { get; set; }  
    public string PartyRole { get; set; }  // Role of the party (Tenant, Company, Organization).


    // Constructor to initialize the Party object with essential details. ('Protected' makes sure these are set by the derived classes)
    protected Party(string phoneNum, string email, string partyRole)
    {
        PhoneNum = phoneNum;
        Email = email;
        PartyRole = partyRole;
    }

}
