using Microsoft.OData.Edm;

namespace GxSaie.Formulas.ReviewConcepts
{
    public interface IFormulaFunction
    {
        string Name { get; }
        LineType ReturnType { get; }
        object Evaluate(List<ExpressionNode> args);
    }
    public static class FunctionRegistry
    {
        private static readonly Dictionary<string, IFormulaFunction> _functions = new();

        static FunctionRegistry()
        {
            Register(new SumFunction());
            Register(new AvgFunction());
            Register(new MaxFunction());
            Register(new SwitchFunction());
            //Register(new GroupByFunction()); // placeholder
        }

        public static void Register(IFormulaFunction function)
        {
            _functions[function.Name.ToLower()] = function;
        }

        public static IFormulaFunction? Get(string name)
        {
            _functions.TryGetValue(name.ToLower(), out var func);
            return func;
        }
        public class SumFunction : IFormulaFunction
        {
            public string Name => "sum";
            public LineType ReturnType => LineType.Decimal;

            public object Evaluate(List<ExpressionNode> args)
            {
                return args.Sum(arg => Convert.ToDecimal(arg.Evaluate()));
            }
        }
        public class AvgFunction : IFormulaFunction
        {
            public string Name => "avg";
            public LineType ReturnType => LineType.Decimal;

            public object Evaluate(List<ExpressionNode> args)
            {
                var values = args.Select(arg => Convert.ToDecimal(arg.Evaluate())).ToList();
                return values.Count > 0 ? values.Average() : 0;
            }
        }
        public class MaxFunction : IFormulaFunction
        {
            public string Name => "max";
            public LineType ReturnType => LineType.Decimal;

            public object Evaluate(List<ExpressionNode> args)
            {
                return args.Max(arg => Convert.ToDecimal(arg.Evaluate()));
            }
        }
        public class SwitchFunction : IFormulaFunction
        {
            public string Name => "switch";
            public LineType ReturnType => LineType.String; // Can be dynamic if needed

            public object Evaluate(List<ExpressionNode> args)
            {
                if (args.Count < 3) return 0;

                var condition = args[0].Evaluate()?.ToString();

                for (int i = 1; i < args.Count - 1; i += 2)
                {
                    var caseValue = args[i].Evaluate()?.ToString();
                    var resultValue = args[i + 1].Evaluate();

                    if (condition == caseValue)
                        return resultValue;
                }

                // Optional: fallback if no match
                return args.Count % 2 == 0 ? args.Last().Evaluate() : 0;
            }
        }
    }
    //switch("A"; "A"; 100; "B"; 200; "C"; 300) → 100
    //switch("B"; "A"; 100; "B"; 200; "C"; 300) → 200
    //switch("X"; "A"; 100; "B"; 200; "C"; 300; 999) → 999 // fallback
}
