namespace GxShared.Formulas.ForModels

public class FormulaLine// read only
{
    //120: s130*3+v128/IF(p127="A1E1";10;20); [w]
    //130: i37+sum(v120+v129); [d]
    //158: (v100+v120)*15/100;
    //245: (v200+70+sum(i120..i158))*15/g158; [i]
    //kxxx access current input value (valsaie)/always there
    //sxxx access actsaie value (enum -> TableTyp)
    public int Id { get; set; } = 0;
    public string LineNumber { get; set; } = string.Empty; // e.g. "158", "158c"
    public bool IsClientOnly => LineNumber.EndsWith("c", StringComparison.OrdinalIgnoreCase);
    public string LineKey => LineNumber.TrimEnd('c'); // normalized for lookup
    public int? Idrub { get; set; } = 0;
    public string? Formula { get; set; } = string.Empty;
    public string? Valresu { get; set; } = string.Empty;
    public string? Valsaie { get; set; } = string.Empty;
    public LineType? Vtyp { get; set; } // 1: string, 2: int, 3: long, 4: decimal, 5: DateTime, 6: bool
    public FormulaResult? Result { get; set; }
    public DateTime? Datval { get; set; } = DateTime.MinValue;
    public PeriodType? Ptyp { get; set; } // 1: always, 2: to session, 3: to exo, 4: from a to b
    public DateTime? Ddeb { get; set; } = DateTime.MinValue;
    public DateTime? Dfin { get; set; } = DateTime.MinValue;
    public int Source { get; set; } = 0; //1-rubvar 2-program
    public bool IsSuspended { get; set; } = false;
}
//InDataLineStream
public class InDataLineStream : DataLineBase // read only
{
    public int Id { get; set; } = 0;
    public TableType Etyp { get; set; } // 1: Actsaie, 2: Actdet, 3: Resdon, 4: Resdet, 5: Resbro, 6: Tiersp, 7: Rubvar
    public string Scdrub { get; set; } = string.Empty;
    public int Idrub { get; set; } = 0;
    public int Idtie { get; set; } = 0;
    public LineType? Vtyp { get; set; } // 1: string, 2: int, 3: long, 4: decimal, 5: DateTime, 6: bool
    public FormulaResult? Result { get; set; }
    public DateTime? Datval { get; set; } = DateTime.MinValue;
    public TrustLevel Trust { get; set; } = TrustLevel.Valid;
}
public class OutDataLineStream // writable at client, read only at server
{
    public int Id { get; set; } = 0;
    public string? LineNumber { get; set; } = string.Empty;
    public int SourceFormulaId { get; set; } = 0;
    public TableType Etyp { get; set; }  // 1: Actsaie, 2: Actdet, 3: Resdon, 4: Resdet, 5: Resbro, 6: Tiersp, 7: Rubvar
    public string Scdrub { get; set; } = string.Empty;
    public int Idrub { get; set; } = 0;
    public int Idtie { get; set; } = 0;
    public LineType? Vtyp { get; set; } // 1: string, 2: int, 3: long, 4: decimal, 5: DateTime, 6: bool
    public FormulaResult? Result { get; set; }
    public DateTime? Ddeb { get; set; } = DateTime.MinValue;
    public DateTime? Dfin { get; set; } = DateTime.MinValue;
    public TrustLevel Trust { get; set; } = TrustLevel.Valid;
    // Augmented metadata
    public string? Liba { get; set; }     // Label
    public string? Abg { get; set; }      // Abbreviation
    public string? Nom { get; set; }      // Full name
    public string? Pnom { get; set; }     // Short name
    public string? Smatri { get; set; }   // Matrix code
}

public class CalculationContext
{
    public int Opetyp { get; set; }
    public int Sireel { get; set; }
    public int Idorg { get; set; }
    public int Idses { get; set; }
    public int Idexo { get; set; }
    public DateTime Datope { get; set; }
    public string? SessionLabel { get; set; } //session or program label
    public TrustLevel Trust { get; set; } = TrustLevel.Valid;
}
public class OutDataContainer
{
    public int Id { get; set; }
    public CalculationContext CalculContext { get; set; }
    public DateTime Datope { get; set; } = DateTime.MinValue;
    public ICollection<OutDataLineStream> Lines { get; set; } = new List<OutDataLineStream>();
    public string? CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now; 
    public int ProgramId { get; set; } = 0;
    public TrustLevel Trust { get; set; } = TrustLevel.Valid;
}

public class FormulaResult
{
    public string? Raw { get; set; }
    public LineType? Type { get; set; }
    public object? Value { get; set; } // or use typed fields
    public DateTime? Time { get; set; } = null;
    public TrustLevel Trust { get; set; } = TrustLevel.Valid;
    public bool IsEmpty => Value == null;
    public static FormulaResult Empty => new FormulaResult
    {
        Raw = null,
        Value = null,
        Type = LineType.String,
        Trust = TrustLevel.Unknown
    };

}
public abstract class DataLineBase
{
    public int Id { get; set; }
    public TableType Etyp { get; set; }
    public string Scdrub { get; set; }
    public int Idrub { get; set; }
    public int Idtie { get; set; }
    public PeriodType? Ptyp { get; set; } // 1: always, 2: to session, 3: to exo, 4: from a to b
    public DateTime? Ddeb { get; set; } = DateTime.MinValue;
    public DateTime? Dfin { get; set; } = DateTime.MinValue;
    public LineType? Vtyp { get; set; }
    public FormulaResult? Result { get; set; }
    public TrustLevel Trust { get; set; } = TrustLevel.Valid;
    // Augmented metadata
    public string? Liba { get; set; }     // Label
    public string? Abg { get; set; }      // Abbreviation
    public string? Nom { get; set; }      // Full name
    public string? Pnom { get; set; }     // Short name
    public string? Smatri { get; set; }   // Matrix code
}
