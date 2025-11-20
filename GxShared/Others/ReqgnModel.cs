using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.Others
{
    public class ReqgnModel
    {
        public long Id { get; set; } = 0;
        public string Usrid { get; set; } = string.Empty;
        public int Idorg { get; set; } = 0;
        public int Otyp { get; set; } = 0;
        public int Orig { get; set; } = 0;
        public int ReqInt { get; set; } = 0;
        public string ReqStr { get; set; } = string.Empty;
        public string ReqPara1 { get; set; } = string.Empty;
        public string ReqPara2 { get; set; } = string.Empty;
        public int Utyp { get; set; } = 0;
        public DateTime Datact { get; set; } = DateTime.Now;
        public string Cdinvit { get; set; } = string.Empty;
        //pour les colonnes
        //public int Porig { get; set; } = 0; //Orig
        //public int Pidorg { get; set; } = 0; //Idorg
        public int Pdomtie { get; set; } = 0;
        public int Ptyptie { get; set; } = 0;
        public int Psupafl { get; set; } = 0;
        public List<string> Lsusda { get; set; } = new List<string>();
    }
}
