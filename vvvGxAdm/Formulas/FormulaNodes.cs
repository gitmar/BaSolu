namespace GxSaie.Formulas.ReviewConcepts
{
    public abstract class ExpressionNode
    {
        public abstract object Evaluate();
        public abstract LineType Type { get; }

    }
    public class ConstantNode : ExpressionNode
    {
        public object Value { get; }
        public LineType ValueType { get; }
        public ConstantNode(object value, LineType type)
        {
            Value = value;
            ValueType = type;
        }
        public override object Evaluate() => Value;
        public override LineType Type => ValueType;
    }
    public class BinaryOperatorNode : ExpressionNode
    {
        public string Operator { get; }
        public ExpressionNode Left { get; }
        public ExpressionNode Right { get; }

        public BinaryOperatorNode(string op, ExpressionNode left, ExpressionNode right)
        {
            Operator = op;
            Left = left;
            Right = right;
        }


        public override object Evaluate()
        {
            var a = Convert.ToDecimal(Left.Evaluate());
            var b = Convert.ToDecimal(Right.Evaluate());

            return Operator switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => b != 0 ? a / b : 0,
                _ => 0
            };
        }

        public override LineType Type => LineType.Decimal;
    }
    public class FunctionNode : ExpressionNode
    {
        public override LineType Type =>
            FunctionRegistry.Get(Name)?.ReturnType ?? LineType.String;
        public string Name { get; }
        public List<ExpressionNode> Arguments { get; }

        public FunctionNode(string name, List<ExpressionNode> args)
        {
            Name = name.ToLower();
            Arguments = args;
        }
        public override object Evaluate()
        {
            var func = FunctionRegistry.Get(Name);
            return func?.Evaluate(Arguments) ?? 0;
        }
    }
}

//public class FunctionNode : ExpressionNode
//{
//    public override LineType Type => LineType.Decimal;
//    public string Name { get; }
//    public List<ExpressionNode> Arguments { get; }
//    public FunctionNode(string name, List<ExpressionNode> args)
//    {
//        Name = name.ToLower();
//        Arguments = args;
//    }

//    public override object Evaluate()
//    {
//        return Name switch
//        {
//            "sum" => Arguments.Sum(arg => Convert.ToDecimal(arg.Evaluate())),
//            "if" => EvaluateIf(),
//            _ => 0
//        };
//    }
//    private object EvaluateIf()
//    {
//        if (Arguments.Count < 3) return 0;
//        var condition = Arguments[0].Evaluate()?.ToString();
//        return condition == "true" ? Arguments[1].Evaluate() : Arguments[2].Evaluate();
//    }
//    //public ExpressionNode? ResolveScdrub(string token)
//    //{
//    //    if (string.IsNullOrWhiteSpace(token)) return null;

//    //    var prefix = token.Substring(0, 1);
//    //    var number = token.Substring(1);

//    //    if (!int.TryParse(number, out var num)) return null;

//    //    if (prefix == "i")
//    //    {
//    //        var line = GetFormulaLineByNumber(token);
//    //        if (line?.Result?.Value != null)
//    //            return new ConstantNode(line.Result.Value, line.Result.Type ?? LineType.Decimal);
//    //    }
//    //    else
//    //    {
//    //        var etyp = ResolveTableType(prefix);
//    //        var scdrub = $"{prefix}{num}";
//    //        var input = GetInputValue(scdrub, etyp);

//    //        if (input?.Result?.Value != null)
//    //            return new ConstantNode(input.Result.Value, input.Result.Type ?? LineType.Decimal);
//    //    }

//    //    return null;
//    //}

//    private TableType ResolveTableType(string prefix) => prefix switch
//    {
//        "s" => TableType.Actsaie,
//        "sd" => TableType.Actdet,
//        "r" => TableType.Resdon,
//        "rd" => TableType.Resdet,
//        "b" => TableType.Resbro,
//        "p" => TableType.Tiersp,
//        "u" => TableType.Rubvar,
//        "v" => TableType.Actsaie, // default input
//        _ => TableType.Actsaie
//    };
//    public ExpressionNode? ResolveScdrub(string token)
//    {
//        if (string.IsNullOrWhiteSpace(token)) return null;

//        var prefix = token.Substring(0, 1);
//        var number = token.Substring(1);

//        if (!int.TryParse(number, out var num)) return null;

//        if (prefix == "i")
//        {
//            var line = GetFormulaLineByNumber(token);
//            if (line?.Result?.Value != null)
//                return new ConstantNode(line.Result.Value, line.Result.Type ?? LineType.Decimal);
//        }
//        else
//        {
//            var etyp = ResolveTableType(prefix);
//            var scdrub = $"{prefix}{num}";
//            var input = GetInputValue(scdrub, etyp);

//            if (input?.Result?.Value != null)
//                return new ConstantNode(input.Result.Value, input.Result.Type ?? LineType.Decimal);
//        }

//        return null;
//    }
//    public static ExpressionNode? ResolveScdrub(string token, FormulaEvaluationContext context)
//    {
//        if (string.IsNullOrWhiteSpace(token)) return null;

//        var prefix = token.Substring(0, 1);
//        var number = token.Substring(1);

//        if (!int.TryParse(number, out var num)) return null;

//        if (prefix == "i")
//        {
//            var line = FormulaLineByScdrub.GetFormulaLineByNumber(token, context.PreviousLines);
//            if (line?.Result?.Value != null)
//                return new ConstantNode(line.Result.Value, line.Result.Type ?? LineType.Decimal);
//        }
//        else
//        {
//            var etyp = ResolveTableType(prefix);
//            var scdrub = $"{prefix}{num}";

//            var input = context.InputData.FirstOrDefault(d =>
//                d.Idtie == context.Idtie &&
//                d.Etyp == etyp &&
//                d.Scdrub == scdrub &&
//                IsValid(d, context.SessionDate));

//            if (input?.Result?.Value != null)
//                return new ConstantNode(input.Result.Value, input.Result.Type ?? LineType.Decimal);
//        }
//        return null;
//    }
//}

