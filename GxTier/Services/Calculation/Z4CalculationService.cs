using System.Collections.Generic;

using GxFormula.ForaBizz;
using GxFormula.Forasource;

using GxShared.GxDtos;

using GxTie.StaticHelpers;

namespace GxTie.Services.Calculation
{
    //public interface ICalculationService
    //{
    //    Task<SaieSession> CalculateSaieAsync(CalcContext ctx, SaieSession session);
    //    Task<CalcSession> RunCalcAsync(CalcContext ctx, CalcSession session);
    //}

    public sealed class CalculationService : ICalculationService
    {
        private readonly ISaieCalculator _saieCalculator;
        private readonly IProgramCalculator _programCalculator;

        public CalculationService(
            ISaieCalculator saieCalculator,
            IProgramCalculator programCalculator)
        {
            _saieCalculator = saieCalculator;
            _programCalculator = programCalculator;
        }

        public Task<SaieSession> CalculateSaieAsync(CalcContext ctx, SaieSession session)
            => _saieCalculator.CalculateAsync(ctx, session);

        public Task<CalcSession> RunCalcAsync(CalcContext ctx, CalcSession session)
            => _programCalculator.RunCalcAsync(ctx, session);
    }
    //public interface ISaieCalculator
    //{
    //    Task<SaieSession> InitializeAsync(PlngenDto program, TierspDto tier);
    //    Task<SaieSession> CalculateAsync(CalcContext ctx, SaieSession session);
    //}

    internal sealed class SaieCalculator : ISaieCalculator
    {
        private readonly FormulaEngine _engine;
        private readonly IProgramLineParser _parser;

        public SaieCalculator(FormulaEngine engine, IProgramLineParser parser)
        {
            _engine = engine;
            _parser = parser;
        }

        public Task<SaieSession> InitializeAsync(PlngenDto program, TierspDto tier)
        {
            var session = new SaieSession
            {
                Program = program,
                Tier = tier,
                RubVarRows = program.Rubvars.Select(v => new RubVarRow
                {
                    Id = v.Id,
                    Irub = v.Id,
                    Scdrub = v.Scdrub,
                    Atyp = v.Atyp ?? 0,
                    Vgpe = v.Vgpe,
                    Liba = v.Liba,
                    Abg = v.Liba,
                    InputValue = string.Empty,
                    Iraw = string.Empty,
                    ////SourceRubvar = v,
                    Details = v.Rubfmts.Select(f => new RubFmtRow
                    {
                        Id = f.Id,
                        Irub = v.Id,
                        Ifmt = f.Id,
                        Scdfmt = f.Zcdrub,
                        Atyp = f.Ztyp ?? 0,
                        Vgpe = f.Vgpe,
                        Liba = f.Liba,
                        Ftsrc = f.Ftsrc,
                        InputValue = string.Empty,
                        Iraw = string.Empty,
                        ////SourceRubfmt = f
                    }).ToList()
                }).ToList(),
                RubFmtRows = program.Rubvars
                    .SelectMany(v => v.Rubfmts.Select(f => new RubFmtRow
                    {
                        Id = f.Id,
                        Irub = v.Id,
                        Ifmt = f.Id,
                        Scdfmt = f.Zcdrub,
                        Atyp = f.Ztyp ?? 0,
                        Vgpe = f.Vgpe,
                        Liba = f.Liba,
                        Ftsrc = f.Ftsrc,
                        InputValue = string.Empty,
                        Iraw = string.Empty,
                        ////SourceRubfmt = f
                    }))
                    .ToList()
            };

            return Task.FromResult(session);
        }
        public Task<SaieSession> CalculateAsync(CalcContext ctx, SaieSession session)
        {
            if (ctx.Program is null)
                throw new ArgumentNullException(nameof(ctx.Program));

            session.Program = ctx.Program;
            session.Tier = ctx.Tier;
            session.RubVarRows ??= BuildRubVarRows(ctx.Program);
            session.RubFmtRows ??= BuildRubFmtRows(ctx.Program);

            session.Actsaies.Clear();
            session.Actdets.Clear();

            var evalContext = BuildEvalContext(ctx);

            foreach (var rub in ctx.Program.Rubvars)
            {
                var row = session.RubVarRows.FirstOrDefault(x => x.Irub == rub.Id);
                if (row is null)
                    continue;

                var fmtRows = row.Details ?? new List<RubFmtRow>();
                var dets = EvaluateRubfmtRows(evalContext, rub, fmtRows);

                session.Actdets.AddRange(dets);

                var act = EvaluateRubvarRow(evalContext, rub, dets);
                session.Actsaies.Add(act);

                row.Aval = act.Aval;
                row.Iraw = act.Iraw;
            }

            return Task.FromResult(session);
        }

