using GxShared.Formulas.ForaModels;
namespace GxShared.Formulas.ForaContext
{
    public class FormulaOutcomeEvaluator
    {
        private readonly FormulaEvaluationContext _context;
        private readonly FormulaLineRegistry _registry;
        private readonly FormulaLineEvaluator _evaluator;

        public FormulaOutcomeEvaluator(FormulaEvaluationContext context)
        {
            _context = context;
            _registry = new FormulaLineRegistry(context.PreviousLines);
            _evaluator = new FormulaLineEvaluator(context, _registry);
        }

        public List<OutDataLineStream> EvaluateAll()
        {
            var outputs = new List<OutDataLineStream>();

            foreach (var line in _context.PreviousLines.Where(l => !l.IsClientOnly))
            {
                var outcome = _evaluator.EvaluateWithLog(line);

                var output = new OutDataLineStream
                {
                    Ptie = _context.Idtie,
                    Scdrub = line.LineNumber,
                    Etyp = TableType.Rubvar,
                    Result = outcome.Result,
                    Trust = outcome.Result.Trust,
                    Ddeb = _context.SessionDate,
                    Dfin = _context.SessionDate,
                    Liba = "", // line.Liba,
                    Abg = "", //line.Abg,
                    Nom = "", //line.Nom,
                    Pnom = "", //line.Pnom,
                    Smatri = "" //line.Smatri
                };

                outputs.Add(output);
            }

            return outputs;
        }
    }
}

//Usage :
//var context = new FormulaEvaluationContext
//{
//    Idtie = 12345,
//    InputData = inputData,
//    PreviousLines = formulaLines,
//    SessionDate = DateTime.Today
//};

//var outcomeEvaluator = new FormulaOutcomeEvaluator(context);
//var results = outcomeEvaluator.EvaluateAll();