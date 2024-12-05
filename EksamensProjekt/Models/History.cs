using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksamensProjekt.Models
{
    
        public class History
        {
            public int ChangeID { get; set; }
            public DateTime ChangeDate { get; set; }
            public string ChangeOperation { get; set; }
            public string FieldChanged { get; set; }
            public string OldValue { get; set; }
            public string NewValue { get; set; }
            public int TenancyID { get; set; }
            public int UserID { get; set; }
            public int OrganizationID { get; set; }
            public int AddressID { get; set; }
            public string FullAddress { get; set; }
            public string TenantName { get; set; }
        }
    


}
