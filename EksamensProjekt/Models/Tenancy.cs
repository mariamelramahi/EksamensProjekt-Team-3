namespace EksamensProjekt.Models;

public enum TenancyStatus
{
    Occupied,
    Vacant,
    UnderRenovation
}

public class Tenancy
{
    public int TenancyID { get; set; }
    public TenancyStatus? TenancyStatus { get; set; }
    public DateTime? MoveInDate { get; set; }
    public DateTime? MoveOutDate { get; set; }
    public int? SquareMeter { get; set; }
    public int? Rent { get; set; }
    public int? Rooms { get; set; }
    public int? Bathrooms { get; set; }
    public bool? PetsAllowed { get; set; }
    public List<Tenant>? Tenants { get; set; }
    public Address? Address { get; set; }
    public Company? Company { get; set; }
    public Organization Organization { get; set; }
    public bool IsDeleted { get; set; }


    // Make constructor - fix Org ID to Organization organization

    // look at new object creation in service in relation to constructors

    // Look at refactoring repos to only have Irepo methods





}

