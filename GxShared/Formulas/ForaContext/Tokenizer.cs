using System.Text;
using System.Text.RegularExpressions;
using GxShared.Formulas.ForaModels;

namespace GxShared.Formulas.ForaContext
{
    //s130*3+v128/if(p127="A1E1";10;20)
    public class FormulaToken
    {
        public FormulaTokenType Type { get; set; }
        public string Value { get; set; } = string.Empty;
        public int Position { get; set; } // character index in formula
    }
    public static class FormulaTokenizer
    {
        private static readonly HashSet<string> Functions = new() { "sum", "if" };
        private static readonly HashSet<char> Operators = new() { '+', '-', '*', '/', '(', ')', '=', ';' };

        public static List<FormulaToken> Tokenize(string formula)
        {
            var tokens = new List<FormulaToken>();
            var buffer = new StringBuilder();
            int position = 0;

            for (int i = 0; i < formula.Length; i++)
            {
                char c = formula[i];

                if (char.IsWhiteSpace(c)) continue;

                if (Operators.Contains(c))
                {
                    FlushBuffer();
                    tokens.Add(new FormulaToken { Type = FormulaTokenType.Operator, Value = c.ToString(), Position = i });
                    continue;
                }

                buffer.Append(c);

                // Lookahead for end of token
                bool isEnd = (i + 1 == formula.Length) || Operators.Contains(formula[i + 1]) || char.IsWhiteSpace(formula[i + 1]);
                if (isEnd) FlushBuffer();
            }

            return tokens;

            void FlushBuffer()
            {
                if (buffer.Length == 0) return;
                string token = buffer.ToString();
                var type = ResolveType(token);
                tokens.Add(new FormulaToken { Type = type, Value = token, Position = position });
                position += token.Length;
                buffer.Clear();
            }
        }

        private static FormulaTokenType ResolveType(string token)
        {
            if (Functions.Contains(token.ToLower())) return FormulaTokenType.Function;
            if (Regex.IsMatch(token, @"^[svrikg]\d+$")) return FormulaTokenType.ScdrubReference;
            if (Regex.IsMatch(token, @"^p\d+$")) return FormulaTokenType.EnumReference;
            if (Regex.IsMatch(token, @"^\d+(\.\d+)?$")) return FormulaTokenType.Constant;
            return FormulaTokenType.Variable;
        }
    }
}
