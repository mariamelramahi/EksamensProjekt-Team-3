using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace EksamensProjekt.Models
{
    public enum TenancyStatus
    {
    Active,
    Inactive,
    Pending
    }
   public class Tenancy
    {
        public int TenancyID { get; set; }
        public TenancyStatus? TenancyStatus { get; set; }
        public DateTime? MoveInDate { get; set; }
        public DateTime? MoveOutDate { get; set; }
        public string? SquareMeter { get; set; }
        public int? Rent { get; set; }
        public int? Rooms { get; set; }
        public int? Bathrooms { get; set; }
        public bool? PetsAllowed { get; set; }
        public List<Tenant>? tenants { get; set; }
        public StandardAddress? address { get; set; }
        public Company? Company { get; set; }
    
    }
}

