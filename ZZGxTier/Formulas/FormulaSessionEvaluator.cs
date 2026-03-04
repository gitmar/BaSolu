namespace GxSaie.Formulas.ReviewConcepts
{
    public class FormulaSession
    {
        public List<FormulaLine> Lines { get; set; } = new();
        public List<InDataLineStream> Inputs { get; set; } = new();
        public List<OutDataLineStream> Outputs { get; set; } = new();
        public DateTime SessionDate { get; set; } = DateTime.Today;
        public long Idtie { get; set; }
    }
    public class FormulaSessionEvaluator
    {
        private readonly FormulaSession _session;

        public FormulaSessionEvaluator(FormulaSession session)
        {
            _session = session;
        }

        public void EvaluateAll()
        {
            var context = new FormulaEvaluationContext
            {
                Idtie = Convert.ToInt32(_session.Idtie),
                InputData = _session.Inputs,
                PreviousLines = _session.Lines,
                SessionDate = _session.SessionDate
            };

            var registry = new FormulaLineRegistry(_session.Lines);
            var evaluator = new FormulaLineEvaluator(context, registry);

            foreach (var line in _session.Lines.Where(l => !l.IsClientOnly))
            {
                var outcome = evaluator.EvaluateWithLog(line);

                _session.Outputs.Add(new OutDataLineStream
                {
                    Idtie = Convert.ToInt32(_session.Idtie),
                    Scdrub = line.LineNumber,
                    Result = outcome.Result,
                    Trust = outcome.Result.Trust,
                    Ddeb = _session.SessionDate,
                    Dfin = _session.SessionDate
                });
            }
        }
    }
}
