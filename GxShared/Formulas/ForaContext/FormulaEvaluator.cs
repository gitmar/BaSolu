using GxShared.Formulas.ForaModels;
namespace GxShared.Formulas.ForaContext
{
    public class FormulaLineEvaluator
    {
        private readonly FormulaEvaluationContext _context;
        private readonly FormulaLineRegistry _registry;

        public FormulaLineEvaluator(FormulaEvaluationContext context, FormulaLineRegistry registry)
        {
            _context = context;
            _registry = registry;
        }

        public FormulaEvaluationOutcome EvaluateWithLog(FormulaLine line)
        {
            var log = new FormulaEvaluationLog
            {
                LineNumber = line.LineNumber,
                Formula = line.Formula
            };

            if (string.IsNullOrWhiteSpace(line.Formula))
            {
                return new FormulaEvaluationOutcome
                {
                    Result = FormulaResult.Empty,
                    Log = log
                };
            }

            // Step 1: Tokenize
            var tokens = FormulaTokenizer.Tokenize(line.Formula);
            log.Tokens = tokens;

            // Step 2: Resolve tokens using context.InputData
            var resolver = new FormulaSemanticResolver(tokens, _context.InputData);
            var resolved = resolver.Resolve();
            log.Resolved = resolved;

            // Step 3: Trust filtering
            var filtered = resolved
                .Where(rt => rt.Trust != TrustLevel.Suspended && rt.Trust != TrustLevel.Invalid)
                .ToList();

            // Step 4: Parse tree using context
            var parser = new FormulaExpressionParser(filtered, _context);
            var tree = parser.Parse();

            // Step 5: Evaluate
            var value = tree.Evaluate();

            // Step 6: Wrap result
            var result = new FormulaResult
            {
                Raw = value?.ToString(),
                Value = value,
                Type = tree.Type,
                Trust = TrustLevel.Valid
            };

            // Step 7: Log result
            log.ResultValue = result.Value;
            log.ResultType = result.Type ?? LineType.String;
            log.Trust = result.Trust;

            return new FormulaEvaluationOutcome
            {
                Result = result,
                Log = log
            };
        }
    }
    public class FormulaEvaluator
    {
        private readonly List<ResolvedToken> _tokens;

        public FormulaEvaluator(List<ResolvedToken> tokens)
        {
            _tokens = tokens;
        }

        public FormulaResult Evaluate()
        {
            var stack = new Stack<object>();
            var ops = new Stack<string>();

            foreach (var token in _tokens)
            {
                switch (token.Token.Type)
                {
                    case FormulaTokenType.Constant:
                        stack.Push(ParseConstant(token.Token.Value));
                        break;

                    case FormulaTokenType.ScdrubReference:
                    case FormulaTokenType.EnumReference:
                        stack.Push(token.Value ?? 0); // fallback to 0 if unresolved
                        break;

                    case FormulaTokenType.Operator:
                        HandleOperator(token.Token.Value, stack, ops);
                        break;

                    case FormulaTokenType.Function:
                        var result = EvaluateFunction(token.Token.Value, stack);
                        stack.Push(result);
                        break;
                }
            }

            var final = stack.Count > 0 ? stack.Pop() : null;
            return new FormulaResult
            {
                Raw = final?.ToString(),
                Type = ResolveType(final),
                Value = final
            };
        }

        private object ParseConstant(string value)
        {
            if (decimal.TryParse(value, out var dec)) return dec;
            if (int.TryParse(value, out var i)) return i;
            return value.Trim('"');
        }

        private void HandleOperator(string op, Stack<object> stack, Stack<string> ops)
        {
            if (stack.Count < 2) return;
            var b = Convert.ToDecimal(stack.Pop());
            var a = Convert.ToDecimal(stack.Pop());

            object result = op switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => b != 0 ? a / b : 0,
                _ => 0
            };

            stack.Push(result);
        }

        private object EvaluateFunction(string name, Stack<object> stack)
        {
            name = name.ToLower();
            return name switch
            {
                "sum" => stack.Sum(x => Convert.ToDecimal(x)),
                "if" => EvaluateIf(stack),
                _ => 0
            };
        }

        private object EvaluateIf(Stack<object> stack)
        {
            if (stack.Count < 3) return 0;
            var falseVal = stack.Pop();
            var trueVal = stack.Pop();
            var condition = stack.Pop();

            return condition?.ToString() == "true" ? trueVal : falseVal;
        }

        private LineType ResolveType(object? value)
        {
            return value switch
            {
                string => LineType.String,
                int => LineType.Int,
                long => LineType.Long,
                decimal => LineType.Decimal,
                DateTime => LineType.Date,
                bool => LineType.Bool,
                _ => LineType.String
            };
        }
    }
    public class FormulaEvaluationContext
    {
        public int Idtie { get; set; } // current person
        public List<FormulaLine> PreviousLines { get; set; } = new();
        public List<InDataLineStream> InputData { get; set; } = new();

        public DateTime SessionDate { get; set; } = DateTime.Today;

        public FormulaLine? GetFormulaLineByNumber(string lineNumber)
        {
            return PreviousLines.FirstOrDefault(f => f.LineNumber == lineNumber);
        }

        public InDataLineStream? GetInputValue(string scdrub, TableType etyp)
        {
            return InputData.FirstOrDefault(d =>
                d.Idtie == Idtie &&
                d.Etyp == etyp &&
                d.Scdrub == scdrub &&
                IsValid(d));
        }

        private bool IsValid(InDataLineStream d)
        {
            return d.Trust != TrustLevel.Suspended &&
                   d.Datval <= SessionDate &&
                   (d.Result?.Value != null);
        }
    }
    public class FormulaEvaluationOutcome
    {
        public FormulaResult Result { get; set; } = FormulaResult.Empty;
        public FormulaEvaluationLog Log { get; set; } = new();
    }

    public class FormulaEvaluationLog
    {
        public string LineNumber { get; set; } = string.Empty;
        public string Formula { get; set; } = string.Empty;
        public List<FormulaToken> Tokens { get; set; } = new();
        public List<ResolvedToken> Resolved { get; set; } = new();
        public object? ResultValue { get; set; }
        public LineType ResultType { get; set; }
        public TrustLevel Trust { get; set; }
    }
}
