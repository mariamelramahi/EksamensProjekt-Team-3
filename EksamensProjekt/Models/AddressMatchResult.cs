namespace EksamensProjekt.Models
{
    public class AddressMatchResult
    {
        public Address ImportedAddress {  get; set; }
        public List<AddressAndMatchScore> PotentialMatches { get; set; }
        public AddressAndMatchScore? SelectedMatch { get; set; }
        public bool IsUserSelectionRequired { get; set; } // Flag to indicate if user must select the match

    }
}
