namespace GxShared.Formulas.ForaModels
{
    public enum LineType
    {
        String = 1, //string
        Int = 2, //int
        Long = 3, //long
        Decimal = 4, //decimal
        Date = 5, //DateTime
        Bool = 6  //bool
    }
    public enum TableType
    {
        Actsaie = 1, // Actsaie (s)
        Actdet = 2, // Actdet (sd)
        Resdon = 3, // Resdon (r)
        Resdet = 4, // Resdet (rd)
        Resbro = 5, // Resbro (b)
        Tiersp = 6, // Tiersp (p)
        Rubvar = 7 // Rubvar (u)
    }

    [Flags]
    public enum TiersProp
    {
        Idtie = 110, // Idtie
        Smatri = 115, // Smatri
        Nom = 140, // Nom
        Pnom = 143, // Pnom
        Sgrad = 160, // grade
        Sfonc = 163, // fonction
        Sexe = 170, // sexe
        Statut = 173, // statut
        Idhie = 180, // Idhie - service
        Idpst = 183, // Idpst - poste
        Dnais = 190, // Dnais
        Dinsc = 193, // Dinsc
        Demb = 196, // Demb
        Adr = 310, // Adr
        Adr1 = 315, // Adr1
    }
    public enum FormulaTokenType
    {
        Variable,
        Operator,
        Function,
        Constant,
        EnumReference,
        ScdrubReference
    }
    public enum PeriodType
    {
        Always = 1, // Always valid
        Session = 2, // valid session only
        Exo = 3, // valid exo only
        Fromto = 4, // valid from date1 to date2 
    }
    public enum TrustLevel
    {
        Accepted = 0,
        Valid = 1,
        Suspended = 2,
        Error = 3,
        Warning = 4,
        Unknown = 5,
        Invalid = 6
    }
}