        private FormulaEvaluationContext BuildEvalContext(CalcContext ctx)
            => new()
            {
                Idorg = ctx.Idorg,
                Ipln = ctx.Ipln,
                Itie = ctx.Itie,
                //SessionDate = ctx.Date,
                Tier = ctx.Tier,
                //Actsaies = ctx.Actsaies,
                //Actdets = ctx.Actdets
            };

        private List<RubVarRow> BuildRubVarRows(PlngenDto program)
            => program.Rubvars.Select(v => new RubVarRow
            {
                Id = v.Id,
                Irub = v.Id,
                Scdrub = v.Scdrub,
                Atyp = v.Atyp ?? 0,
                Vgpe = v.Vgpe,
                Frscr = v.Frsrc,
                Liba = v.Liba,
                Abg = v.Liba,
                InputValue = string.Empty,
                Iraw = string.Empty,
                ////SourceRubvar = v,
                Details = v.Rubfmts.Select(f => new RubFmtRow
                {
                    Id = f.Id,
                    Irub = v.Id,
                    Ifmt = f.Id,
                    Scdfmt = f.Zcdrub,
                    Atyp = f.Ztyp ?? 0,
                    Vgpe = f.Vgpe,
                    Ftsrc = f.Ftsrc,
                    Liba = f.Liba,
                    InputValue = string.Empty,
                    Iraw = string.Empty
                    ////SourceRubfmt = f
                }).ToList()
            }).ToList();

        private List<RubFmtRow> BuildRubFmtRows(PlngenDto program)
            => program.Rubvars
                .SelectMany(v => v.Rubfmts.Select(f => new RubFmtRow
                {
                    Id = f.Id,
                    Irub = v.Id,
                    Ifmt = f.Id,
                    Scdfmt = f.Zcdrub,
                    Atyp = f.Ztyp ?? 0,
                    Vgpe = f.Vgpe,
                    Liba = f.Liba,
                    Ftsrc = f.Ftsrc,
                    InputValue = string.Empty,
                    Iraw = string.Empty
                    ////SourceRubfmt = f
                }))
                .ToList();

        private ActsaieDto EvaluateRubvarRow(FormulaEvaluationContext ctx, RubvarDto rub, List<ActdetDto> dets)
        {
            var result = _engine.Evaluate(rub.Frsrc, ctx);
            return new ActsaieDto
            {
                Itie = ctx.Itie,
                Ipln = ctx.Ipln ?? 0,
                Irub = rub.Id,
                Atyp = rub.Atyp,
                Inptvalue = result?.Value?.ToString(),
                Aval = string.IsNullOrEmpty(rub?.Frsrc)
                    ? string.Empty : result?.Raw?.ToString(),
                Iraw = MyConverters.Trunc1000(result?.Raw)?.ToString() ?? string.Empty,
                Actdets = dets
            };
        }

        private List<ActdetDto> EvaluateRubfmtRows(FormulaEvaluationContext ctx, RubvarDto rub, List<RubFmtRow> fmtRows)
            => fmtRows.Select(r =>
            {
                var result = _engine.Evaluate(r.Ftsrc, ctx);

                return new ActdetDto
                {
                    Itie = ctx.Itie ?? 0,
                    Ipln = ctx.Ipln ?? 0,
                    Irub = rub.Id,
                    Ifmt = r.Ifmt,
                    Atyp = r.Atyp,
                    Vgpe = r.Vgpe,
                    //Sesperi = ctx.SessionDate,
                    Inptvalue = result?.Value?.ToString(),
                    Aval = string.IsNullOrEmpty(rub?.Frsrc)
                    ? string.Empty : result?.Raw?.ToString(),
                    Iraw = MyConverters.Trunc1000(result?.Raw)?.ToString() ?? string.Empty
                };
            }).ToList();
    }

