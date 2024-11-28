namespace EksamensProjekt.Models;

public class Address
{
    public int AddressID { get; set; }
    public string? Street { get; set; }
    public string? Number { get; set; }
    public string? FloorNumber { get; set; }
    public string? Zipcode { get; set; }
    public string? Country { get; set; }
    public bool IsStandardized { get; set; }

    public override string ToString()
    {
        return Street + " " + Number + ", " + FloorNumber + ", " + Zipcode + ", " + Country;
    }

    // Override Equals, because two objects with the same data can be percieved as different
    // if they do not have the same reference equality.
    // This override ensures addresses are equal based on values. 
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (Address)obj;

        return string.Equals(Street, other.Street, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Number, other.Number, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(FloorNumber, other.FloorNumber, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Zipcode, other.Zipcode, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Country, other.Country, StringComparison.OrdinalIgnoreCase);
    }

    // Override GetHashCode
    //Good practice to override getHasCode with equals. The one important property of hashcode to 
    //remember is that two equal objects must contain the same hascodes. 
    public override int GetHashCode()
    {
        return HashCode.Combine(
            Street?.ToLowerInvariant(),
            Number?.ToLowerInvariant(),
            FloorNumber?.ToLowerInvariant(),
            Zipcode?.ToLowerInvariant(),
            Country?.ToLowerInvariant()
        );
    }
}
