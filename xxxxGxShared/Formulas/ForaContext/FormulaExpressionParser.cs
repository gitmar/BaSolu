using GxShared.Formulas.ForaModels;
namespace GxShared.Formulas.ForaContext
{
    public class FormulaExpressionParser
    {
        private readonly List<ResolvedToken> _tokens;
        private readonly FormulaEvaluationContext _context;
        private int _position = 0;

        public FormulaExpressionParser(List<ResolvedToken> tokens, FormulaEvaluationContext context)
        {
            _tokens = tokens;
            _context = context;
        }
        public ExpressionNode Parse()
        {
            return ParseExpression();
        }
        private ExpressionNode ParseExpression(int minPrecedence = 0)
        {
            var left = ParsePrimary();

            while (_position < _tokens.Count && IsOperator(_tokens[_position].Token.Value))
            {
                var op = _tokens[_position].Token.Value;
                var precedence = GetPrecedence(op);

                if (precedence < minPrecedence) break;

                _position++; // consume operator
                var right = ParseExpression(precedence + 1);
                left = new BinaryOperatorNode(op, left, right);
            }

            return left;
        }
        private ExpressionNode ParsePrimary()
        {
            if (_position >= _tokens.Count) return new ConstantNode(0, LineType.Decimal);

            var token = _tokens[_position];
            _position++;

            return token.Token.Type switch
            {
                FormulaTokenType.Constant => new ConstantNode(token.Value!, token.ValueType ?? LineType.Decimal),
                FormulaTokenType.ScdrubReference or FormulaTokenType.EnumReference =>
                    new ConstantNode(token.Value ?? 0, token.ValueType ?? LineType.Decimal),
                FormulaTokenType.Function => ParseFunction(token.Token.Value, _context),
                //FormulaTokenType.Function => ParseFunction(token.Token.Value),
                FormulaTokenType.Operator when token.Token.Value == "(" => ParseGrouped(),
                _ => new ConstantNode(0, LineType.Decimal)
            };
        }
        private ExpressionNode ParseFunction(string name, FormulaEvaluationContext context)
        {
            var args = new List<ExpressionNode>();

            if (_position < _tokens.Count && _tokens[_position].Token.Value == "(")
            {
                _position++; // consume '('
                while (_position < _tokens.Count && _tokens[_position].Token.Value != ")")
                {
                    var token = _tokens[_position];

                    // Detect range: i120 .. i158
                    if (token.Token.Type == FormulaTokenType.ScdrubReference &&
                        _position + 2 < _tokens.Count &&
                        _tokens[_position + 1].Token.Value == ".." &&
                        _tokens[_position + 2].Token.Type == FormulaTokenType.ScdrubReference)
                    {
                        var start = token.Token.Value;
                        var end = _tokens[_position + 2].Token.Value;
                        args.AddRange(ExpandRange(start, end, context));
                        _position += 3;
                    }
                    else
                    {
                        args.Add(ParseExpression());
                        if (_position < _tokens.Count && _tokens[_position].Token.Value == ";")
                            _position++; // consume separator
                    }
                }
                if (_position < _tokens.Count && _tokens[_position].Token.Value == ")")
                    _position++; // consume ')'
            }

            return new FunctionNode(name, args);
        }
        private ExpressionNode ParseGrouped()
        {
            var expr = ParseExpression();
            if (_position < _tokens.Count && _tokens[_position].Token.Value == ")")
                _position++; // consume closing parenthesis
            return expr;
        }
        private bool IsOperator(string value) => value is "+" or "-" or "*" or "/" or "=";
        private int GetPrecedence(string op) => op switch
        {
            "*" or "/" => 2,
            "+" or "-" => 1,
            "=" => 0,
            _ => -1
        };
        private List<ExpressionNode> ExpandRange(string start, string end, FormulaEvaluationContext context)
        {
            var nodes = new List<ExpressionNode>();

            var prefix = start.Substring(0, 1); // e.g. "i"
            var startNum = int.Parse(start.Substring(1));
            var endNum = int.Parse(end.Substring(1));

            for (int i = startNum; i <= endNum; i++)
            {
                var lineRef = $"{prefix}{i}";
                var formulaLine = context.GetFormulaLineByNumber(lineRef);

                if (formulaLine?.Result?.Value != null)
                {
                    nodes.Add(new ConstantNode(formulaLine.Result.Value, formulaLine.Result.Type ?? LineType.Decimal));
                }
            }

            return nodes;
        }
    }
}
//private ExpressionNode ParseFunction(string name)
//{
//    var args = new List<ExpressionNode>();

//    if (_position < _tokens.Count && _tokens[_position].Token.Value == "(")
//    {
//        _position++; // consume '('
//        while (_position < _tokens.Count && _tokens[_position].Token.Value != ")")
//        {
//            var token = _tokens[_position];

//            // Detect range: i120 .. i158
//            if (token.Token.Type == FormulaTokenType.ScdrubReference &&
//                _position + 2 < _tokens.Count &&
//                _tokens[_position + 1].Token.Value == ".." &&
//                _tokens[_position + 2].Token.Type == FormulaTokenType.ScdrubReference)
//            {
//                var start = token.Token.Value;
//                var end = _tokens[_position + 2].Token.Value;
//                args.AddRange(ExpandRange(start, end));
//                _position += 3;
//            }
//            else
//            {
//                args.Add(ParseExpression());
//                if (_position < _tokens.Count && _tokens[_position].Token.Value == ";")
//                    _position++; // consume separator
//            }
//        }
//        if (_position < _tokens.Count && _tokens[_position].Token.Value == ")")
//            _position++; // consume ')'
//    }

//    return new FunctionNode(name, args);
//}
//private ExpressionNode ParseFunction(string name)
//{
//    var args = new List<ExpressionNode>();

//    if (_position < _tokens.Count && _tokens[_position].Token.Value == "(")
//    {
//        _position++; // consume '('
//        while (_position < _tokens.Count && _tokens[_position].Token.Value != ")")
//        {
//            args.Add(ParseExpression());
//            if (_position < _tokens.Count && _tokens[_position].Token.Value == ";")
//                _position++; // consume separator
//        }
//        if (_position < _tokens.Count && _tokens[_position].Token.Value == ")")
//            _position++; // consume ')'
//    }

//    return new FunctionNode(name, args);
//}
//private List<ExpressionNode> ExpandRange(string start, string end)
//{
//    var nodes = new List<ExpressionNode>();

//    var prefix = start.Substring(0, 1); // e.g. "i"
//    var startNum = int.Parse(start.Substring(1));
//    var endNum = int.Parse(end.Substring(1));

//    for (int i = startNum; i <= endNum; i++)
//    {
//        var scdrub = $"{prefix}{i}";
//        var token = _tokens.FirstOrDefault(t => t.Token.Value == scdrub);
//        var value = token?.Value ?? 0;
//        var type = token?.ValueType ?? LineType.Decimal;

//        nodes.Add(new ConstantNode(value, type));
//    }

//    return nodes;
//}

