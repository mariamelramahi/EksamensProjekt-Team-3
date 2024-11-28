namespace EksamensProjekt.Models
{
    public class AddressMatchResult
    {
        public Address ImportedAddress {  get; set; }
        public List<AddressAndMatchScore> PotentialMatches { get; set; }

    }
}
