using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.GlobModels
{
    public class CalcRefera
    {
        public string Tbn { get; set; } // Refers to a table name in the formula (e.g., AA, AP, AG, RC, TX)
        public string TbName { get; set; } // Corresponding table name (e.g., Actsaie, Rescum, Tiersp)
        public ICollection<CalcField> Fields { get; set; } // Collection of fields in the table
    }

    public class CalcField
    {
        public string FdName { get; set; } // Field name (e.g., Svala, Fullname)
    }

}
