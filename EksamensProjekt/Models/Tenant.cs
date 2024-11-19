using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksamensProjekt.Models
{
    public class Tenant
    {
        public int TenantID { get; set; }
        public int PartyID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }
        public string PartyRole { get; set;}

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
