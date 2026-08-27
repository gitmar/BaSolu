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
                var det = EvaluateRubfmtRows(evalContext, rub, fmtRows);

                session.Actdets.AddRange(det);

                var act = EvaluateRubvarRow(evalContext, rub, det);
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
        FormulaEvaluationContext ctx,
        RubvarDto rub,
        List<ActdetDto> dets)
        {
            // Find the corresponding RubVarRow to get InputValue
            var row = ((SaieSession)ctx.Session!).RubVarRows
                .FirstOrDefault(r => r.Irub == rub.Id);

            var inputValue = decimal.TryParse(row?.InputValue, out var v) ? v : 0m;

            // If Atyp = 1 (string), treat as non‑calculable: Aval = InputValue
            if (rub.Atyp == 1)
            {
                return new ActsaieDto
                {
                    Itie = ctx.Itie ?? 0,
                    Ipln = ctx.Ipln ?? 0,
                    Irub = rub.Id,
                    Atyp = rub.Atyp,
                    Inptvalue = row?.InputValue ?? string.Empty,
                    Aval = row?.InputValue ?? string.Empty,
                    Iraw = string.Empty,
                    Actdets = dets
                };
            }

            // If no formula or it's a label/note (starts with '#'), just use InputValue
            if (IsPassthroughFormula(rub.Frsrc))
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

            // Prepare context for formula evaluation
            ctx.CurrentInputValue = inputValue;

            // Decide whether this is a multi-line program or a simple formula
            FormulaResult result;

            if (IsMultiLineProgram(rub.Frsrc))
            {
                result = EvaluateMultiLine(rub.Frsrc, ctx);
            }
            else
            {
                result = _engine.Evaluate(rub.Frsrc, ctx);
            }

            // Build the ActsaieDto from the evaluation result
            var aval = result?.Value?.ToString() ?? string.Empty;
            var raw = result?.Raw?.ToString();

            return new ActsaieDto
            {
                Itie = ctx.Itie ?? 0,
                Ipln = ctx.Ipln ?? 0,
                Irub = rub.Id,
                Atyp = rub.Atyp,
                Inptvalue = row?.InputValue ?? string.Empty,
                Aval = aval,
                Iraw = MyConverters.Trunc1000(raw)?.ToString() ?? string.Empty,
                Actdets = dets
            };
        }

        private bool IsMultiLineProgram(string frsrc)
        {
            if (string.IsNullOrWhiteSpace(frsrc))
                return false;

            // Simple heuristic: contains both '@' (alias lines) and '=' (final formula line)
            return frsrc.Contains('@') && frsrc.Contains('=');
        }

        private FormulaResult EvaluateMultiLine(string program, FormulaEvaluationContext evalContext)
        {
            var lines = program
                .Split('\n')
                .Select(l => l.Trim())
                .Where(l => !string.IsNullOrEmpty(l))
                .ToList();

            FormulaResult? finalResult = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("@"))
                {
                    // @I120: 150+INP;
                    var colonIdx = line.IndexOf(':');
                    if (colonIdx < 0)
                        throw new FormatException($"Invalid alias line: {line}");

                    var aliasPart = line[1..colonIdx].Trim();      // "I120"
                    var formulaPart = line[(colonIdx + 1)..].Trim(); // "150+INP;"

                    if (formulaPart.EndsWith(";"))
                        formulaPart = formulaPart[..^1].Trim();

                    var result = _engine.Evaluate(formulaPart, evalContext);
                    evalContext.Locals[aliasPart] = new ConstantNode(result.Value, LineType.Decimal);
                }
                else if (line.StartsWith("="))
                {
                    // = I120+I135;
                    var formulaPart = line[1..].Trim();
                    if (formulaPart.EndsWith(";"))
                        formulaPart = formulaPart[..^1].Trim();

                    finalResult = _engine.Evaluate(formulaPart, evalContext);
                }
                else
                {
                    // Legacy single-line formulas without '@' or '='
                    if (finalResult is null)
                    {
                        var formulaPart = line;
                        if (formulaPart.EndsWith(";"))
                            formulaPart = formulaPart[..^1].Trim();

                        finalResult = _engine.Evaluate(formulaPart, evalContext);
                    }
                }
            }

            return finalResult ?? FormulaResult.Empty;
        }

        private static bool IsPassthroughFormula(string? frsrc)
        {
            if (string.IsNullOrWhiteSpace(frsrc))
                return true;

            var s = frsrc.Trim();

            // Convention: lines starting with '#' are labels/notes → passthrough
            if (s.StartsWith("#"))
                return true;

            return false;
        }

        private List<ActdetDto> EvaluateRubfmtRows(
            FormulaEvaluationContext ctx,
            RubvarDto rub,
            List<RubFmtRow> fmtRows)
            => fmtRows.Select(r =>
            {
                var inputValue = decimal.TryParse(r.InputValue, out var v) ? v : 0m;

                // If Atyp = 1 (string), treat as non‑calculable: Aval = InputValue
                if (r.Atyp == 1)
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
                        Aval = r.InputValue ?? string.Empty,
                        Iraw = string.Empty
                    };
                }

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
        ////    private ActsaieDto EvaluateRubvarRow(
        ////    FormulaEvaluationContext ctx,
        ////    RubvarDto rub,
        ////    List<ActdetDto> dets)
        ////    {
        ////        // Find the corresponding RubVarRow to get InputValue
        ////        var row = ((SaieSession)ctx.Session!).RubVarRows
        ////            .FirstOrDefault(r => r.Irub == rub.Id);

        ////        var inputValue = decimal.TryParse(row?.InputValue, out var v) ? v : 0m;

        ////        // If no formula or it's a label/note (starts with '#'), just use InputValue
        ////        if (IsPassthroughFormula(rub.Frsrc))
        ////        {
        ////            return new ActsaieDto
        ////            {
        ////                Itie = ctx.Itie ?? 0,
        ////                Ipln = ctx.Ipln ?? 0,
        ////                Irub = rub.Id,
        ////                Atyp = rub.Atyp,
        ////                Inptvalue = row?.InputValue ?? string.Empty,
        ////                Aval = inputValue.ToString(),
        ////                Iraw = MyConverters.Trunc1000(inputValue)?.ToString() ?? string.Empty,
        ////                Actdets = dets
        ////            };
        ////        }

        ////        // Prepare context for formula evaluation
        ////        ctx.CurrentInputValue = inputValue;

        ////        // Decide whether this is a multi-line program or a simple formula
        ////        FormulaResult result;

        ////        if (IsMultiLineProgram(rub.Frsrc))
        ////        {
        ////            result = EvaluateMultiLine(rub.Frsrc, ctx);
        ////        }
        ////        else
        ////        {
        ////            result = _engine.Evaluate(rub.Frsrc, ctx);
        ////        }

        ////        // Build the ActsaieDto from the evaluation result
        ////        var aval = result?.Value?.ToString() ?? string.Empty;
        ////        var raw = result?.Raw?.ToString();

        ////        return new ActsaieDto
        ////        {
        ////            Itie = ctx.Itie ?? 0,
        ////            Ipln = ctx.Ipln ?? 0,
        ////            Irub = rub.Id,
        ////            Atyp = rub.Atyp,
        ////            Inptvalue = row?.InputValue ?? string.Empty,
        ////            Aval = aval,
        ////            Iraw = MyConverters.Trunc1000(raw)?.ToString() ?? string.Empty,
        ////            Actdets = dets
        ////        };
        ////    }

        ////    private bool IsMultiLineProgram(string frsrc)
        ////    {
        ////        if (string.IsNullOrWhiteSpace(frsrc))
        ////            return false;

        ////        // Simple heuristic: contains both '@' (alias lines) and '=' (final formula line)
        ////        return frsrc.Contains('@') && frsrc.Contains('=');
        ////    }

        ////    private FormulaResult EvaluateMultiLine(string program, FormulaEvaluationContext evalContext)
        ////    {
        ////        var lines = program
        ////            .Split('\n')
        ////            .Select(l => l.Trim())
        ////            .Where(l => !string.IsNullOrEmpty(l))
        ////            .ToList();

        ////        FormulaResult? finalResult = null;

        ////        foreach (var line in lines)
        ////        {
        ////            if (line.StartsWith("@"))
        ////            {
        ////                // @I120: 150+INP;
        ////                var colonIdx = line.IndexOf(':');
        ////                if (colonIdx < 0)
        ////                    throw new FormatException($"Invalid alias line: {line}");

        ////                var aliasPart = line[1..colonIdx].Trim();      // "I120"
        ////                var formulaPart = line[(colonIdx + 1)..].Trim(); // "150+INP;"

        ////                if (formulaPart.EndsWith(";"))
        ////                    formulaPart = formulaPart[..^1].Trim();

        ////                var result = _engine.Evaluate(formulaPart, evalContext);
        ////                evalContext.Locals[aliasPart] = new ConstantNode(result.Value, LineType.Decimal);
        ////            }
        ////            else if (line.StartsWith("="))
        ////            {
        ////                // = I120+I135;
        ////                var formulaPart = line[1..].Trim();
        ////                if (formulaPart.EndsWith(";"))
        ////                    formulaPart = formulaPart[..^1].Trim();

        ////                finalResult = _engine.Evaluate(formulaPart, evalContext);
        ////            }
        ////            else
        ////            {
        ////                // Optionally support legacy single-line formulas without '@' or '='
        ////                // For safety, you can either:
        ////                // - treat them as final formula, or
        ////                // - ignore them, or
        ////                // - throw if you want strict syntax.
        ////                // Here we treat them as the final formula if no '=' line exists yet.
        ////                if (finalResult is null)
        ////                {
        ////                    var formulaPart = line;
        ////                    if (formulaPart.EndsWith(";"))
        ////                        formulaPart = formulaPart[..^1].Trim();

        ////                    finalResult = _engine.Evaluate(formulaPart, evalContext);
        ////                }
        ////            }
        ////        }

        ////        return finalResult ?? FormulaResult.Empty;
        ////    }
        ////    private static bool IsPassthroughFormula(string? frsrc)
        ////    {
        ////        if (string.IsNullOrWhiteSpace(frsrc))
        ////            return true;

        ////        var s = frsrc.Trim();

        ////        // Convention: lines starting with '#' are labels/notes → passthrough
        ////        if (s.StartsWith("#"))
        ////            return true;

        ////        return false;
        ////    }
        ////    private List<ActdetDto> EvaluateRubfmtRows(
        ////FormulaEvaluationContext ctx, RubvarDto rub, List<RubFmtRow> fmtRows)
        ////=> fmtRows.Select(r =>
        ////{
        ////    var inputValue = decimal.TryParse(r.InputValue, out var v) ? v : 0m;

        ////    // If no formula, just use InputValue
        ////    if (string.IsNullOrWhiteSpace(r.Ftsrc))
        ////    {
        ////        return new ActdetDto
        ////        {
        ////            Itie = ctx.Itie ?? 0,
        ////            Ipln = ctx.Ipln ?? 0,
        ////            Irub = rub.Id,
        ////            Ifmt = r.Ifmt,
        ////            Atyp = r.Atyp,
        ////            Vgpe = r.Vgpe,
        ////            Inptvalue = r.InputValue ?? string.Empty,
        ////            Aval = inputValue.ToString(),
        ////            Iraw = MyConverters.Trunc1000(inputValue)?.ToString() ?? string.Empty
        ////        };
        ////    }

        ////    // Otherwise, evaluate the formula
        ////    ctx.CurrentInputValue = inputValue;
        ////    var result = _engine.Evaluate(r.Ftsrc, ctx);

        ////    return new ActdetDto
        ////    {
        ////        Itie = ctx.Itie ?? 0,
        ////        Ipln = ctx.Ipln ?? 0,
        ////        Irub = rub.Id,
        ////        Ifmt = r.Ifmt,
        ////        Atyp = r.Atyp,
        ////        Vgpe = r.Vgpe,
        ////        Inptvalue = result?.Value?.ToString() ?? r.InputValue ?? string.Empty,
        ////        Aval = string.IsNullOrEmpty(r.Ftsrc)
        ////            ? string.Empty
        ////            : result?.Raw?.ToString() ?? string.Empty,
        ////        Iraw = MyConverters.Trunc1000(result?.Raw)?.ToString() ?? string.Empty
        ////    };
        ////}).ToList();

        //public FormulaResult EvaluateMultiLine(string program, FormulaEvaluationContext evalContext)
        //{
        //    var lines = program
        //        .Split('\n')
        //        .Select(l => l.Trim())
        //        .Where(l => !string.IsNullOrEmpty(l))
        //        .ToList();

        //    FormulaResult? finalResult = null;

        //    foreach (var line in lines)
        //    {
        //        if (line.StartsWith("@"))
        //        {
        //            // @I120: 150+INP;
        //            var colonIdx = line.IndexOf(':');
        //            if (colonIdx < 0)
        //                throw new FormatException($"Invalid alias line: {line}");

        //            var aliasPart = line[1..colonIdx].Trim(); // "I120"
        //            var formulaPart = line[(colonIdx + 1)..].Trim(); // "150+INP;"

        //            // Remove trailing ';' if present
        //            if (formulaPart.EndsWith(";"))
        //                formulaPart = formulaPart[..^1].Trim();

        //            var result = _engine.Evaluate(formulaPart, evalContext);
        //            evalContext.Locals[aliasPart] = new ConstantNode(result.Value, LineType.Decimal);
        //        }
        //        else if (line.StartsWith("="))
        //        {
        //            // = I120+I135;
        //            var formulaPart = line[1..].Trim();
        //            if (formulaPart.EndsWith(";"))
        //                formulaPart = formulaPart[..^1].Trim();

        //            finalResult = _engine.Evaluate(formulaPart, evalContext);
        //        }
        //        else
        //        {
        //            // Optionally support legacy lines without '@' or '='
        //            // e.g. "bonus: I120*10/100;"
        //            // You can parse similarly or delegate to existing logic.
        //        }
        //    }

        //    return finalResult ?? FormulaResult.Empty;
        //}
        //    private ActsaieDto EvaluateRubvarRow(
        //FormulaEvaluationContext ctx, RubvarDto rub, List<ActdetDto> dets)
        //    {
        //        // Find the corresponding RubVarRow to get InputValue
        //        var row = ((SaieSession)ctx.Session!).RubVarRows
        //            .FirstOrDefault(r => r.Irub == rub.Id);

        //        var inputValue = decimal.TryParse(row?.InputValue, out var v) ? v : 0m;

        //        // If no formula, just use InputValue
        //        if (string.IsNullOrWhiteSpace(rub.Frsrc))
        //        {
        //            return new ActsaieDto
        //            {
        //                Itie = ctx.Itie ?? 0,
        //                Ipln = ctx.Ipln ?? 0,
        //                Irub = rub.Id,
        //                Atyp = rub.Atyp,
        //                Inptvalue = row?.InputValue ?? string.Empty,
        //                Aval = inputValue.ToString(),
        //                Iraw = MyConverters.Trunc1000(inputValue)?.ToString() ?? string.Empty,
        //                Actdets = dets
        //            };
        //        }

        //        // Otherwise, evaluate the formula
        //        ctx.CurrentInputValue = inputValue;
        //        var result = _engine.Evaluate(rub.Frsrc, ctx);

        //        return new ActsaieDto
        //        {
        //            Itie = ctx.Itie ?? 0,
        //            Ipln = ctx.Ipln ?? 0,
        //            Irub = rub.Id,
        //            Atyp = rub.Atyp,
        //            Inptvalue = row?.InputValue ?? string.Empty,
        //            Aval = string.IsNullOrEmpty(rub.Frsrc)
        //                ? string.Empty
        //                : result?.Value?.ToString() ?? string.Empty,
        //            Iraw = MyConverters.Trunc1000(result?.Raw)?.ToString() ?? string.Empty,
        //            Actdets = dets
        //        };
        //    }
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
}