using GxFormula.ForaBizz;
using GxFormula.Forasource;

namespace GxTie.Services.Calculation
{
    public interface IProgramCalculator
    {
        Task<CalcSession> RunCalcAsync(CalcContext ctx, CalcSession session);
    }
    internal sealed class ProgramCalculator : IProgramCalculator
    {
        private readonly FormulaEngine _engine;
        private readonly IProgramLineParser _parser;

        public ProgramCalculator(FormulaEngine engine, IProgramLineParser parser)
        {
            _engine = engine;
            _parser = parser;
        }

        public Task<CalcSession> RunCalcAsync(CalcContext ctx, CalcSession session)
        {
            if (ctx.Program is null)
                throw new ArgumentNullException(nameof(ctx.Program));

            var evalCtx = BuildEvalContext(ctx);
            var lines = _parser.Parse(new PlngenLineSource(ctx.Program));

            foreach (var line in lines)
            {
                var result = _engine.Evaluate(line.Formula, evalCtx);
                if (result is null)
                    continue;

                session.Outputs[line.LineNumber ?? 0] =
                    ResultMapper.MapToOutputStream(ctx, line, result);

                if (ctx.IsTestMode)
                {
                    session.Resbros.Add(ResultMapper.MapToResbro(ctx, line, result));
                }
                else
                {
                    session.Resdons.Add(ResultMapper.MapToResdon(ctx, line, result));
                    if (line.SaveDetail)
                        session.Resdets.Add(ResultMapper.MapToResdet(ctx, line, result));
                }
            }

            return Task.FromResult(session);
        }

        private FormulaEvaluationContext BuildEvalContext(CalcContext ctx)
            => new()
            {
                Idorg = ctx.Idorg,
                Ipln = ctx.Ipln,
                Itie = ctx.Itie,
                SessionDate = ctx.SessionDate ?? DateTime.Today,
                Tier = ctx.Tier,
                Actsaies = ctx.Actsaies,
                Actdets = ctx.Actdets,
                EnsTbls = ctx.EnsTbls
            };
    }
}
