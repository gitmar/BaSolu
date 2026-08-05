using GxFormula.ForaBizz;
using GxFormula.Forasource;

using GxShared.GxDtos;
using GxShared.Sess;

using GxTie.StaticHelpers;

namespace GxTie.Services.Calculation
{
    public interface ISaieCalculator
    {
        Task<SaieSession> InitializeAsync(PlngenDto program, TierspDto tier, List<Gtabl>? ensTbls = null);
        Task<SaieSession> CalculateAsync(CalcContext ctx, SaieSession session);
    }
    internal sealed class SaieCalculator : ISaieCalculator
    {
        private readonly FormulaEngine _engine;
        private readonly IProgramLineParser _parser;

        public SaieCalculator(FormulaEngine engine, IProgramLineParser parser)
        {
            _engine = engine;
            _parser = parser;
        }

        public Task<SaieSession> InitializeAsync(
            PlngenDto program, TierspDto tier, List<Gtabl>? ensTbls = null)
        {
            var session = new SaieSession
            {
                Program = program,
                Tier = tier,
                EnsTbls = ensTbls ?? new List<Gtabl>(),
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
                        Iraw = string.Empty
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
                        Iraw = string.Empty
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

            var evalContext = BuildEvalContext(ctx, session);

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

        private FormulaEvaluationContext BuildEvalContext(CalcContext ctx, SaieSession session)
    => new()
    {
        Idorg = ctx.Idorg,
        Ipln = ctx.Ipln,
        Itie = ctx.Itie,
        Tier = ctx.Tier,
        Session = session, // <-- new
        Actsaies = ctx.Actsaies,
        Actdets = ctx.Actdets,
        EnsTbls = ctx.EnsTbls
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
                }))
                .ToList();

        private ActsaieDto EvaluateRubvarRow(
    FormulaEvaluationContext ctx, RubvarDto rub, List<ActdetDto> dets)
        {
            // Find the corresponding RubVarRow to get InputValue
            var row = ((SaieSession)ctx.Session!).RubVarRows
                .FirstOrDefault(r => r.Irub == rub.Id);

            var inputValue = decimal.TryParse(row?.InputValue, out var v) ? v : 0m;

            // If no formula, just use InputValue
            if (string.IsNullOrWhiteSpace(rub.Frsrc))
            {
                return new ActsaieDto
                {
                    Itie = ctx.Itie ?? 0,
                    Ipln = ctx.Ipln ?? 0,
                    Irub = rub.Id,
                    Atyp = rub.Atyp,
                    Inptvalue = row?.InputValue ?? string.Empty,
                    Aval = inputValue.ToString(),
                    Iraw = MyConverters.Trunc1000(inputValue)?.ToString() ?? string.Empty,
                    Actdets = dets
                };
            }

            // Otherwise, evaluate the formula
            ctx.CurrentInputValue = inputValue;
            var result = _engine.Evaluate(rub.Frsrc, ctx);

            return new ActsaieDto
            {
                Itie = ctx.Itie ?? 0,
                Ipln = ctx.Ipln ?? 0,
                Irub = rub.Id,
                Atyp = rub.Atyp,
                Inptvalue = result?.Value?.ToString() ?? row?.InputValue ?? string.Empty,
                Aval = string.IsNullOrEmpty(rub.Frsrc)
                    ? string.Empty
                    : result?.Raw?.ToString() ?? string.Empty,
                Iraw = MyConverters.Trunc1000(result?.Raw)?.ToString() ?? string.Empty,
                Actdets = dets
            };
        }

        private List<ActdetDto> EvaluateRubfmtRows(
    FormulaEvaluationContext ctx, RubvarDto rub, List<RubFmtRow> fmtRows)
    => fmtRows.Select(r =>
    {
        var inputValue = decimal.TryParse(r.InputValue, out var v) ? v : 0m;

        // If no formula, just use InputValue
        if (string.IsNullOrWhiteSpace(r.Ftsrc))
        {
            return new ActdetDto
            {
                Itie = ctx.Itie ?? 0,
                Ipln = ctx.Ipln ?? 0,
                Irub = rub.Id,
                Ifmt = r.Ifmt,
                Atyp = r.Atyp,
                Vgpe = r.Vgpe,
                Inptvalue = r.InputValue ?? string.Empty,
                Aval = inputValue.ToString(),
                Iraw = MyConverters.Trunc1000(inputValue)?.ToString() ?? string.Empty
            };
        }

        // Otherwise, evaluate the formula
        ctx.CurrentInputValue = inputValue;
        var result = _engine.Evaluate(r.Ftsrc, ctx);

        return new ActdetDto
        {
            Itie = ctx.Itie ?? 0,
            Ipln = ctx.Ipln ?? 0,
            Irub = rub.Id,
            Ifmt = r.Ifmt,
            Atyp = r.Atyp,
            Vgpe = r.Vgpe,
            Inptvalue = result?.Value?.ToString() ?? r.InputValue ?? string.Empty,
            Aval = string.IsNullOrEmpty(r.Ftsrc)
                ? string.Empty
                : result?.Raw?.ToString() ?? string.Empty,
            Iraw = MyConverters.Trunc1000(result?.Raw)?.ToString() ?? string.Empty
        };
    }).ToList();
    }
    //private ActsaieDto EvaluateRubvarRow(
    //        FormulaEvaluationContext ctx, RubvarDto rub, List<ActdetDto> dets)
    //    {
    //        var result = _engine.Evaluate(rub.Frsrc, ctx);

    //        return new ActsaieDto
    //        {
    //            Itie = ctx.Itie,
    //            Ipln = ctx.Ipln ?? 0,
    //            Irub = rub.Id,
    //            Atyp = rub.Atyp,
    //            Inptvalue = result?.Value?.ToString(),
    //            Frsrc = string.IsNullOrEmpty(rub?.Frsrc)
    //                ? string.Empty
    //                : result?.Raw?.ToString(),
    //            Aval = result.Value.ToString(),
    //            Iraw = MyConverters.Trunc1000(result?.Raw)?.ToString() ?? string.Empty,
    //            Actdets = dets
    //        };
    //    }
    //private List<ActdetDto> EvaluateRubfmtRows(
    //        FormulaEvaluationContext ctx, RubvarDto rub, List<RubFmtRow> fmtRows)
    //        => fmtRows.Select(r =>
    //        {
    //            var result = _engine.Evaluate(r.Ftsrc, ctx);

    //            return new ActdetDto
    //            {
    //                Itie = ctx.Itie ?? 0,
    //                Ipln = ctx.Ipln ?? 0,
    //                Irub = rub.Id,
    //                Ifmt = r.Ifmt,
    //                Atyp = r.Atyp,
    //                Vgpe = r.Vgpe,
    //                Inptvalue = result?.Value?.ToString(),
    //                Ftsrc = string.IsNullOrEmpty(rub?.Frsrc)
    //                    ? string.Empty
    //                    : result?.Raw?.ToString(),
    //                Aval = result.Value.ToString(),
    //                Iraw = MyConverters.Trunc1000(result?.Raw)?.ToString() ?? string.Empty
    //            };
    //        }).ToList();
    //}
}