using GxFormula.ForaBizz;
using GxFormula.Forasource;

using GxShared.GxDtos;
//using GxShared.GxGuards;
//using GxShared.Interfaces;
using GxTie.StaticHelpers;

using GxTie.Services.LineSources;

namespace GxTie.Services.Calculation
{
    public interface ICalculationService
    {
        Task<SaieSession> CalculateSaieAsync(CalcContext ctx, SaieSession session);
        Task<CalcSession> RunCalcAsync(CalcContext ctx, CalcSession session);
    }
    public sealed class CalculationService : ICalculationService
    {
        private readonly FormulaEngine _engine;

        public CalculationService(FormulaEngine engine)
        {
            _engine = engine;
        }

        public Task<SaieSession> CalculateSaieAsync(CalcContext ctx, SaieSession session)
        {
            if (ctx.Program is null)
                throw new ArgumentNullException(nameof(ctx.Program));

            session.Program = ctx.Program;
            session.Tier = ctx.Tier;
            session.RubVarRows ??= BuildRubVarRows(ctx.Program);
            session.RubFmtRows ??= BuildRubFmtRows(ctx.Program);

            session.Actsaies.Clear();
            session.Actdets.Clear();

            foreach (var rub in ctx.Program.Rubvars)
            {
                var row = session.RubVarRows.FirstOrDefault(x => x.Irub == rub.Id);
                if (row is null)
                    continue;

                var fmtRows = row.Details ?? new List<RubFmtRow>();
                var dets = EvaluateRubFmtRows(ctx, rub, fmtRows);

                session.Actdets.AddRange(dets);

                var act = EvaluateRubVarRow(ctx, rub, dets);
                session.Actsaies.Add(act);

                row.Aval = act.Aval;
                row.Iraw = act.Iraw?.ToString();
            }

            return Task.FromResult(session);
        }

        public Task<CalcSession> RunCalcAsync(CalcContext ctx, CalcSession session)
        {
            var evalCtx = BuildEvalContext(ctx);
            var lines = _innerParser.Parse(new PlngenLineSource(ctx.Program));

            foreach (var line in lines)
            {
                var result = _engine.Evaluate(line.Formula, evalCtx);
                if (result is null)
                    continue;

                session.Outputs[line.LineNumber ?? 0] = CreateOutputStream(ctx, line, result);

                if (ctx.IsTestMode)
                    session.Resbros.Add(CreateResbro(ctx, line, result));
                else
                {
                    session.Resdons.Add(CreateResdon(ctx, line, result));
                    if (line.SaveDetail)
                        session.Resdets.Add(CreateResdet(ctx, line, result));
                }
            }

            return Task.FromResult(session);
        }

