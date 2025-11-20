using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.GlobModels
{
    public class Inscinv
    {
        public string Sduname { get; set; }
        public string Sdemail { get; set; }
        public string Sdid { get; set; }
        public int SdIdorg { get; set; }
        public int SdIdhie { get; set; }
        public string StrOrga { get; set; }
        public string StrHier { get; set; }
        public int Sdqui { get; set; }
        public string SdSrole { get; set; }
        public ICollection<Apirole> AllRoles { get; set; } = new List<Apirole>();
        public ICollection<Apirole> UsrRoles { get; set; } = new List<Apirole>();
        public int Idtiew { get; set; }
        ////public Tiewel Mytiewel { get; set; }
        public string Tiemail { get; set; }
        public string Tienom { get; set; }
        public string Tiepnom { get; set; }
        public int Toqui { get; set; }
        public int Tievers { get; set; }
        //public int TieIdhie { get; set; }
        public int Dequi { get; set; }
        public string Idhie { get; set; } = string.Empty;
        public string Idpst { get; set; } = string.Empty;
        public int TieIrole { get; set; }
        public string TieSrole { get; set; }
        public string Tiephone { get; set; }
        public int Tiepays { get; set; }
        public int Tieville { get; set; }
        public string Iraison { get; set; }
        public int SdSiparain { get; set; }
        public int LocServer { get; set; }
        public string Stdoma { get; set; }
        public int MethP { get; set; }
        public bool IsPret { get; set; }
        public string CdInvite { get; set; }
    }
}
