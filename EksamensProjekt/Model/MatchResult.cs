using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksamensProjekt.Model
{
    public class MatchResult
    {
        public ModifiedExcelAddress ImportedAddress { get; set; }
        public StandardAddress DatabaseAddress { get; set; }
        public string MatchType { get; set; } //make enum?
        public int TenancyID { get; set; }
    }

}
