using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EksamensProjekt.Model.Tenancy;

namespace EksamensProjekt.Model
{
    public class ModifiedExcelAddress : Address
    {
        public int AddressID { get; set; }
        public string StreetName { get; set; }
        public string Number { get; set; }
        public string Floor { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public Status? TenancyStatus { get; set; }
        public DateTime? MoveInDate { get; set; }
        public DateTime? MoveOutDate { get; set; }
        public string? SquareMeter { get; set; }
        public int? Rent { get; set; }
        public int? Rooms { get; set; }
        public int? BathRooms { get; set; }
        public bool? PetsAllowed { get; set; }
        public List<Tenant>? Tenants { get; set; }
        public Address? StandardAddress { get; set; }
        public Company? Company { get; set; }
        public ModifiedExcelAddress() { }
    }
}
    