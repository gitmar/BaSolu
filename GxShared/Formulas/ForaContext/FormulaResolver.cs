using GxShared.Formulas.ForaModels;
namespace GxShared.Formulas.ForaContext
{
    public static class FormulaResolver
    {
        public static ExpressionNode? ResolveScdrub(string token, FormulaEvaluationContext context)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;

            var prefix = token.Substring(0, 1);
            var number = token.Substring(1);

            if (!int.TryParse(number, out var num)) return null;

            if (prefix == "i")
            {
                var line = FormulaLineByScdrub.GetFormulaLineByNumber(token, context.PreviousLines);
                if (line?.Result?.Value != null)
                    return new ConstantNode(line.Result.Value, line.Result.Type ?? LineType.Decimal);
            }
            else
            {
                var etyp = ResolveTableType(prefix);
                var scdrub = $"{prefix}{num}";

                var input = context.InputData.FirstOrDefault(d =>
                    d.Ptie == context.Idtie &&
                    d.Etyp == etyp &&
                    d.Scdrub == scdrub &&
                    IsValid(d, context.SessionDate));

                if (input?.Result?.Value != null)
                    return new ConstantNode(input.Result.Value, input.Result.Type ?? LineType.Decimal);
            }

            return null;
        }

        public static TableType ResolveTableType(string prefix) => prefix switch
        {
            "s" => TableType.Actsaie,
            "sd" => TableType.Actdet,
            "r" => TableType.Resdon,
            "rd" => TableType.Resdet,
            "b" => TableType.Resbro,
            "p" => TableType.Tiersp,
            "u" => TableType.Rubvar,
            "v" => TableType.Actsaie,
            _ => TableType.Actsaie
        };

        public static bool IsValid(InDataLineStream d, DateTime sessionDate)
        {
            return d.Trust != TrustLevel.Suspended &&
                   d.Datval <= sessionDate &&
                   d.Result?.Value != null;
        }
    }
}
