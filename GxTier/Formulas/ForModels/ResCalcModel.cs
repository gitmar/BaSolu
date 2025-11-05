using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GxSaie.Formulas.ReviewConcepts;

namespace GxSaie.Formulas.ForModels
{
    public enum LineType { W, I, D }
    public class ResCalcModel
    {
        public int Opetyp { get; set; } = 0;
        public int? Sireel { get; set; } = 0;
        public int Idorg { get; set; }
        public int Idpln { get; set; }
        public string Vperi { get; set; }
        public string Eperi { get; set; }
        public DateTime Datope { get; set; } = DateTime.MinValue;
        public int Gseq { get; set; } = 1; //no pai
        public List<InDataLineStream> InLines { get; set; } = new();
        public List<OutDataLineStream> OutLines { get; set; } = new();
    }
    //public class FormulaLine
    //{
    //    public int Id { get; set; } = 0; // e.g., "110", "29", "xxx"
    //    public int Idtie { get; set; } = 0;
    //    public string? Smatri { get; set; } = string.Empty;
    //    public int Idrub { get; set; } = 0;
    //    public int LineNumber { get; set; }
    //    public string? Scdrub { get; set; } = string.Empty;
    //    public string Formula { get; set; } = string.Empty;
    //    public string? Result { get; set; }
    //    public string? Resaie { get; set; } = string.Empty;   // user input
    //    public string? State { get; set; }
    //    public LineType Type { get; set; } = LineType.I;
    //    public int? Atyp { get; set; } = 0;
    //    public bool IsDefault { get; set; } = false;       // true for default line ("xxx")
    //    public string? Expression { get; set; }            // formula expression
    //    public object? EvalResult { get; set; }            // result of evaluation
    //    public TrustLevel InputTrust { get; set; } = TrustLevel.UserInput;


    //    public int Idpres { get; set; } = 0;
    //    public int Idprub { get; set; } = 0;
    //    public int? ParentLine { get; set; }
    //    //pour la preparation de la saisie
    //    public string? Liba { get; set; } = string.Empty;
    //    public string? Abg { get; set; } = string.Empty;
    //    public string? Nom { get; set; } = string.Empty;
    //    public string? Pnom { get; set; } = string.Empty;
    //    public bool IsActive { get; set; } = true;
    //}
}
