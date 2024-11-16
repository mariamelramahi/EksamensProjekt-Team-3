using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksamensProjekt.Models
{
    public class StandardAddress
    {
        public int StandardAddressID { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string FloorNumber { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }

        public override string ToString()
        {
            return Street + " " + Number + ", " + FloorNumber + ", " + Zipcode + ", " + Country;
        }
    }
}
