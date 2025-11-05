using GxShared.Formulas.ForaModels;
namespace GxShared.Formulas.ForaContext
{
    public class FormulaLineRegistry
    {
        private readonly List<FormulaLine> _allLines;

        public FormulaLineRegistry(IEnumerable<FormulaLine> lines)
        {
            _allLines = lines.ToList();
        }

        public IEnumerable<FormulaLine> All => _allLines;

        public IEnumerable<FormulaLine> Writable =>
            _allLines.Where(l => !l.IsClientOnly);

        public IEnumerable<FormulaLine> ClientOnly =>
            _allLines.Where(l => l.IsClientOnly);

        public FormulaLine? GetByLineNumber(string token)
        {
            if (string.IsNullOrWhiteSpace(token) || !token.StartsWith("i")) return null;
            var key = token.Substring(1).TrimEnd('c');
            return _allLines.FirstOrDefault(l => l.LineKey == key);
        }

        public IEnumerable<FormulaLine> DependenciesOf(string formula)
        {
            var tokens = FormulaTokenizer.Tokenize(formula);
            var refs = tokens
                .Where(t => t.Type == FormulaTokenType.ScdrubReference && t.Value.StartsWith("i"))
                .Select(t => t.Value.Substring(1).TrimEnd('c'));

            return _allLines.Where(l => refs.Contains(l.LineKey));
        }
    }
    //var registry = new FormulaLineRegistry(formulaLines);

    //    var writableLines = registry.Writable.ToList();       // for server sync
    //    var clientDrafts = registry.ClientOnly.ToList();       // for discard
    //    var line158 = registry.GetByLineNumber("i158c");       // normalized lookup
    //    var deps = registry.DependenciesOf("sum(i120..i123)"); // dependency tracking
 }
