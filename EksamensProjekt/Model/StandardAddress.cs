using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksamensProjekt.Model
{
    public class StandardAddress : Address
    {
        public int AddressID {  get; set; }
        public string StreetName { get; set; }
        public string Number { get; set; }
        public string Floor { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public StandardAddress()
        {
        }
    }
}
