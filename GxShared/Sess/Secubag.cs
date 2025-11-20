using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.Session
{
    public class Secubag
    {
        [Key]
        public long Id { get; set; }
        public string Userid { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool Succeeded { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public ICollection<Accaut> Fxauts { get; set; } = new List<Accaut>();
        //public ICollection<RolModel> Fxrols { get; set; } = new List<RolModel>();
        //public ICollection<UsrModel> FxURAs { get; set; } = new List<UsrModel>();
    }
    public class Accaut
    {
        public int Id { get; set; }
        public int Aseq { get; set; }
        public int? Avar { get; set; }
        public int? Alea { get; set; }
        public string? Onom { get; set; }
        public string? Oabg { get; set; }
        public int? Oeta { get; set; }
        public int? Ono { get; set; }
        public bool? Siop { get; set; }
        public int? Oit { get; set; }
        public int? Eta { get; set; }
        public string? Obsv { get; set; }
        //public virtual ICollection<Gsuaut> Gsuauts { get; set; } = new List<Gsuaut>();
    }
}