    //public interface IProgramCalculator
    //{
    //    Task<CalcSession> RunCalcAsync(CalcContext ctx, CalcSession session);
    //}

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
            var evalCtx = BuildEvalContext(ctx);
            var lines = _parser.Parse(new PlngenLineSource(ctx.Program));

            foreach (var line in lines)
            {
                var result = _engine.Evaluate(line.Formula, evalCtx);
                if (result is null)
                    continue;

                session.Outputs[line.LineNumber ?? 0] = ResultMapper.MapToOutputStream(ctx, line, result);

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
    internal static class ResultMapper
    {
        public static OutDataLineStream MapToOutputStream(CalcContext ctx, ProgramLine line, FormulaResult result)
            => new()
            {
                Itie = ctx.Itie,
                Ipln = ctx.Ipln,
                Irub = line.Irub ?? 0,
                Atyp = result.Type.HasValue ? (int)result.Type.Value : 0,
                Inptvalue = result.Value?.ToString(),
                Aval = result.Raw?.ToString(),
                Iraw = MyConverters.Trunc1000(result.Raw)?.ToString() ?? string.Empty
            };

        public static ResdonDto MapToResdon(CalcContext ctx, ProgramLine line, FormulaResult result)
            => new()
            {
                Itie = ctx.Itie,
                Ipln = ctx.Ipln,
                Irub = line.Irub ?? 0,
                Atyp = result.Type.HasValue ? (int)result.Type.Value : 0,
                Inptvalue = result.Value?.ToString(),
                Aval = result.Raw?.ToString(),
                Iraw = MyConverters.Trunc1000(result.Raw)?.ToString() ?? string.Empty
            };

        public static ResbroDto MapToResbro(CalcContext ctx, ProgramLine line, FormulaResult result)
            => new()
            {
                Itie = ctx.Itie,
                Ipln = ctx.Ipln,
                Irub = line.Irub ?? 0,
                Atyp = result.Type.HasValue ? (int)result.Type.Value : 0,
                Inptvalue = result.Value?.ToString(),
                Aval = result.Raw?.ToString(),
                Iraw = MyConverters.Trunc1000(result.Raw)?.ToString() ?? string.Empty
            };

        public static ResdetDto MapToResdet(CalcContext ctx, ProgramLine line, FormulaResult result)
            => new()
            {
                Itie = ctx.Itie,
                Ires = 0,
                Ipln = ctx.Ipln,
                Irub = line.Irub ?? 0,
                Ifmt = line.Ifmt ?? 0,
                Atyp = result.Type.HasValue ? (int)result.Type.Value : 0,
                Inptvalue = result.Value?.ToString(),
                Aval = result.Raw?.ToString(),
                Iraw = MyConverters.Trunc1000(result.Raw)?.ToString() ?? string.Empty
            };
    }
}
//
//public async Task<SaieSession> CalculateAsync(CalcContext ctx, SaieSession session)
//{
//    if (ctx.Program is null)
//        throw new ArgumentNullException(nameof(ctx.Program));

//    session.Program = ctx.Program;
//    session.Tier = ctx.Tier;
//    session.RubVarRows ??= BuildRubVarRows(ctx.Program);
//    session.RubFmtRows ??= BuildRubFmtRows(ctx.Program);

//    session.Actsaies.Clear();
//    session.Actdets.Clear();

//    var evalContext = BuildEvalContext(ctx);

//    foreach (var rub in ctx.Program.Rubvars)
//    {
//        var row = session.RubVarRows.FirstOrDefault(x => x.Irub == rub.Id);
//        if (row is null)
//            continue;

//        var fmtRows = row.Details ?? new List<RubFmtRow>();
//        var dets = EvaluateRubFmtRows(evalContext, rub, fmtRows);

//        session.Actdets.AddRange(dets);

//        var act = EvaluateRubVarRow(evalContext, rub, dets);
//        session.Actsaies.Add(act);

//        row.Aval = act.Aval;
//        row.Iraw = act.Iraw;
//    }

//    return Task.FromResult(session);
//}
