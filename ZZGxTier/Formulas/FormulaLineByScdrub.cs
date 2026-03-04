namespace GxSaie.Formulas.ReviewConcepts
{
    public class FormulaLineByScdrub
    {
        public static FormulaLine? GetFormulaLineByNumber(string token, List<FormulaLine> previousLines)
        {
            if (string.IsNullOrWhiteSpace(token) || !token.StartsWith("i")) return null;

            var lineNumberStr = token.Substring(1);
            if (!int.TryParse(lineNumberStr, out var lineNumber)) return null;

            return previousLines.FirstOrDefault(f => int.TryParse(f.LineNumber, out var ln) && ln == lineNumber);
        }
    }
}
