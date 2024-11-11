namespace EksamensProjekt.Model
{
    public class Tenancy
    {
        public int TenancyID { get; set; }
        public Status TenancyStatus { get; set; }
        public DateTime MoveInDate { get; set; }
        public DateTime MoveOutDate { get; set; }
        public string SquareMeter { get; set; }
        public int Rent { get; set; }
        public int Rooms { get; set; }
        public int BathRooms { get; set; }
        public bool PetsAllowed { get; set; }
        public List<Tenant> Tenants { get; set; }
        public Address StandardAddress { get; set; }
        public Company Company { get; set; }

        public Tenancy(Status tenancyStatus, DateTime moveInDate, DateTime moveOutDate, string squareMeter, int rent, int rooms, int bathRooms, bool petsAllowed, List<Tenant> tenants, Address standardAddress, Company company)
        {
            TenancyStatus = tenancyStatus;
            MoveInDate = moveInDate;
            MoveOutDate = moveOutDate;
            SquareMeter = squareMeter;
            Rent = rent;
            Rooms = rooms;
            BathRooms = bathRooms;
            PetsAllowed = petsAllowed;
            Tenants = tenants ?? new List<Tenant>();
            StandardAddress = standardAddress;
            Company = company;
        }
        public Tenancy() { }
        public enum Status
        {
            Occupied,
            Vacant,
            UnderRenovation
        }
    }
}
