using GxFormula.ForaBizz;
using GxFormula.Forasource;

using GxShared.GxDtos;

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

            var resdonByLineNumber = new Dictionary<int, ResdonDto>();
            var resbroByLineNumber = new Dictionary<int, ResbroDto>();

            foreach (var line in lines)
            {
                if (IsCommentLine(line.Formula))
                    continue;

                var result = _engine.Evaluate(line.Formula, evalCtx);
                if (result is null)
                    continue;

                if (line.IsDetail && line.ParentLineNumber.HasValue)
                {
                    if (ctx.IsTestMode)
                    {
                        if (!resbroByLineNumber.TryGetValue(line.ParentLineNumber.Value, out var parentResbro))
                            throw new InvalidOperationException(
                                $"Detail line @{line.ParentLineNumber}#{line.DetailCode} has no parent line @{line.ParentLineNumber} evaluated in this program.");

                        var bdet = ResultMapper.MapToResbdet(ctx, line, result);
                        bdet.Prowguid = parentResbro.Rowguid;
                        bdet.Zcdrub = line.DetailCode;
                        session.Resbdets.Add(bdet);
                    }
                    else
                    {
                        if (!resdonByLineNumber.TryGetValue(line.ParentLineNumber.Value, out var parentResdon))
                            throw new InvalidOperationException(
                                $"Detail line @{line.ParentLineNumber}#{line.DetailCode} has no parent line @{line.ParentLineNumber} evaluated in this program.");

                        var det = ResultMapper.MapToResdet(ctx, line, result);
                        det.Prowguid = parentResdon.Rowguid;
                        det.Zcdrub = line.DetailCode;
                        session.Resdets.Add(det);
                    }
                }
                else
                {
                    session.Outputs[line.LineNumber ?? 0] = ResultMapper.MapToOutputStream(ctx, line, result);

                    if (ctx.IsTestMode)
                    {
                        var bro = ResultMapper.MapToResbro(ctx, line, result);
                        bro.Rowguid = Guid.NewGuid();
                        session.Resbros.Add(bro);
                        resbroByLineNumber[line.LineNumber ?? 0] = bro;
                    }
                    else
                    {
                        var don = ResultMapper.MapToResdon(ctx, line, result);
                        don.Rowguid = Guid.NewGuid();
                        session.Resdons.Add(don);
                        resdonByLineNumber[line.LineNumber ?? 0] = don;
                    }
                }
            }

            return Task.FromResult(session);
        }
        //public Task<CalcSession> RunCalcAsync(CalcContext ctx, CalcSession session)
        //{
        //    if (ctx.Program is null)
        //        throw new ArgumentNullException(nameof(ctx.Program));

        //    var evalCtx = BuildEvalContext(ctx);
        //    var lines = _parser.Parse(new PlngenLineSource(ctx.Program));

        //    foreach (var line in lines)
        //    {
        //        // Skip comment / label lines
        //        if (IsCommentLine(line.Formula))
        //            continue;

        //        var result = _engine.Evaluate(line.Formula, evalCtx);
        //        if (result is null)
        //            continue;

        //        session.Outputs[line.LineNumber ?? 0] =
        //            ResultMapper.MapToOutputStream(ctx, line, result);

        //        if (ctx.IsTestMode)
        //        {
        //            session.Resbros.Add(ResultMapper.MapToResbro(ctx, line, result));
        //        }
        //        else
        //        {
        //            session.Resdons.Add(ResultMapper.MapToResdon(ctx, line, result));
        //            if (line.SaveDetail)
        //                session.Resdets.Add(ResultMapper.MapToResdet(ctx, line, result));
        //        }
        //    }

        //    return Task.FromResult(session);
        //}

        private static bool IsCommentLine(string? formula)
        {
            if (string.IsNullOrWhiteSpace(formula))
                return true;

            var s = formula.Trim();

            // Treat lines starting with '//' or '#' as comments / non‑executable
            if (s.StartsWith("//") || s.StartsWith("#"))
                return true;

            return false;
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
    //public interface IProgramCalculator
    //{
    //    Task<CalcSession> RunCalcAsync(CalcContext ctx, CalcSession session);
    //}
    //internal sealed class ProgramCalculator : IProgramCalculator
    //{
    //    private readonly FormulaEngine _engine;
    //    private readonly IProgramLineParser _parser;

    //    public ProgramCalculator(FormulaEngine engine, IProgramLineParser parser)
    //    {
    //        _engine = engine;
    //        _parser = parser;
    //    }

    //    public Task<CalcSession> RunCalcAsync(CalcContext ctx, CalcSession session)
    //    {
    //        if (ctx.Program is null)
    //            throw new ArgumentNullException(nameof(ctx.Program));

    //        var evalCtx = BuildEvalContext(ctx);
    //        var lines = _parser.Parse(new PlngenLineSource(ctx.Program));

    //        foreach (var line in lines)
    //        {
    //            var result = _engine.Evaluate(line.Formula, evalCtx);
    //            if (result is null)
    //                continue;

    //            session.Outputs[line.LineNumber ?? 0] =
    //                ResultMapper.MapToOutputStream(ctx, line, result);

    //            if (ctx.IsTestMode)
    //            {
    //                session.Resbros.Add(ResultMapper.MapToResbro(ctx, line, result));
    //            }
    //            else
    //            {
    //                session.Resdons.Add(ResultMapper.MapToResdon(ctx, line, result));
    //                if (line.SaveDetail)
    //                    session.Resdets.Add(ResultMapper.MapToResdet(ctx, line, result));
    //            }
    //        }

    //        return Task.FromResult(session);
    //    }

    //    private FormulaEvaluationContext BuildEvalContext(CalcContext ctx)
    //        => new()
    //        {
    //            Idorg = ctx.Idorg,
    //            Ipln = ctx.Ipln,
    //            Itie = ctx.Itie,
    //            SessionDate = ctx.SessionDate ?? DateTime.Today,
    //            Tier = ctx.Tier,
    //            Actsaies = ctx.Actsaies,
    //            Actdets = ctx.Actdets,
    //            EnsTbls = ctx.EnsTbls
    //        };
    //}
}
