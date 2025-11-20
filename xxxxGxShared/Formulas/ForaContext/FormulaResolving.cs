using GxShared.Formulas.ForaModels;
namespace GxShared.Formulas.ForaContext
{
    public class FormulaSemanticResolver
    {
        private readonly List<FormulaToken> _tokens;
        private readonly List<InDataLineStream> _inputData;
        private readonly Dictionary<string, TiersProp> _tiersMap;

        public FormulaSemanticResolver(List<FormulaToken> tokens, List<InDataLineStream> inputData)
        {
            _tokens = tokens;
            _inputData = inputData;
            _tiersMap = Enum.GetValues(typeof(TiersProp))
                            .Cast<TiersProp>()
                            .ToDictionary(e => $"p{(int)e}", e => e);
        }

        public List<ResolvedToken> Resolve()
        {
            var resolved = new List<ResolvedToken>();

            foreach (var token in _tokens)
            {
                switch (token.Type)
                {
                    case FormulaTokenType.ScdrubReference:
                        var match = _inputData.FirstOrDefault(d => d.Scdrub == token.Value);
                        if (match != null)
                        {
                            resolved.Add(new ResolvedToken(token, match, match.Vtyp, match.Result?.Value, match.Trust));
                        }
                        else
                        {
                            resolved.Add(new ResolvedToken(token, null, null, null, TrustLevel.Suspended));
                        }
                        break;

                    case FormulaTokenType.EnumReference:
                        if (_tiersMap.TryGetValue(token.Value, out var prop))
                        {
                            resolved.Add(new ResolvedToken(token, prop, LineType.String, $"Enum:{prop}", TrustLevel.Valid));
                        }
                        else
                        {
                            resolved.Add(new ResolvedToken(token, null, null, null, TrustLevel.Suspended));
                        }
                        break;

                    default:
                        resolved.Add(new ResolvedToken(token, null)); // passthrough
                        break;
                }
            }

            return resolved;
        }
    }
    //public class FormulaSemanticResolver
    //{
    //    private readonly List<FormulaToken> _tokens;
    //    private readonly List<InDataLineStream> _inputData;
    //    private readonly Dictionary<string, TiersProp> _tiersMap;

    //    public FormulaSemanticResolver(List<FormulaToken> tokens, List<InDataLineStream> inputData)
    //    {
    //        _tokens = tokens;
    //        _inputData = inputData;
    //        _tiersMap = Enum.GetValues(typeof(TiersProp))
    //                        .Cast<TiersProp>()
    //                        .ToDictionary(e => $"p{(int)e}", e => e);
    //    }

    //    public List<ResolvedToken> Resolve()
    //    {
    //        var resolved = new List<ResolvedToken>();

    //        foreach (var token in _tokens)
    //        {
    //            switch (token.Type)
    //            {
    //                case FormulaTokenType.ScdrubReference:
    //                    var match = _inputData.FirstOrDefault(d => d.Scdrub == token.Value);
    //                    resolved.Add(new ResolvedToken(token, match));
    //                    break;

    //                case FormulaTokenType.EnumReference:
    //                    if (_tiersMap.TryGetValue(token.Value, out var prop))
    //                        resolved.Add(new ResolvedToken(token, prop));
    //                    else
    //                        resolved.Add(new ResolvedToken(token, null)); // unresolved
    //                    break;

    //                default:
    //                    resolved.Add(new ResolvedToken(token, null)); // passthrough
    //                    break;
    //            }
    //        }

    //        return resolved;
    //    }
    //}
    public class ResolvedToken
    {
        public FormulaToken Token { get; }
        public object? Binding { get; } // Can be InDataLineStream or TiersProp
        public LineType? ValueType { get; }
        public object? Value { get; }
        public TrustLevel Trust { get; }

        public ResolvedToken(FormulaToken token, object? binding, LineType? valueType = null, object? value = null, TrustLevel trust = TrustLevel.Valid)
        {
            Token = token;
            Binding = binding;
            ValueType = valueType;
            Value = value;
            Trust = trust;
        }

        public bool IsResolved => Binding != null;
    }
    //public class ResolvedToken
    //{
    //    public FormulaToken Token { get; }
    //    public object? Binding { get; } // Can be InDataLineStream or TiersProp

    //    public ResolvedToken(FormulaToken token, object? binding)
    //    {
    //        Token = token;
    //        Binding = binding;
    //    }

    //    public bool IsResolved => Binding != null;
    //}
}
