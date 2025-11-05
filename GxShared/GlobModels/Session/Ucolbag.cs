using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GxShared.GlobModels
{
    public class Usaibag
    {
        [Key]
        public int Id { get; set; } = 0;
        public bool Succeeded { get; set; } = false;
        public int Idorg { get; set; } = 0;
        public ICollection<Clvar> Ucols { get; set; } = new List<Clvar>();
        public ICollection<string> ErrList { get; set; } = new List<string>();
    }
    public class Clvar
    {
        public int Xseq { get; set; } = 0;
        public int Code { get; set; } = 0;
        public int Xsac { get; set; } = 0;
        public int Gvars { get; set; } = 0;
        public int Itb { get; set; } = 0;
        public int Ade { get; set; } = 0;
        public int Elea { get; set; } = 0;
        public int Sigrid { get; set; } = 0;
        public int Ele2 { get; set; } = 0;
        public string Liba { get; set; } = string.Empty;
        public string Abg { get; set; } = string.Empty;
        public int Tovers { get; set; } = 0;
        public int Idorg { get; set; } = 0;
        public int Idhie { get; set; } = 0;
        public int Dpdce { get; set; } = 0;
        public int Xdpd { get; set; } = 0;
        public int Ydpd { get; set; } = 0;
        public int Epai { get; set; } = 0;
        public int Totyp { get; set; } = 0;
        public int Etyp { get; set; } = 0;
        public int Ztyp { get; set; } = 0;
        public int Jtyp { get; set; } = 0;
        public int Ktyp { get; set; } = 0;
        public int Ltyp { get; set; } = 0;
        public int Zgpe { get; set; } = 0;
        public int Zfro { get; set; } = 0;
        public string Scdrub { get; set; } = string.Empty;
        public int Ivala { get; set; } = 0;
        public string Svala { get; set; } = string.Empty;
        public float Rvala { get; set; } = 0;
        public int Pseq { get; set; } = 0;
        public int Gtyp { get; set; } = 0;
        public int Eta { get; set; } = 0;
    }

    
}