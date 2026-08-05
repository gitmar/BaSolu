using GxFormula.ForaBizz;
using GxFormula.Forasource;

using GxShared.GxDtos;
using GxShared.GxGuards;
using GxShared.Helpers;
using GxShared.Interfaces;

using GxTie.Services.Interfaces;
using GxTie.StaticHelpers;
using GxTie.Services;
using GxTie.Services.LineSources;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable


namespace GxTie.Services
{
    public sealed class PlngenLineSource : IProgramLineSource
    {
        private readonly PlngenDto _program;

        public PlngenLineSource(PlngenDto program) => _program = program;

        public string? GetSourceText() => _program?.Fpsrc;

        public IReadOnlyCollection<ProgramLineContext> GetContexts()
        {
            var items = new List<ProgramLineContext>();

            foreach (var rubvar in _program.Rubvars)
            {
                items.Add(new ProgramLineContext
                {
                    LineNumber = TryParseLine(rubvar.Scdrub),
                    Irub = rubvar.Id,
                    Liba = rubvar.Liba,
                    Rubvar = rubvar
                });

                foreach (var rubfmt in rubvar.Rubfmts)
                {
                    items.Add(new ProgramLineContext
                    {
                        LineNumber = TryParseLine(rubfmt.Zcdrub),
                        Irub = rubvar.Id,
                        Ifmt = rubfmt.Id,
                        Liba = rubfmt.Liba,
                        Rubvar = rubvar,
                        Rubfmt = rubfmt
                    });
                }
            }

            return items;
        }

        private static int? TryParseLine(string? value) => int.TryParse(value, out var n) ? n : null;
    }

    public sealed class RubvarLineSource : IProgramLineSource
    {
        private readonly RubvarDto _rubvar;
        private readonly IReadOnlyCollection<RubfmtDto> _rubfmts;

        public RubvarLineSource(RubvarDto rubvar, IReadOnlyCollection<RubfmtDto> rubfmts)
        {
            _rubvar = rubvar;
            _rubfmts = rubfmts;
        }

        public string? GetSourceText() => _rubvar?.Frsrc;

        public IReadOnlyCollection<ProgramLineContext> GetContexts()
        {
            var items = new List<ProgramLineContext>
        {
            new ProgramLineContext
            {
                Irub = _rubvar.Id,
                Liba = _rubvar.Liba,
                Rubvar = _rubvar
            }
        };

            foreach (var rubfmt in _rubfmts)
            {
                items.Add(new ProgramLineContext
                {
                    LineNumber = TryParseLine(rubfmt.Zcdrub),
                    Irub = _rubvar.Id,
                    Ifmt = rubfmt.Id,
                    Liba = rubfmt.Liba,
                    Rubvar = _rubvar,
                    Rubfmt = rubfmt
                });
            }

            return items;
        }

        private static int? TryParseLine(string? value) => int.TryParse(value, out var n) ? n : null;
    }

    public sealed class ProgramLineParser
    {
        public List<ProgramLine> Parse(IProgramLineSource source)
        {
            var lines = ParseProgramLines(source.GetSourceText());
            return EnrichProgramLines(lines, source.GetContexts());
        }

        public List<ProgramLine> ParseProgramLines(string? sourceText)
        {
            var lines = new List<ProgramLine>();

            if (string.IsNullOrWhiteSpace(sourceText))
                return lines;

            var rawLines = sourceText
                .Replace("\r\n", "\n")
                .Replace('\r', '\n')
                .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var raw in rawLines)
            {
                var parsed = ParseProgramLine(raw);
                if (parsed != null)
                    lines.Add(parsed);
            }

            return lines;
        }

