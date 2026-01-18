using System.ComponentModel.DataAnnotations.Schema;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace GxWapi.DaModels
{
    public partial class Actsaie
    {
        [JsonIgnore]
        public int Iui { get; set; } = 0;
        [JsonIgnore]
        public int Xadd1 { get; set; } = 0;
        [JsonIgnore]
        public int Xedt1 { get; set; } = 0;
        [JsonIgnore]
        public string Liba { get; set; } = string.Empty;
        [JsonIgnore]
        public string KMatri { get; set; } = string.Empty;
        [JsonIgnore]
        public string Nom { get; set; } = string.Empty;
        [JsonIgnore]
        public string Pnom { get; set; } = string.Empty;
        [JsonIgnore]
        public string Abg { get; set; } = string.Empty;
        [JsonIgnore]
        public string Frsource { get; set; } = string.Empty;
    }
    public partial class Actdet
    {
        [JsonIgnore]
        public int Iui { get; set; } = 0;
        [JsonIgnore]
        public int Xadd1 { get; set; } = 0;
        [JsonIgnore]
        public int Xedt1 { get; set; } = 0;
        [JsonIgnore]
        public string Liba { get; set; } = string.Empty;
        [JsonIgnore]
        public string Abg { get; set; } = string.Empty;
        [JsonIgnore]
        public int Col { get; set; } = 0;
        [JsonIgnore]
        public int Lne { get; set; } = 0;
        [JsonIgnore]
        public int Lgtf { get; set; } = 0;
        [JsonIgnore]
        public string Ftsource { get; set; } = string.Empty;
    }
    public partial class Resdon
    {
        [JsonIgnore]
        public int Xadd1 { get; set; } = 0;
        [JsonIgnore]
        public int Xedt1 { get; set; } = 0;
        [JsonIgnore]
        public string Liba { get; set; } = string.Empty;
        [JsonIgnore]
        public string SMatri { get; set; } = string.Empty;
        [JsonIgnore]
        public string Nom { get; set; } = string.Empty;
        [JsonIgnore]
        public string Pnom { get; set; } = string.Empty;
        [JsonIgnore]
        public string Abg { get; set; } = string.Empty;
        [JsonIgnore]
        public string Frsource { get; set; } = string.Empty;
    }
    public partial class Resdet
    {
        [JsonIgnore]
        public int Xadd1 { get; set; } = 0;
        [JsonIgnore]
        public int Xedt1 { get; set; } = 0;
        [JsonIgnore]
        public string Liba { get; set; } = string.Empty;
        [JsonIgnore]
        public string Abg { get; set; } = string.Empty;
        [JsonIgnore]
        public int Col { get; set; } = 0;
        [JsonIgnore]
        public int Lne { get; set; } = 0;
        [JsonIgnore]
        public int Lgtf { get; set; } = 0;
        [JsonIgnore]
        public string Ftsource { get; set; } = string.Empty;
    }
    public partial class Resbro
    {
        [JsonIgnore]
        public int Xadd1 { get; set; } = 0;
        [JsonIgnore]
        public int Xedt1 { get; set; } = 0;
        [JsonIgnore]
        public string Liba { get; set; } = string.Empty;
        [JsonIgnore]
        public string SMatri { get; set; } = string.Empty;
        [JsonIgnore]
        public string Nom { get; set; } = string.Empty;
        [JsonIgnore]
        public string Pnom { get; set; } = string.Empty;
        [JsonIgnore]
        public string Abg { get; set; } = string.Empty;
        [JsonIgnore]
        public string Frsource { get; set; } = string.Empty;
    }
    public partial class Gsgfix
    {
        [JsonIgnore]
        public int Iui { get; set; } = 0;
        [JsonIgnore]
        public bool IsParent => (Gvars == 4 && (Itb == 1 || Itb ==2));
        [JsonIgnore]
        public int Xadd1 { get; set; } = 0;
        [JsonIgnore]
        public int Xedt1 { get; set; } = 0;
        [JsonIgnore]
        public int Xdel1 { get; set; } = 0;
        [JsonIgnore]
        public int Xgen1 { get; set; } = 0;
        //[JsonIgnore]
        //public List<Gsglne> colsxVM { get; set; } = new();
    }
    public partial class Gsglne
    {
        [JsonIgnore]
        public int Iui { get; set; } = 0;
        [JsonIgnore]
        public int Xadd1 { get; set; } = 0;
        [JsonIgnore]
        public int Xedt1 { get; set; } = 0;
    }
    public partial class Plngen
    {
        [JsonIgnore]
        public int Iui { get; set; } = 0;
        [JsonIgnore]
        public int Xadd1 { get; set; } = 0;
        [JsonIgnore]
        public int Xedt1 { get; set; } = 0;
    }
    public partial class Rubvar
    {
        [JsonIgnore]
        public int Iui { get; set; } = 0;
        [JsonIgnore]
        public int Xadd1 { get; set; } = 0;
        [JsonIgnore]
        public int Xedt1 { get; set; } = 0;
    }
    public partial class Rubfmt
    {
        [JsonIgnore]
        public int Iui { get; set; } = 0;
        [JsonIgnore]
        public int Xadd1 { get; set; } = 0;
        [JsonIgnore]
        public int Xedt1 { get; set; } = 0;
    }
    public partial class Rubhie
    {
        [JsonIgnore]
        public int Iui { get; set; } = 0;
        [JsonIgnore]
        public int Xadd1 { get; set; } = 0;
        [JsonIgnore]
        public int Xedt1 { get; set; } = 0;
    }
    public partial class Rubpst
    {
        [JsonIgnore]
        public int Iui { get; set; } = 0;
        [JsonIgnore]
        public int Xadd1 { get; set; } = 0;
        [JsonIgnore]
        public int Xedt1 { get; set; } = 0;
    }
    public partial class Tiersp
    {
        [JsonIgnore]
        public int Iui { get; set; } = 0;
        [JsonIgnore]
        public int Xadd1 { get; set; } = 0;
        [JsonIgnore]
        public int Xedt1 { get; set; } = 0;
        [JsonIgnore]
        public int Xgen1 { get; set; } = 0;
        [JsonIgnore]
        public int Nwhie { get; set; } = 0;
        [JsonIgnore]
        public int Nwpst { get; set; } = 0;
        [JsonIgnore]
        public bool IsJaccChecked
        {
            get => Jacc == 1;
            set => Jacc = value ? 1 : 0;
        }
        [JsonIgnore]
        public bool IsJrejChecked
        {
            get => Jrej == 1;
            set => Jrej = value ? 1 : 0;
        }
        [JsonIgnore]
        public bool IsJnotChecked
        {
            get => Jnot == 1;
            set => Jnot = value ? 1 : 0;
        }
    }
    public partial class Tiewel
    {
        [JsonIgnore]
        public int Iui { get; set; } = 0;
        [JsonIgnore]
        public int Xadd1 { get; set; } = 0;
        [JsonIgnore]
        public int Xedt1 { get; set; } = 0;
        [JsonIgnore]
        public int Xgen1 { get; set; } = 0;
        [JsonIgnore]
        public bool IsJaccChecked
        {
            get => Jacc == 1;
            set => Jacc = value ? 1 : 0;
        }
        [JsonIgnore]
        public bool IsJrejChecked
        {
            get => Jrej == 1;
            set => Jrej = value ? 1 : 0;
        }
        [JsonIgnore]
        public bool IsJnotChecked
        {
            get => Jnot == 1;
            set => Jnot = value ? 1 : 0;
        }
        //[JsonIgnore]
        //private bool isValCheck = false;
        //[JsonIgnore]
        //private bool isRejCheck = false;
        //[JsonIgnore]
        //public bool IsValCheck
        //{
        //    get => isValCheck;
        //    set
        //    {
        //        if (value)
        //        {
        //            if (isRejCheck) isRejCheck = false;
        //        }
        //        isValCheck = value;
        //    }
        //}
        //[JsonIgnore]
        //public bool IsRejCheck
        //{
        //    get => isRejCheck;
        //    set
        //    {
        //        if (value)
        //        {
        //            if (isValCheck) isValCheck = false;
        //        }
        //        isRejCheck = value;
        //    }
        //}
        //[JsonIgnore]
        //public bool IsNotCheck { get; set; } = false;
    }
    public partial class Tieafl
    {
        [JsonIgnore]
        public int Iui { get; set; } = 0;
        [JsonIgnore]
        public int Xadd1 { get; set; } = 0;
        [JsonIgnore]
        public int Xedt1 { get; set; } = 0;
        [JsonIgnore]
        public int Xgen1 { get; set; } = 0;

        [JsonIgnore]
        private bool isValCheck = false;
        [JsonIgnore]
        private bool isRejCheck = false;
        [JsonIgnore]
        public bool IsValCheck
        {
            get => isValCheck;
            set
            {
                if (value)
                {
                    if (isRejCheck) isRejCheck = false;
                }
                isValCheck = value;
            }
        }
        [JsonIgnore]
        public bool IsRejCheck
        {
            get => isRejCheck;
            set
            {
                if (value)
                {
                    if (isValCheck) isValCheck = false;
                }
                isRejCheck = value;
            }
        }
        [JsonIgnore]
        public bool IsNotCheck { get; set; } = false;
    }
    public partial class Tiwafl
    {
        [JsonIgnore]
        public int Iui { get; set; } = 0;
        [JsonIgnore]
        public int Xadd1 { get; set; } = 0;
        [JsonIgnore]
        public int Xedt1 { get; set; } = 0;
        [JsonIgnore]
        public int Xgen1 { get; set; } = 0;
        [JsonIgnore]
        private bool isValCheck = false;
        [JsonIgnore]
        private bool isRejCheck = false;
        [JsonIgnore]
        public bool IsValCheck
        {
            get => isValCheck;
            set
            {
                if (value)
                {
                    if (isRejCheck) isRejCheck = false;
                }
                isValCheck = value;
            }
        }
        [JsonIgnore]
        public bool IsRejCheck
        {
            get => isRejCheck;
            set
            {
                if (value)
                {
                    if (isValCheck) isValCheck = false;
                }
                isRejCheck = value;
            }
        }
        [JsonIgnore]
        public bool IsNotCheck { get; set; } = false;
    }
    public partial class Gsesio
    {
        [JsonIgnore]
        public int Iui { get; set; } = 0;
        [JsonIgnore]
        public int Xadd1 { get; set; } = 0;
        [JsonIgnore]
        public int Xedt1 { get; set; } = 0;
    }

    public class Mytable
    {
        public List<Eltable> langues { get; set; } = new List<Eltable>();
        public List<Eltable> domas { get; set; } = new List<Eltable>();
        public List<Eltable> sites { get; set; } = new List<Eltable>();
        public List<Eltable> countries { get; set; } = new List<Eltable>();
        public List<Eltable> regions { get; set; } = new List<Eltable>();
        public List<Eltable> paiements { get; set; } = new List<Eltable>();
        public List<Eltable> etapes { get; set; } = new List<Eltable>();
        public List<Eltable> statuts { get; set; } = new List<Eltable>();
        public List<Eltable> roles { get; set; } = new List<Eltable>();
    }
    public class Eltable
    {
        public int Elea { get; set; }
        public string Liba { get; set; }
        public string Abg { get; set; }
    }
}