        private List<RubVarRow> BuildRubVarRows(PlngenDto program)
            => program.Rubvars.Select(v => new RubVarRow
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
                SourceRubvar = v,
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
                    SourceRubfmt = f
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
                    Iraw = string.Empty,
                    SourceRubfmt = f
                }))
                .ToList();

        private FormulaEvaluationContext BuildEvalContext(CalcContext ctx)
            => new()
            {
                Idorg = ctx.Idorg,
                Ipln = ctx.Ipln,
                Itie = ctx.Itie
            };
        private ActsaieDto EvaluateRubVarRow(CalcContext ctx, RubvarDto rub, List<ActdetDto> dets)
        {
            var result = _engine.Evaluate(rub.Frsrc, BuildEvalContext(ctx));
            return new ActsaieDto
            {
                Itie = ctx.Itie,
                Ipln = ctx.Ipln,
                Irub = rub.Id,
                Atyp = rub.Atyp,
                Inptvalue = result?.Value?.ToString(),
                Aval = result?.Raw,
                Iraw = MyConverters.Trunc1000(result?.Raw)?.ToString(),
                Actdets = dets
            };
        }

        private List<ActdetDto> EvaluateRubFmtRows(CalcContext ctx, RubvarDto rub, List<RubFmtRow> fmtRows)
            => fmtRows.Select(r =>
            {
                var result = _engine.Evaluate(r.Ftsrc, BuildEvalContext(ctx));

                return new ActdetDto
                {
                    Itie = ctx.Itie,
                    Ipln = ctx.Ipln,
                    Irub = rub.Id,
                    Ifmt = r.Ifmt,
                    Atyp = r.Atyp,
                    Vgpe = r.Vgpe,
                    Sesperi = ctx.CurSes,
                    Inptvalue = result?.Value?.ToString(),
                    Aval = result?.Raw,
                    Iraw = MyConverters.Trunc1000(result?.Raw)?.ToString()
                };
            }).ToList();
        private static OutDataLineStream CreateOutputStream(CalcContext ctx, ProgramLine line, FormulaResult result)
            => new()
            {
                Itie = ctx.Itie,
                Ipln = ctx.Ipln,
                Irub = line.Irub ?? 0,
                Atyp = result.Type.HasValue ? (int)result.Type.Value : 0,
                Inptvalue = result.Value?.ToString(),
                Aval = result.Raw?.ToString(),
                Iraw = MyConverters.Trunc1000(result.Raw).ToString()
            };

        private ResdonDto CreateResdon(CalcContext ctx, ProgramLine line, FormulaResult result) => new()
        {
            Itie = ctx.Itie,
            Ipln = ctx.Ipln,
            Irub = line.Irub ?? 0,
            Atyp = result.Type.HasValue ? (int)result.Type.Value : 0,
            Inptvalue = result.Value?.ToString(),
            Aval = result.Raw?.ToString(),
            Iraw = MyConverters.Trunc1000(result.Raw).ToString()
        };

        private ResbroDto CreateResbro(CalcContext ctx, ProgramLine line, FormulaResult result) => new()
        {
            Itie = ctx.Itie,
            Ipln = ctx.Ipln,
            Irub = line.Irub ?? 0,
            Atyp = result.Type.HasValue ? (int)result.Type.Value : 0,
            Inptvalue = result.Value?.ToString(),
            Aval = result.Raw?.ToString(),
            Iraw = MyConverters.Trunc1000(result.Raw).ToString()
        };

        private ResdetDto CreateResdet(CalcContext ctx, ProgramLine line, FormulaResult result) => new()
        {
            Itie = ctx.Itie,
            Ires = 0,
            Ipln = ctx.Ipln,
            Irub = line.Irub ?? 0,
            Ifmt = line.Ifmt ?? 0,
            Atyp = result.Type.HasValue ? (int)result.Type.Value : 0,
            Inptvalue = result.Value?.ToString(),
            Aval = result.Raw?.ToString(),
            Iraw = MyConverters.Trunc1000(result.Raw).ToString()
        };
    }
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCalculationPipeline(this IServiceCollection services)
        {
            services.AddSingleton<FormulaEngine>();
            services.AddSingleton<ProgramLineParser>();

            services.AddScoped<ISaieCalculator, SaieCalculator>();
            services.AddScoped<IProgramCalculator, ProgramCalculator>();

            services.AddSingleton<ISaieSessionFactory, SaieSessionFactory>();

            services.AddScoped<ICalculationPersistence, CalculationPersistence>();

            services.AddScoped<ICalculationService, CalculationService>();
            services.AddScoped<ICalculationWorkflow, CalculationWorkflow>();
            services.AddScoped<ISaieWorkflowService, SaieWorkflowService>();

            return services;
        }
        //public static IServiceCollection2 AddCalculationPipeline(this IServiceCollection services)
        //{
        //    // Core engine (singleton, stateless)
        //    //services.AddSingleton<FormulaEngine>();

        //    // Parser (singleton, stateless)
        //    //services.AddSingleton<IProgramLineParser, ProgramLineParser>();

        //    // Internal calculators (scoped)
        //    //services.AddScoped<ISaieCalculator, SaieCalculator>();
        //    //services.AddScoped<IProgramCalculator, ProgramCalculator>();

        //    // Public calculation service (facade)
        //    //services.AddScoped<ICalculationService, CalculationService>();

        //    // Workflow and persistence
        //    //services.AddScoped<ICalculationWorkflow, CalculationWorkflow>();
        //    //services.AddScoped<ISaieWorkflowService, SaieWorkflowService>();
        //    //services.AddScoped<ICalculationPersistence, CalculationPersistence>();

        //    return services;
        //}
    }
    public sealed class FormulaLineExecutor
    {
        private readonly FormulaEvaluationService _evaluationService;
        private readonly ProgramLineParser _innerParser;
        public FormulaLineExecutor(FormulaEvaluationService evaluationService)
        {
            _evaluationService = evaluationService;
        }

        // Saiepart: one formula per RubvarDto
        public void ExecuteSaiepart(IProgramLineSource source, FormulaEvaluationContext context, SaieSession session)
        {
            var parser = new ProgramLineParser();
            var lines = parser.Parse(source); // returns ProgramLine

            foreach (var line in lines)
            {
                var outcome = _evaluationService.EvaluateWithLog(line, context);
                line.Aval = outcome.Result.Value?.ToString();
            }

            // Synchronize results back into grid
            ApplyCalculatedResultsToGrid(session);
            //SyncResultsToGrid(lines, session);
        }


        public void ExecuteCalcpart(IProgramLineSource source, FormulaEvaluationContext context)
        {
            var parser = new ProgramLineParser();
            var lines = parser.Parse(source);

            var results = new Dictionary<string, object>();

            foreach (var line in lines.OrderBy(l => l.LineNumber))
            {
                var outcome = _evaluationService.EvaluateWithLog(line, context);

                if (line.LineNumber.HasValue)
                {
                    results[$"@{line.LineNumber}"] = outcome.Result.Value;
                    line.Aval = outcome.Result.Value?.ToString();
                }
                else if (!string.IsNullOrEmpty(line.Identifier))
                {
                    results[line.Identifier] = outcome.Result.Value;
                    context.Variables[line.Identifier] = outcome.Result.Value;
                    line.Aval = outcome.Result.Value?.ToString();
                }
            }
        }
        
        private void ApplyCalculatedResultsToGrid(SaieSession session)
        {
            // Update RubVarRows from Actsaies
            foreach (var act in session.Actsaies)
            {
                var row = session.RubVarRows.FirstOrDefault(r => r.Irub == act.Irub);
                if (row is not null)
                {
                    row.Vgpe = act.Vgpe;
                    row.Aval = act.Aval;
                    row.Iraw = act.Iraw?.ToString();
                    row.InputValue = act.Inptvalue ?? row.InputValue;
                }
            }

            // Update RubFmtRows (details) from Actdets
            foreach (var det in session.Actdets)
            {
                var master = session.RubVarRows.FirstOrDefault(r => r.Irub == det.Irub);
                if (master is null)
                    continue;

                var detail = master.Details.FirstOrDefault(d => d.Ifmt == det.Ifmt);
                if (detail is not null)
                {
                    detail.Vgpe = det.Vgpe;
                    detail.Aval = det.Aval;
                    detail.Iraw = det.Iraw?.ToString();
                    detail.InputValue = det.Inptvalue ?? detail.InputValue;
                }
            }
        }
        public List<FormulaLine> Parse(IProgramLineSource source)
        {
            var parser = new ProgramLineParser();
            var programLines = parser.ParseProgramLines(source.GetSourceText());
            var contexts = source.GetContexts();

            var formulaLines = new List<FormulaLine>();

            foreach (var pl in programLines)
            {
                var ctx = contexts.FirstOrDefault(c => c.LineNumber == pl.LineNumber);

                formulaLines.Add(new FormulaLine
                {
                    LineNumber = pl.LineNumber,
                    Identifier = pl.Identifier,
                    Formula = pl.Formula,
                    //Irub = ctx?.Irub,
                    //Ifmt = ctx?.Ifmt,
                    //Liba = ctx?.Liba,
                    Aval = null,
                    InputValue = string.Empty
                });
            }

            return formulaLines;
        }

    }
    public sealed class CalcRunner
    {
        private readonly ProgramLineParser _innerParser;
        private readonly FormulaEngine _engine;

        public CalcRunner(FormulaEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            _innerParser = new ProgramLineParser();
        }

        private FormulaEvaluationContext BuildEvalContext(CalcContext ctx)
        {
            return new FormulaEvaluationContext
            {
                SessionDate = ctx.Date,
                Tier = ctx.Tier,
                Actsaies = ctx.Actsaies,
                Actdets = ctx.Actdets,
                PreviousLines = new List<FormulaLine>()
            };
        }

        public Task<CalcSession> RunCalcAsync(CalcContext ctx, CalcSession session)
        {
            var evalCtx = BuildEvalContext(ctx);
            var lines = _innerParser.Parse(new PlngenLineSource(ctx.Program));

            foreach (var line in lines)
            {
                var result = _engine.Evaluate(line.Formula, evalCtx);
                if (result is null)
                    continue;

                session.Outputs[line.LineNumber ?? 0] = CreateOutputStream(ctx, line, result);

                if (ctx.IsTestMode)
                {
                    session.Resbros.Add(CreateResbro(ctx, line, result));
                }
                else
                {
                    session.Resdons.Add(CreateResdon(ctx, line, result));
                    if (line.SaveDetail)
                        session.Resdets.Add(CreateResdet(ctx, line, result));
                }
            }

            return Task.FromResult(session);
        }
    }

    //public sealed class FormulaLineExecutor
    //{
    //    private readonly FormulaEvaluationService _evaluationService;

    //    public FormulaLineExecutor(FormulaEvaluationService evaluationService)
    //    {
    //        _evaluationService = evaluationService;
    //    }

    //    // Saiepart: one formula per RubvarDto
    //    public void ExecuteSaiepart(RubvarLineSource source, FormulaEvaluationContext context)
    //    {
    //        var parser = new ProgramLineParser();
    //        var lines = parser.Parse(source);

    //        foreach (var line in lines)
    //        {
    //            var outcome = _evaluationService.EvaluateWithLog(line, context);

    //            // Persist result into RubvarDto / Actsaie
    //            if (line.Rubvar != null)
    //            {
    //                line.Rubvar.CalcValue = outcome.Result.Value;
    //                PersistToActsaie(line.Rubvar, outcome.Result, context);
    //            }
    //        }
    //    }

    //    // Calcpart: multiple sequential lines
    //    public void ExecuteCalcpart(PlngenLineSource source, FormulaEvaluationContext context)
    //    {
    //        var parser = new ProgramLineParser();
    //        var lines = parser.Parse(source);

    //        var results = new Dictionary<string, object>();

    //        foreach (var line in lines.OrderBy(l => l.LineNumber))
    //        {
    //            var outcome = _evaluationService.EvaluateWithLog(line, context);

    //            if (line.LineNumber.HasValue)
    //            {
    //                // Numbered line → persist
    //                results[$"@{line.LineNumber}"] = outcome.Result.Value;
    //                if (line.Rubvar != null)
    //                    PersistToActsaie(line.Rubvar, outcome.Result, context);
    //            }
    //            else if (!string.IsNullOrEmpty(line.Liba))
    //            {
    //                // Named variable → store in context only
    //                results[line.Liba] = outcome.Result.Value;
    //                context.Variables[line.Liba] = outcome.Result.Value;
    //            }
    //        }
    //    }

    //    private void PersistToActsaie(RubvarDto rubvar, FormulaResult result, FormulaEvaluationContext context)
    //    {
    //        // Map result into Actsaie/Actdet persistence model
    //        var act = context.Actsaies.FirstOrDefault(a => a.Id == rubvar.Id);
    //        if (act != null)
    //        {
    //            act.Iraw = result.Raw;
    //            act.Aval = result.Value.ToString(); // as decimal?;
    //        }
    //    }
    //}
    // private void ApplyCalculatedResultsToGrid(SaieSession session)
    // {
    //     // Update Aval on RubVarRows
    //     foreach (var act in session.Actsaies)
    //     {
    //         var row = session.RubVarRows.FirstOrDefault(r => r.Irub == act.Irub);
    //         if (row is not null)
    //         {
    //             row.Vgpe = act.Vgpe;
    //             row.Aval = act.Aval;
    //             row.Iraw = act.Iraw?.ToString();
    //         }
    //     }

    //     // Update Aval on RubFmtRows (Details)
    //     foreach (var det in session.Actdets)
    //     {
    //         var master = session.RubVarRows.FirstOrDefault(r => r.Irub == det.Irub);
    //         if (master is null)
    //             continue;

    //         var detail = master.Details.FirstOrDefault(d =>
    //             d.Irub == det.Irub && d.Ifmt == det.Ifmt);

    //         if (detail is not null)
    //         {
    //             detail.Vgpe = det.Vgpe;
    //             detail.Aval = det.Aval;
    //             detail.Iraw = det.Iraw?.ToString();
    //         }
    //     }
    // }

    //public void ExecuteSaiepart(RubvarLineSource source, FormulaEvaluationContext context)
    //{
    //    var parser = new ProgramLineParser();
    //    var lines = parser.Parse(source);

    //    foreach (var line in lines)
    //    {
    //        var outcome = _evaluationService.EvaluateWithLog(line, context);

    //        // Push result back into UI grid row
    //        line.Aval = outcome.Result.Value.ToString();
    //        line.InputValue = line.InputValue; // keep user input untouched
    //    }
    //}

    // Calcpart: multiple sequential lines
    //public void ExecuteCalcpart(PlngenLineSource source, FormulaEvaluationContext context)
    //{
    //    var parser = new ProgramLineParser();
    //    var lines = parser.Parse(source);

    //    var results = new Dictionary<string, object>();

    //    foreach (var line in lines.OrderBy(l => l.LineNumber))
    //    {
    //        var outcome = _evaluationService.EvaluateWithLog(line, context);

    //        if (line.LineNumber.HasValue)
    //        {
    //            // Numbered line → store in results
    //            var key = $"@{line.LineNumber}";
    //            results[key] = outcome.Result.Value;

    //            // Update ProgramLine values
    //            line.Aval = outcome.Result.Value?.ToString();
    //            line.InputValue = line.InputValue;
    //        }
    //        else if (!string.IsNullOrEmpty(line.Identifier))
    //        {
    //            // Named variable → store in context only
    //            results[line.Identifier] = outcome.Result.Value;
    //            context.Variables[line.Identifier] = outcome.Result.Value;

    //            line.Aval = outcome.Result.Value?.ToString();
    //            line.InputValue = line.InputValue;
    //        }
    //    }
    //}

    //public void ExecuteCalcpart(PlngenLineSource source, FormulaEvaluationContext context)
    //{
    //    var parser = new ProgramLineParser();
    //    var lines = parser.Parse(source);

    //    var results = new Dictionary<string, object>();

    //    foreach (var line in lines.OrderBy(l => l.LineNumber))
    //    {
    //        var outcome = _evaluationService.EvaluateWithLog(line, context);

    //        if (line.LineNumber.HasValue)
    //        {
    //            // Numbered line → store in results and grid
    //            results[$"@{line.LineNumber}"] = outcome.Result.Value;
    //            line.Aval = outcome.Result.Value.ToString();
    //        }
    //        else if (!string.IsNullOrEmpty(line.Liba))
    //        {
    //            // Named variable → store in context only
    //            results[line.Liba] = outcome.Result.Value;
    //            context.Variables[line.Liba] = outcome.Result.Value;
    //        }
    //    }
    //}
}