        private ProgramLine? ParseProgramLine(string raw)
        {
            var line = raw.Trim();
            if (string.IsNullOrWhiteSpace(line))
                return null;

            var atIndex = line.IndexOf('@');
            if (atIndex < 0)
                return null;

            line = line[(atIndex + 1)..].Trim();

            var colonIndex = line.IndexOf(':');
            if (colonIndex <= 0)
                return null;

            if (!int.TryParse(line[..colonIndex].Trim(), out var lineNumber))
                return null;

            var exprPart = line[(colonIndex + 1)..].Trim();

            string? meta = null;
            var metaStart = exprPart.LastIndexOf('[');
            var metaEnd = exprPart.LastIndexOf(']');

            if (metaStart >= 0 && metaEnd > metaStart)
            {
                meta = exprPart[(metaStart + 1)..metaEnd].Trim();
                exprPart = exprPart[..metaStart].Trim();
            }

            if (exprPart.EndsWith(";"))
                exprPart = exprPart[..^1].TrimEnd();

            if (string.IsNullOrWhiteSpace(exprPart))
                return null;

            return new ProgramLine
            {
                LineNumber = lineNumber,
                Formula = exprPart,
                Meta = meta
            };
        }
        private List<ProgramLine> EnrichProgramLines(
            List<ProgramLine> lines,
            IReadOnlyCollection<ProgramLineContext> contexts)
        {
            var byLine = contexts
                .Where(x => x.LineNumber.HasValue)
                .GroupBy(x => x.LineNumber!.Value)
                .ToDictionary(g => g.Key, g => g.First());

            foreach (var line in lines)
            {
                byLine.TryGetValue(line.LineNumber ?? 0, out var ctx);
                line.Irub = ctx?.Irub;
                line.Ifmt = ctx?.Ifmt;
                line.Liba = ctx?.Liba;
                line.Type = ProgramLineTypeMapper.MapType(line.Meta, ctx?.Rubvar, ctx?.Rubfmt);
                line.SaveDetail = ProgramLineTypeMapper.ShouldSaveDetail(line.Meta, ctx?.Rubvar, ctx?.Rubfmt);
            }

            return lines;
        }
    }

    public interface ICalculationService
    {
        Task<SaieSession> CalculateSaieAsync(CalcContext ctx);
        Task<CalcSession> RunCalcAsync(CalcContext ctx, CalcSession session);
    }

    public interface ICalculationPersistence
    {
        Task SaveSaieAsync(CalcContext ctx, SaieSession session, PendingSaveMode inSaveMode);
        Task SaveCalcAsync(CalcContext ctx, CalcSession session);
    }

    public interface ICalculationWorkflow
    {
        Task<SaieSession> LoadSaieAsync(int programId, int tierId);
        Task<SaieSession> CalculateSaieAsync(CalcContext ctx);
        Task<SaieSession> CalculateAndSaveSaieAsync(CalcContext ctx, PendingSaveMode inSaveMode);
        //Task<CalcSession> CalculateCalcAsync(CalcContext ctx);
        //Task<CalcSession> CalculateAndSaveCalcAsync(CalcContext ctx);
        Task<List<CalcSession>> CalculateCalcAsync(IEnumerable<CalcContext> contexts);
        Task<List<CalcSession>> CalculateAndSaveCalcAsync(IEnumerable<CalcContext> contexts);
    }
    public interface ISaieWorkflowService
    {
        Task<SaieSession> LoadSaieAsync(int programId, int tierId);
        Task<SaieSession> CalculateSaieAsync(CalcContext ctx, SaieSession session);
        Task SaveSaieAsync(CalcContext ctx, SaieSession session);
    }
    public interface IProgramLineSource
    {
        string? GetSourceText();
        IReadOnlyCollection<ProgramLineContext> GetContexts();
    }

    public sealed class CalculationService : ICalculationService
    {
        private readonly FormulaEngine _engine;
        private readonly ProgramLineParser _lineParser;

        public CalculationService(FormulaEngine engine, ProgramLineParser lineParser)
        {
            _engine = engine;
            _lineParser = lineParser;
        }

        public Task<SaieSession> CalculateSaieAsync(CalcContext ctx)
        {
            var source = new PlngenLineSource(ctx.Program);
            var lines = _lineParser.Parse(source);

            var session = new SaieSession
            {
                Program = ctx.Program,
                Tier = ctx.Tier,
                RubVarRows = BuildRubVarRows(ctx.Program),
                RubFmtRows = BuildRubFmtRows(ctx.Program)
            };

            session.Actdets.Clear();
            session.Actsaies.Clear();

            foreach (var rub in ctx.Program.Rubvars)
            {
                var fmtRows = session.RubFmtRows.Where(x => x.Irub == rub.Id).ToList();
                var dets = EvaluateRubFmtRows(ctx, rub, fmtRows);
                session.Actdets.AddRange(dets);

                var act = EvaluateRubVarRow(ctx, rub, dets);
                session.Actsaies.Add(act);

                var row = session.RubVarRows.FirstOrDefault(x => x.Irub == rub.Id);
                if (row != null)
                    row.Aval = act.Aval;
            }

            return Task.FromResult(session);
        }
        public Task<CalcSession> RunCalcAsync(CalcContext ctx, CalcSession session)
        {
            var evalCtx = BuildEvalContext(ctx);
            var lines = _lineParser.Parse(new PlngenLineSource(ctx.Program));

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
        private static OutDataLineStream CreateOutputStream(
        CalcContext ctx,
        ProgramLine line,
        FormulaResult result)
        {
            return new OutDataLineStream
            {
                Itie = ctx.Itie,
                Ipln = ctx.Ipln,
                Irub = line.Irub ?? 0,
                Atyp = result.Type.HasValue ? (int)result.Type.Value : 0,
                Inptvalue = result.Value?.ToString(),
                Aval = result.Raw?.ToString(),
                Iraw = MyConverters.Trunc1000(result.Raw).ToString(),
            };
        }

        private List<RubGridRow> BuildRubVarRows(PlngenDto program)
            => program.Rubvars.Select(v => new RubGridRow
            {
                Id = v.Id,
                Irub = v.Id,
                Scdrub = v.Scdrub,
                Atyp = v.Atyp,
                Vgpe = v.Vgpe,
                Liba = v.Liba,
                Abg = v.Liba
            }).ToList();

        private List<RubFmtRow> BuildRubFmtRows(PlngenDto program)
            => program.Rubvars
                .SelectMany(v => v.Rubfmts.Select(f => new RubFmtRow
                {
                    Id = f.Id,
                    Irub = v.Id,
                    Ifmt = f.Id,
                    Scdfmt = f.Zcdrub,
                    Atyp = f.Ztyp,
                    Vgpe = f.Vgpe,
                    Liba = f.Liba,
                    Ftsrc = f.Ftsrc
                }))
                .ToList();

        private FormulaEvaluationContext BuildEvalContext(CalcContext ctx, RubvarDto rubvar)
        {
            var scope = new RubvarEvaluationScope
            {
                Rubvar = rubvar,
                Rubfmts = rubvar.Rubfmts
            };

            return new FormulaEvaluationContext
            {
                Idorg = ctx.Idorg,
                Ipln = ctx.Ipln,
                Itie = ctx.Itie,
                SessionDate = DateTime.Today,
                Scope = scope,
                ResolveVariable = name => ResolveVariable(scope, name)
            };
        }

        private object? ResolveVariable(RubvarEvaluationScope scope, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var key = name.Trim().ToUpperInvariant();

            if (key.StartsWith("Z") && key.Length == 3 && int.TryParse(key[1..], out var n))
            {
                var zcode = $"Z{n:00}";
                var match = scope.Rubfmts.FirstOrDefault(x =>
                    string.Equals(x.Zcdrub?.Trim(), zcode, StringComparison.OrdinalIgnoreCase));

                return match?.Aval;
            }

            return null;
        }
        private ActsaieDto EvaluateRubVarRow(CalcContext ctx, RubvarDto rub, List<ActdetDto> dets)
        {
            var result = _engine.Evaluate(rub.Frsrc, BuildEvalContext(ctx, rub));

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
                    Iact = 0,
                    Ipln = ctx.Ipln,
                    Irub = rub.Id,
                    Ifmt = r.Ifmt,
                    Atyp = r.Atyp,
                    Vgpe = r.Vgpe,
                    Sesperi = ctx.CurSes,
                    Inptvalue = result?.Value?.ToString(),
                    Aval = result?.Raw?.ToString(),
                    Iraw = MyConverters.Trunc1000(result?.Raw).ToString()
                };
            }).ToList();

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

        private FormulaEvaluationContext BuildEvalContext(CalcContext ctx) => new()
        {
            Idorg = ctx.Idorg,
            Ipln = ctx.Ipln,
            Itie = ctx.Itie
        };
    }

    public sealed class CalculationPersistence : ICalculationPersistence
    {
        private readonly IPendingChangesGuard _guard;

        public CalculationPersistence(IPendingChangesGuard guard)
        {
            _guard = guard;
        }

        public async Task SaveSaieAsync(CalcContext ctx, SaieSession session, PendingSaveMode inSaveMode)
        {
            foreach (var act in session.Actsaies)
            {
                act.Itie = ctx.Itie;
                act.Ipln = ctx.Ipln;
                act.Iraw = MyConverters.Trunc1000(act.Iraw).ToString();
            }

            foreach (var det in session.Actdets)
                det.Iraw = MyConverters.Trunc1000(det.Iraw).ToString();

            foreach (var act in session.Actsaies)
                await _guard.TrackInsert("Actsaie", act);

            foreach (var det in session.Actdets)
                await _guard.TrackInsert("Actdet", det);

            if (inSaveMode == PendingSaveMode.Immediate)
                await _guard.FlushAsync();
        }
        public async Task SaveCalcAsync(CalcContext ctx, CalcSession session)
        {
            foreach (var don in session.Resdons)
            {
                don.Itie = ctx.Itie;
                don.Ipln = ctx.Ipln;
                don.Iraw = MyConverters.Trunc1000(don.Iraw).ToString();
            }

            foreach (var bro in session.Resbros)
            {
                bro.Itie = ctx.Itie;
                bro.Ipln = ctx.Ipln;
                bro.Iraw = MyConverters.Trunc1000(bro.Iraw).ToString();
            }

            foreach (var det in session.Resdets)
                det.Iraw = MyConverters.Trunc1000(det.Iraw).ToString();

            await TrackAndFlushAsync("Resdon", session.Resdons);
            await TrackAndFlushAsync("Resbro", session.Resbros);
            await TrackAndFlushAsync("Resdet", session.Resdets);
        }

        private async Task TrackAndFlushAsync<TDto>(string entitySet, IEnumerable<TDto> items) where TDto : class
        {
            foreach (var item in items)
                await _guard.TrackInsert(entitySet, item);


            if (_guard.GetSaveMode() == PendingSaveMode.Immediate)
                return;

            await _guard.FlushAsync();
        }
    }

    public sealed class CalculationWorkflow : ICalculationWorkflow
    {
        private readonly ICalculationService _calcService;
        private readonly ICalculationPersistence _persistence;

        public CalculationWorkflow(ICalculationService calcService, ICalculationPersistence persistence)
        {
            _calcService = calcService;
            _persistence = persistence;
        }

        public async Task<SaieSession> LoadSaieAsync(int programId, int tierId)
        {
            var ctx = await BuildCalcContextAsync(programId, tierId);
            return await BuildSaieSessionAsync(ctx);
        }

        public async Task<SaieSession> CalculateSaieAsync(CalcContext ctx)
        {
            return await _calcService.CalculateSaieAsync(ctx);
        }

        public async Task<SaieSession> CalculateAndSaveSaieAsync(CalcContext ctx, PendingSaveMode inSaveMode)
        {
            var session = await _calcService.CalculateSaieAsync(ctx);
            await _persistence.SaveSaieAsync(ctx, session, inSaveMode);
            return session;
        }

        public async Task<List<CalcSession>> CalculateCalcAsync(IEnumerable<CalcContext> contexts)
        {
            var results = new List<CalcSession>();

            foreach (var ctx in contexts)
            {
                var session = new CalcSession
                {
                    Program = ctx.Program,
                    Tier = ctx.Tier
                };

                session = await _calcService.RunCalcAsync(ctx, session);
                results.Add(session);
            }

            return results;
        }

        public async Task<List<CalcSession>> CalculateAndSaveCalcAsync(IEnumerable<CalcContext> contexts)
        {
            var results = new List<CalcSession>();

            foreach (var ctx in contexts)
            {
                var session = new CalcSession
                {
                    Program = ctx.Program,
                    Tier = ctx.Tier
                };

                session = await _calcService.RunCalcAsync(ctx, session);
                await _persistence.SaveCalcAsync(ctx, session);
                results.Add(session);
            }

            return results;
        }
        private async Task<CalcContext> BuildCalcContextAsync(int programId, int tierId)
        {
            var program = await LoadProgramAsync(programId);
            var tier = await LoadTierAsync(tierId);

            return new CalcContext
            {
                Program = program,
                Tier = tier,
                IsTestMode = false
            };
        }

        private Task<PlngenDto> LoadProgramAsync(int programId)
            => Task.FromResult(new PlngenDto());

        private Task<TierspDto> LoadTierAsync(int tierId)
            => Task.FromResult(new TierspDto());
    }

    public sealed class SaieWorkflowService : ISaieWorkflowService
    {
        private readonly IProgramService _programService;
        private readonly ICalculation _calcService;
        private readonly ICalcPersistence _calcPersistence;

        public SaieWorkflowService(
            IProgramService programService,
            ICalcEngine calcService,
            ICalcPersistence calcPersistence)
        {
            _programService = programService;
            _calcService = calcService;
            _calcPersistence = calcPersistence;
        }

        public async Task<SaieSession> LoadSaieAsync(int programId, int tierId)
        {
            var ctx = await BuildCalcContextAsync(programId, tierId);
            var session = BuildSaieSession(ctx);
            return session;
        }

        public async Task<SaieSession> CalculateSaieAsync(CalcContext ctx, SaieSession session)
        {
            session ??= BuildSaieSession(ctx);

            foreach (var rubRow in session.RubVarRows)
            {
                var rubvar = rubRow.SourceRubvar;
                if (rubvar is null)
                    continue;

                var act = EvaluateRubVarRow(ctx, rubvar, rubRow.Details);
                rubRow.Aval = act.Aval;
                rubRow.Iraw = act.Iraw?.ToString();

                foreach (var det in act.Actdets)
                {
                    var detailRow = rubRow.Details.FirstOrDefault(x => x.Ifmt == det.Ifmt);
                    if (detailRow is null)
                        continue;

                    detailRow.Aval = det.Aval;
                    detailRow.Iraw = det.Iraw?.ToString();
                }
            }

            session.Actsaies = session.RubVarRows
                .Select(r => new ActsaieDto
                {
                    Itie = ctx.Itie,
                    Ipln = ctx.Ipln,
                    Irub = r.Irub,
                    Atyp = r.SourceRubvar?.Atyp,
                    Inptvalue = r.InputValue,
                    Aval = r.Aval,
                    Iraw = TryDecimal(r.Iraw).ToString(),
                    Actdets = r.Details
                        .Select(d => new ActdetDto
                        {
                            Itie = ctx.Itie,
                            Ipln = ctx.Ipln,
                            Irub = r.Irub,
                            Ifmt = d.Ifmt,
                            Atyp = d.SourceRubfmt?.Atyp,
                            Inptvalue = d.InputValue,
                            Aval = d.Aval,
                            Iraw = TryDecimal(d.Iraw).ToString()
                        })
                        .ToList()
                })
                .ToList();

            session.Actdets = session.Actsaies.SelectMany(x => x.Actdets).ToList();

            return session;
        }

        public async Task SaveSaieAsync(CalcContext ctx, SaieSession session)
        {
            await _calcPersistence.SaveSaieAsync(ctx, session);
        }

        private async Task<CalcContext> BuildCalcContextAsync(int programId, int tierId)
        {
            var program = await _programService.GetProgramAsync(programId);
            var tier = await _programService.GetTierAsync(tierId);

            return new CalcContext
            {
                Program = program,
                Tier = tier,
                Idorg = program?.CurOrga?.Idorg,
                Ipln = program?.Id,
                Itie = tier?.Id,
                IsTestMode = false
            };
        }

        private SaieSession BuildSaieSession(CalcContext ctx)
        {
            var session = new SaieSession
            {
                Program = ctx.Program,
                Tier = ctx.Tier,
                RubVarRows = ctx.Program.Rubvars
                    .Select(rubvar => new RubGridRow
                    {
                        Id = rubvar.Id,
                        Irub = rubvar.Id,
                        Scdrub = rubvar.Scdrub,
                        Liba = rubvar.Liba,
                        Abg = rubvar.Liba,
                        InputValue = string.Empty,
                        Iraw = string.Empty,
                        Aval = null,
                        SourceRubvar = rubvar,
                        Details = rubvar.Rubfmts.Select(rubfmt => new RubFmtRow
                        {
                            Id = rubfmt.Id,
                            Irub = rubvar.Id,
                            Ifmt = rubfmt.Id,
                            Scdfmt = rubfmt.Zcdrub,
                            Liba = rubfmt.Liba,
                            Ftsrc = rubfmt.Ftsrc,
                            InputValue = string.Empty,
                            Iraw = string.Empty,
                            Aval = null,
                            SourceRubfmt = rubfmt
                        }).ToList()
                    })
                    .ToList()
            };

            return session;
        }

        private static decimal? TryDecimal(string? value)
            => decimal.TryParse(value, out var d) ? d : null;
    }
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCalculationPipeline(this IServiceCollection services)
        {
            services.AddSingleton<FormulaEngine>();
            services.AddSingleton<ProgramLineParser>();

            services.AddScoped<ICalculationPersistence, CalculationPersistence>();
            services.AddScoped<ICalculationService, CalculationService>();
            services.AddScoped<ICalculationWorkflow, CalculationWorkflow>();

            return services;
        }
        //}
        //public static class ServiceCollectionExtensions
        //{
        //    public static void AddCalculationPipeline(this IServiceCollection services)
        //    {
        //        services.AddSingleton<FormulaEngine>();
        //        services.AddSingleton<ProgramLineParser>();
        //        services.AddSingleton<ICalculationService, CalculationService>();
        //        services.AddSingleton<ICalculationPersistence, CalculationPersistence>();
        //        services.AddSingleton<ICalculationWorkflow, CalculationWorkflow>();
        //    }
        //}
        //public interface ICalculationService
        //{
        //    Task<SaieSession> BuildSaieAsync(CalcContext ctx);
        //    Task<SaieSession> RecomputeSaieAsync(CalcContext ctx, SaieSession session);
        //    Task<CalcSession> RunCalcAsync(CalcContext ctx, CalcSession session);
        //    Task SaveSaieAsync(CalcContext ctx, SaieSession session);
        //    Task SaveCalcAsync(CalcContext ctx, CalcSession session);
        //}

        //public sealed class CalculationService : ICalculationService
        //{
        //    private readonly FormulaEngine _engine;
        //    private readonly IPendingChangesGuard _guard;

        //    public CalculationService(FormulaEngine engine, IPendingChangesGuard guard)
        //    {
        //        _engine = engine;
        //        _guard = guard;
        //    }


        //public async Task<CalcSession> CalculateCalcAsync(CalcContext ctx)
        //{
        //    var session = new CalcSession
        //    {
        //        Program = ctx.Program,
        //        Tier = ctx.Tier
        //    };

        //public async Task SaveSaieAsync(CalcContext ctx, SaieSession session)
        //{
        //    foreach (var act in session.Actsaies)
        //    {
        //        act.Itie = ctx.Itie;
        //        act.Ipln = ctx.Ipln;
        //        act.Iraw = MyConverters.Trunc1000(act.Iraw).ToString();
        //    }

        //    foreach (var det in session.Actdets)
        //        det.Iraw = MyConverters.Trunc1000(det.Iraw).ToString();

        //    await TrackAndFlushAsync("Actsaie", session.Actsaies);
        //    await TrackAndFlushAsync("Actdet", session.Actdets);
        //}

        //    return await _calcService.RunCalcAsync(ctx, session);
        //}

        //public async Task<CalcSession> CalculateAndSaveCalcAsync(CalcContext ctx)
        //{
        //    var session = await CalculateCalcAsync(ctx);
        //    await _persistence.SaveCalcAsync(ctx, session);
        //    return session;
        //}
        //public CalculationWorkflow(ICalculationService calcService, ICalculationPersistence persistence)
        //{
        //    _calcService = calcService;
        //    _persistence = persistence;
        //}
        //private static decimal? Trunc1000(object? value)
        //{
        //    if (value is null)
        //        return null;

        //    return decimal.TryParse(value.ToString(), out var d)
        //        ? Math.Round(d, 3)
        //        : null;
        //}
        //private static decimal? Trunc1000(object? value)
        //{
        //    if (value is null)
        //        return null;

        //    return decimal.TryParse(value.ToString(), out var d)
        //        ? Math.Round(d, 3)
        //        : null;
        //}
        //private ActsaieDto EvaluateRubVarRow(CalcContext ctx, RubvarDto rub, List<ActdetDto>? dets)
        //{
        //    var result = _engine.Evaluate(rub.Frsrc, BuildEvalContext(ctx));

        //    return new ActsaieDto
        //    {
        //        Itie = ctx.Itie,
        //        Ipln = ctx.Ipln,
        //        Irub = rub.Id,
        //        Atyp = rub.Atyp,
        //        Vgpe = rub.Vgpe,
        //        Sesperi = ctx.CurSes,
        //        Inptvalue = Convert.ToString(result?.Value),
        //        Aval = Convert.ToString(result?.Raw),
        //        Iraw = Convert.ToString(MyConverters.Trunc1000(result?.Raw)),
        //        Actdets = dets ?? new List<ActdetDto>()
        //    };
        //}
        //    public async Task<SaieSession> LoadSaieAsync(int programId, int tierId)
        //    {
        //        var ctx = await BuildCalcContextAsync(programId, tierId);
        //        var session = await BuildSaieAsync(ctx);
        //        return await RecomputeSaieAsync(ctx, session);
        //    }
        //    public Task<SaieSession> BuildSaieAsync(CalcContext ctx)
        //    {
        //        var session = new SaieSession
        //        {
        //            Program = ctx.Program,
        //            Tier = ctx.Tier,
        //            RubVarRows = BuildRubVarRows(ctx.Program),
        //            RubFmtRows = BuildRubFmtRows(ctx.Program)
        //        };

        //        return Task.FromResult(session);
        //    }
        //    private async Task<CalcContext> BuildCalcContextAsync(int programId, int tierId)
        //    {
        //        var program = await LoadProgramAsync(programId);
        //        var tier = await LoadTierAsync(tierId);

        //        return new CalcContext
        //        {
        //            Program = program,
        //            Tier = tier,
        //            //Idorg = program.Idorg,
        //            //Ipln = programId,
        //            //Itie = tierId,
        //            IsTestMode = false
        //        };
        //    }
        //    public Task<SaieSession> RecomputeSaieAsync(CalcContext ctx, SaieSession session)
        //    {
        //        session.Actdets.Clear();
        //        session.Actsaies.Clear();

        //        foreach (var rub in ctx.Program.Rubvars)
        //        {
        //            var fmtRows = session.RubFmtRows.Where(x => x.Irub == rub.Id).ToList();
        //            var dets = EvaluateRubFmtRows(ctx, rub, fmtRows);
        //            session.Actdets.AddRange(dets);

        //            var act = EvaluateRubVarRow(ctx, rub, dets);
        //            session.Actsaies.Add(act);

        //            var row = session.RubVarRows.FirstOrDefault(x => x.Irub == rub.Id);
        //            if (row != null)
        //                row.Aval = act.Aval;
        //        }

        //        return Task.FromResult(session);
        //    }
        //    private async Task<PlngenDto> LoadProgramAsync(int programId)
        //    {
        //        // Replace with your real repository / API call.
        //        return await Task.FromResult(new PlngenDto());
        //    }

        //    private async Task<TierspDto> LoadTierAsync(int tierId)
        //    {
        //        // Replace with your real repository / API call.
        //        return await Task.FromResult(new TierspDto());
        //    }
        //    public Task<CalcSession> RunCalcAsync(CalcContext ctx, CalcSession session)
        //    {
        //        var evalCtx = new FormulaEvaluationContext
        //        {
        //            Idorg = ctx.Idorg,
        //            Ipln = ctx.Ipln,
        //            Itie = ctx.Itie,
        //            InputData = session.Inputs,
        //            OutputData = session.Outputs,
        //            SessionDate = DateTime.Today
        //        };

        //        foreach (var line in GetProgramLines(ctx.Program))
        //        {
        //            var result = _engine.Evaluate(line.Fpsrc, evalCtx);
        //            if (result is null)
        //                continue;

        //            session.Outputs.Add(CreateOutputStream(ctx, line, result));

        //            if (ctx.IsTestMode)
        //            {
        //                session.Resbros.Add(CreateResbro(ctx, line, result));
        //            }
        //            else
        //            {
        //                session.Resdons.Add(CreateResdon(ctx, line, result));

        //                if (line.SaveDetail)
        //                    session.Resdets.Add(CreateResdet(ctx, line, result));
        //            }
        //        }

        //        return Task.FromResult(session);
        //    }

        //    public async Task SaveSaieAsync(CalcContext ctx, SaieSession session)
        //    {
        //        foreach (var act in session.Actsaies)
        //        {
        //            act.Itie = ctx.Itie;
        //            act.Ipln = ctx.Ipln;
        //            act.Iraw = Trunc1000(act.Iraw);
        //        }

        //        foreach (var det in session.Actdets)
        //            det.Iraw = Trunc1000(det.Iraw);

        //        await TrackAndFlushAsync("Actsaie", session.Actsaies);
        //        await TrackAndFlushAsync("Actdet", session.Actdets);
        //    }

        //    public async Task SaveCalcAsync(CalcContext ctx, CalcSession session)
        //    {
        //        foreach (var don in session.Resdons)
        //        {
        //            don.Itie = ctx.Itie;
        //            don.Ipln = ctx.Ipln;
        //            don.Iraw = Trunc1000(don.Iraw);
        //        }

        //        foreach (var bro in session.Resbros)
        //        {
        //            bro.Itie = ctx.Itie;
        //            bro.Ipln = ctx.Ipln;
        //            bro.Iraw = Trunc1000(bro.Iraw);
        //        }

        //        foreach (var det in session.Resdets)
        //            det.Iraw = Trunc1000(det.Iraw);

        //        await TrackAndFlushAsync("Resdon", session.Resdons);
        //        await TrackAndFlushAsync("Resbro", session.Resbros);
        //        await TrackAndFlushAsync("Resdet", session.Resdets);
        //    }

        //    private async Task TrackAndFlushAsync<TDto>(string entitySet, IEnumerable<TDto> items) where TDto : class
        //    {
        //        foreach (var item in items)
        //            await _guard.TrackInsert(entitySet, item);

        //        if (_guard.GetSaveMode() == PendingSaveMode.Immediate)
        //            return;

        //        await _guard.FlushAsync();
        //    }

        //    private List<RubGridRow> BuildRubVarRows(PlngenDto program)
        //        => program.Rubvars.Select(v => new RubGridRow
        //        {
        //            Id = v.Id,
        //            Irub = v.Id,
        //            Scdrub = v.Scdrub,
        //            Liba = v.Liba,
        //            Abg = v.Liba
        //        }).ToList();

        //    private List<RubFmtRow> BuildRubFmtRows(PlngenDto program)
        //        => program.Rubvars
        //            .SelectMany(v => v.Rubfmts.Select(f => new RubFmtRow
        //            {
        //                Id = f.Id,
        //                Irub = v.Id,
        //                Ifmt = f.Id,
        //                Scdfmt = f.Zcdrub,
        //                Liba = f.Liba,
        //                Ftsrc = f.Ftsrc
        //            }))
        //            .ToList();

        //    private List<ActdetDto> EvaluateRubFmtRows(CalcContext ctx, RubvarDto rub, List<RubFmtRow> fmtRows)
        //        => fmtRows.Select(r =>
        //        {
        //            var result = _engine.Evaluate(r.Ftsrc, BuildEvalContext(ctx));
        //            return new ActdetDto
        //            {
        //                Itie = ctx.Itie,
        //                Iact = 0,
        //                Ipln = ctx.Ipln,
        //                Irub = rub.Id,
        //                Ifmt = r.Ifmt,
        //                Atyp = rub.Atyp,
        //                Inptvalue = result?.Value?.ToString(),
        //                Aval = result?.Raw,
        //                Iraw = Trunc1000(result?.Raw)
        //            };
        //        }).ToList();

        //    private ActsaieDto EvaluateRubVarRow(CalcContext ctx, RubvarDto rub, List<ActdetDto> dets)
        //    {
        //        var result = _engine.Evaluate(rub.Frsrc, BuildEvalContext(ctx));

        //        return new ActsaieDto
        //        {
        //            Itie = ctx.Itie,
        //            Ipln = ctx.Ipln,
        //            Irub = rub.Id,
        //            Atyp = rub.Atyp,
        //            Inptvalue = result?.Value?.ToString(),
        //            Aval = result?.Raw,
        //            Iraw = Trunc1000(result?.Raw),
        //            Actdets = dets
        //        };
        //    }

        //    private ResdonDto CreateResdon(CalcContext ctx, ProgramLine line, FormulaResult result) => new()
        //    {
        //        Itie = ctx.Itie,
        //        Ipln = ctx.Ipln,
        //        Irub = line.Irub,
        //        Atyp = result.Type.HasValue ? (int)result.Type.Value : 0,
        //        Inptvalue = result.Value?.ToString(),
        //        Aval = result.Raw,
        //        Iraw = Trunc1000(result.Raw)
        //    };

        //    private ResbroDto CreateResbro(CalcContext ctx, ProgramLine line, FormulaResult result) => new()
        //    {
        //        Itie = ctx.Itie,
        //        Ipln = ctx.Ipln,
        //        Irub = line.Irub,
        //        Atyp = result.Type.HasValue ? (int)result.Type.Value : 0,
        //        Inptvalue = result.Value?.ToString(),
        //        Aval = result.Raw,
        //        Iraw = Trunc1000(result.Raw)
        //    };

        //    private ResdetDto CreateResdet(CalcContext ctx, ProgramLine line, FormulaResult result) => new()
        //    {
        //        Itie = ctx.Itie,
        //        Ires = 0,
        //        Ipln = ctx.Ipln,
        //        Irub = line.Irub,
        //        Ifmt = line.Ifmt,
        //        Atyp = result.Type.HasValue ? (int)result.Type.Value : 0,
        //        Inptvalue = result.Value?.ToString(),
        //        Aval = result.Raw,
        //        Iraw = Trunc1000(result.Raw)
        //    };

        //    private OutDataLineStream CreateOutputStream(CalcContext ctx, ProgramLine line, FormulaResult result) => new()
        //    {
        //        Itie = ctx.Itie,
        //        Ipln = ctx.Ipln,
        //        Irub = line.Irub,
        //        Atyp = result.Type.HasValue ? (int)result.Type.Value : 0,
        //        Inptvalue = result.Value?.ToString(),
        //        Aval = result.Raw,
        //        Iraw = Trunc1000(result.Raw)
        //    };

        //    private FormulaEvaluationContext BuildEvalContext(CalcContext ctx) => new()
        //    {
        //        Idorg = ctx.Idorg,
        //        Ipln = ctx.Ipln,
        //        Itie = ctx.Itie
        //    };
        //    private List<ProgramLine> ParseProgramLines(string? fpsrc)
        //    {
        //        var lines = new List<ProgramLine>();

        //        if (string.IsNullOrWhiteSpace(fpsrc))
        //            return lines;

        //        var rawLines = fpsrc
        //            .Replace("\r\n", "\n")
        //            .Replace('\r', '\n')
        //            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        //        foreach (var raw in rawLines)
        //        {
        //            var parsed = ParseProgramLine(raw);
        //            if (parsed != null)
        //                lines.Add(parsed);
        //        }

        //        return lines;
        //    }
        //    private ProgramLine? ParseProgramLine(string raw)
        //    {
        //        var line = raw.Trim();
        //        if (string.IsNullOrWhiteSpace(line))
        //            return null;

        //        var atIndex = line.IndexOf('@');
        //        if (atIndex < 0)
        //            return null;

        //        line = line[(atIndex + 1)..].Trim();

        //        var colonIndex = line.IndexOf(':');
        //        if (colonIndex <= 0)
        //            return null;

        //        if (!int.TryParse(line[..colonIndex].Trim(), out var lineNumber))
        //            return null;

        //        var exprPart = line[(colonIndex + 1)..].Trim();

        //        string? meta = null;
        //        var metaStart = exprPart.LastIndexOf('[');
        //        var metaEnd = exprPart.LastIndexOf(']');

        //        if (metaStart >= 0 && metaEnd > metaStart)
        //        {
        //            meta = exprPart[(metaStart + 1)..metaEnd].Trim();
        //            exprPart = exprPart[..metaStart].Trim();
        //        }

        //        if (exprPart.EndsWith(";"))
        //            exprPart = exprPart[..^1].TrimEnd();

        //        if (string.IsNullOrWhiteSpace(exprPart))
        //            return null;

        //        return new ProgramLine
        //        {
        //            LineNumber = lineNumber,
        //            Formula = exprPart,
        //            Meta = meta
        //        };
        //    }
        //    private List<ProgramLine> EnrichProgramLines(
        //List<ProgramLine> lines,
        //IReadOnlyCollection<RubvarDto> rubvars,
        //IReadOnlyCollection<RubfmtDto> rubfmts)
        //    {
        //        var rubvarByLine = rubvars
        //.Select(x => new { Item = x, Ok = int.TryParse(x.Scdrub, out var n), Num = n })
        //.Where(x => x.Ok)
        //.GroupBy(x => x.Num)
        //.ToDictionary(g => g.Key, g => g.First().Item);

        //        var rubfmtByLine = rubfmts
        //            .Select(x => new { Item = x, Ok = int.TryParse(x.Zcdrub, out var n), Num = n })
        //            .Where(x => x.Ok)
        //            .GroupBy(x => x.Num)
        //            .ToDictionary(g => g.Key, g => g.First().Item);

        //        foreach (var line in lines)
        //        {
        //            rubvarByLine.TryGetValue(line.LineNumber ?? 0, out var rubvar);
        //            rubfmtByLine.TryGetValue(line.LineNumber ?? 0, out var rubfmt);

        //            line.Irub = rubvar?.Id;
        //            line.Ifmt = rubfmt?.Id;
        //            line.Type = MapType(line.Meta, rubvar, rubfmt);
        //            line.Liba = rubfmt?.Liba ?? rubvar?.Liba;
        //            line.SaveDetail = ShouldSaveDetail(line.Meta, rubvar, rubfmt);
        //        }

        //        return lines;
        //    }
        //public enum PendingSaveMode
        //{
        //    Immediate,
        //    Buffered
        //}

        //public enum LineType
        //{
        //    Decimal,
        //    Int,
        //    Date,
        //    Bool
        //}

        //public interface IPendingChangesGuard
        //{
        //    Task TrackInsert<TDto>(string entitySet, TDto item) where TDto : class;
        //    Task FlushAsync();
        //    PendingSaveMode GetSaveMode();
        //}

        //public sealed class FormulaEngine
        //{
        //    public FormulaResult? Evaluate(string? formula, FormulaEvaluationContext ctx)
        //    {
        //        if (string.IsNullOrWhiteSpace(formula))
        //            return null;

        //        return new FormulaResult
        //        {
        //            Value = formula,
        //            Raw = formula,
        //            Type = null
        //        };
        //    }
        //}

        //public sealed class FormulaResult
        //{
        //    public object? Value { get; init; }
        //    public object? Raw { get; init; }
        //    public LineType? Type { get; init; }
        //}
        //public sealed class RubvarEvaluationScope
        //{
        //    public RubvarDto Rubvar { get; init; } = new();
        //    public IReadOnlyCollection<RubfmtDto> Rubfmts { get; init; } = Array.Empty<RubfmtDto>();
        //}
        //public sealed class FormulaEvaluationContext
        //{
        //    public int? Idorg { get; init; }
        //    public int? Ipln { get; init; }
        //    public int? Itie { get; init; }
        //    public DateTime SessionDate { get; init; }

        //    public IDictionary<string, object?> InputData { get; init; } = new Dictionary<string, object?>();
        //    public IDictionary<string, object?> OutputData { get; init; } = new Dictionary<string, object?>();
        //    public RubvarEvaluationScope? Scope { get; init; }
        //    public Func<string, object?>? ResolveVariable { get; init; }
        //}

        //public sealed class CalcContext
        //{
        //    public PlngenDto Program { get; init; } = new();
        //    public TierspDto Tier { get; init; } = new();
        //    public int? Idorg { get; init; }
        //    public int? Ipln { get; init; }
        //    public int? Itie { get; init; }
        //    public bool IsTestMode { get; init; }
        //}

        //public sealed class SaieSession
        //{
        //    public PlngenDto Program { get; set; } = new();
        //    public TierspDto Tier { get; set; } = new();
        //    public List<RubGridRow> RubVarRows { get; set; } = new();
        //    public List<RubFmtRow> RubFmtRows { get; set; } = new();
        //    public List<ActsaieDto> Actsaies { get; set; } = new();
        //    public List<ActdetDto> Actdets { get; set; } = new();
        //}

        //public sealed class CalcSession
        //{
        //    public PlngenDto Program { get; set; } = new();
        //    public TierspDto Tier { get; set; } = new();
        //    public IDictionary<string, object?> Inputs { get; set; } = new Dictionary<string, object?>();
        //    public IDictionary<string, object?> Outputs { get; set; } = new Dictionary<string, object?>();
        //    public List<ResdonDto> Resdons { get; set; } = new();
        //    public List<ResbroDto> Resbros { get; set; } = new();
        //    public List<ResdetDto> Resdets { get; set; } = new();
        //}

        //public sealed class PlngenDto
        //{
        //    public string? Fpsrc { get; set; }
        //    public List<RubvarDto> Rubvars { get; set; } = new();
        //    public List<RubfmtDto> Rubfmts { get; set; } = new();
        //}

        //public sealed class TierspDto
        //{
        //}

        //public sealed class RubvarDto
        //{
        //    public int Id { get; set; }
        //    public string? Scdrub { get; set; }
        //    public string? Liba { get; set; }
        //    public int? Atyp { get; set; }
        //    public string? Frsrc { get; set; }
        //    public List<RubfmtDto> Rubfmts { get; set; } = new();
        //}

        //public sealed class RubfmtDto
        //{
        //    public int Id { get; set; }
        //    public string? Zcdrub { get; set; }
        //    public string? Liba { get; set; }
        //    public string? Ftsrc { get; set; }
        //}

        //public sealed class RubGridRow
        //{
        //    public int Id { get; set; }
        //    public int? Irub { get; set; }
        //    public string? Scdrub { get; set; }
        //    public string? Liba { get; set; }
        //    public object? Abg { get; set; }
        //    public object? Aval { get; set; }
        //}

        //public sealed class RubFmtRow
        //{
        //    public int Id { get; set; }
        //    public int? Irub { get; set; }
        //    public int? Ifmt { get; set; }
        //    public string? Scdfmt { get; set; }
        //    public string? Liba { get; set; }
        //    public string? Ftsrc { get; set; }
        //}

        //public sealed class ActsaieDto
        //{
        //    public int? Itie { get; set; }
        //    public int? Ipln { get; set; }
        //    public int Irub { get; set; }
        //    public int? Atyp { get; set; }
        //    public string? Inptvalue { get; set; }
        //    public object? Aval { get; set; }
        //    public decimal? Iraw { get; set; }
        //    public List<ActdetDto> Actdets { get; set; } = new();
        //}

        //public sealed class ActdetDto
        //{
        //    public int? Itie { get; set; }
        //    public int Iact { get; set; }
        //    public int? Ipln { get; set; }
        //    public int Irub { get; set; }
        //    public int? Ifmt { get; set; }
        //    public int? Atyp { get; set; }
        //    public string? Inptvalue { get; set; }
        //    public object? Aval { get; set; }
        //    public decimal? Iraw { get; set; }
        //}

        //public sealed class ResdonDto
        //{
        //    public int? Itie { get; set; }
        //    public int? Ipln { get; set; }
        //    public int Irub { get; set; }
        //    public int Atyp { get; set; }
        //    public string? Inptvalue { get; set; }
        //    public object? Aval { get; set; }
        //    public decimal? Iraw { get; set; }
        //}

        //public sealed class ResbroDto
        //{
        //    public int? Itie { get; set; }
        //    public int? Ipln { get; set; }
        //    public int Irub { get; set; }
        //    public int Atyp { get; set; }
        //    public string? Inptvalue { get; set; }
        //    public object? Aval { get; set; }
        //    public decimal? Iraw { get; set; }
        //}

        //public sealed class ResdetDto
        //{
        //    public int? Itie { get; set; }
        //    public int Ires { get; set; }
        //    public int? Ipln { get; set; }
        //    public int Irub { get; set; }
        //    public int? Ifmt { get; set; }
        //    public int Atyp { get; set; }
        //    public string? Inptvalue { get; set; }
        //    public object? Aval { get; set; }
        //    public decimal? Iraw { get; set; }
        //}

        //public sealed class OutDataLineStream
        //{
        //    public int? Itie { get; set; }
        //    public int? Ipln { get; set; }
        //    public int Irub { get; set; }
        //    public int Atyp { get; set; }
        //    public string? Inptvalue { get; set; }
        //    public object? Aval { get; set; }
        //    public string? Iraw { get; set; }
        //}
        //    private static LineType MapType(string? meta, RubvarDto? rubvar, RubfmtDto? rubfmt)
        //    {
        //        if (rubvar?.Atyp != null)
        //        {
        //            return rubvar.Atyp.Value switch
        //            {
        //                1 => LineType.Int,
        //                2 => LineType.Decimal,
        //                3 => LineType.Date,
        //                4 => LineType.Bool,
        //                _ => LineType.Decimal
        //            };
        //        }

        //        return meta?.ToLowerInvariant() switch
        //        {
        //            "i" => LineType.Int,
        //            "d" => LineType.Decimal,
        //            "w" => LineType.Decimal,
        //            _ => LineType.Decimal
        //        };
        //    }
        //    private object? ParseResultValue(string? aval, string? rowType, string? satyp)
        //    {
        //        var type = !string.IsNullOrWhiteSpace(rowType) ? rowType : satyp;
        //        if (string.IsNullOrWhiteSpace(type))
        //            return aval;

        //        return type.Trim().ToLowerInvariant() switch
        //        {
        //            "int" => int.TryParse(aval, out var i) ? i : aval,
        //            "decimal" => decimal.TryParse(aval, out var d) ? d : aval,
        //            "real" => double.TryParse(aval, out var r) ? r : aval,
        //            "date" => DateTime.TryParse(aval, out var dt) ? dt : aval,
        //            "bool" => bool.TryParse(aval, out var b) ? b : aval,
        //            "string" => aval,
        //            _ => aval
        //        };
        //    }
        //    private static bool ShouldSaveDetail(string? meta, RubvarDto? rubvar, RubfmtDto? rubfmt)
        //    {
        //        if (!string.IsNullOrWhiteSpace(meta))
        //            return true;

        //        return rubfmt != null || rubvar != null;
        //    }

        //    public interface ICalculationWorkflow
        //    {
        //        Task<SaieSession> CalculateSaieAsync(CalcContext ctx);
        //        Task<List<CalcSession>> CalculateCalcAsync(IEnumerable<CalcContext> contexts);
        //    }
        //    public sealed class CalculationWorkflow : ICalculationWorkflow
        //    {
        //        private readonly ICalculationService _calcService;

        //        public CalculationWorkflow(ICalculationService calcService)
        //        {
        //            _calcService = calcService;
        //        }

        //        public async Task<SaieSession> CalculateSaieAsync(CalcContext ctx)
        //        {
        //            var session = await _calcService.BuildSaieAsync(ctx);
        //            session = await _calcService.RecomputeSaieAsync(ctx, session);
        //            return session;
        //        }

        //public Task<CalcSession> RunCalcAsync(CalcContext ctx, CalcSession session)
        //{
        //    var evalCtx = BuildEvalContext(ctx);
        //    var lines = _lineParser.Parse(new PlngenLineSource(ctx.Program));

        //    foreach (var line in lines)
        //    {
        //        var result = _engine.Evaluate(line.Formula, evalCtx);
        //        if (result is null)
        //            continue;
        //        session.Outputs[line.LineNumber ?? 0] = result.Raw;
        //        session.Outputs[line.Formula ?? Guid.NewGuid().ToString()] = result.Raw;

        //        if (ctx.IsTestMode)
        //        {
        //            session.Resbros.Add(CreateResbro(ctx, line, result));
        //        }
        //        else
        //        {
        //            session.Resdons.Add(CreateResdon(ctx, line, result));

        //            if (line.SaveDetail)
        //                session.Resdets.Add(CreateResdet(ctx, line, result));
        //        }
        //    }

        //    return Task.FromResult(session);
        //}
        //        public async Task<List<CalcSession>> CalculateCalcAsync(IEnumerable<CalcContext> contexts)
        //        {
        //            var results = new List<CalcSession>();

        //            foreach (var ctx in contexts)
        //            {
        //                var session = new CalcSession
        //                {
        //                    Program = ctx.Program,
        //                    Tier = ctx.Tier
        //                };

        //                session = await _calcService.RunCalcAsync(ctx, session);
        //                results.Add(session);
        //            }

        //            return results;
        //        }
        //    }
        //}
    }    
}