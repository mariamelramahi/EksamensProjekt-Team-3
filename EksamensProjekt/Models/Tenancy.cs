using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



namespace EksamensProjekt.Models
{
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
        public StandardAddress? Address { get; set; }
        public Company? Company { get; set; }
        public int? OrganizationID { get; set; }

        
    }
    
}

