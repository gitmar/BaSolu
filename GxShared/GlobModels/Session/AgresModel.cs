using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.GlobModels
{
    public class Gpdivh
    {
        public int Idorg { get; set; } = 0;
        public int Idpln { get; set; } = 0;
        public int Idhie { get; set; } = 0;
        public int Code { get; set; } = 0;
        public int? Pdom { get; set; } = 0;
        public int? Patr { get; set; } = 0;
        public int? Tyhie { get; set; } = 0;
        public int? Siroot { get; set; } = 0;
        public int? Ydpd { get; set; } = 0;
        public int? Sidiv { get; set; } = 0;
        public string Liba { get; set; } = string.Empty;
        public string Abg { get; set; } = string.Empty;
        public string Refa { get; set; } = string.Empty;
        public ICollection<Divpst> Divpsts { get; set; } = new List<Divpst>();
    }
    public class Divpst
    {
        public int Idpst { get; set; } = 0;
        public int? Idhie { get; set; } = 0;
        public int Code { get; set; } = 0;
        public string Liba { get; set; } = string.Empty;
        public string Abg { get; set; } = string.Empty;
        public string Refa { get; set; } = string.Empty;
    }
}
