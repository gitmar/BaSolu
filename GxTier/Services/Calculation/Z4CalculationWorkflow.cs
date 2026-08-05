using GxFormula.Forasource;

using GxShared.GxDtos;
using GxShared.Sess;
using GxShared.GxGuards;

namespace GxTie.Services.Calculation
{
    //public interface ICalculationWorkflow
    //{
    //    Task<SaieSession> LoadSaieAsync(PlngenDto program, TierspDto tier, List<Gtabl> ensTbls);
    //    Task<SaieSession> CalculateSaieAsync(CalcContext ctx, SaieSession session);
    //    Task<SaieSession> CalculateAndSaveSaieAsync(CalcContext ctx, SaieSession session, PendingSaveMode inSaveMode);

    //    Task<List<CalcSession>> CalculateCalcAsync(IEnumerable<CalcContext> contexts);
    //    Task<List<CalcSession>> CalculateAndSaveCalcAsync(IEnumerable<CalcContext> contexts);
    //}

    //public sealed class CalculationWorkflow : ICalculationWorkflow
    //{
    //    private readonly ICalculationService _calcService;
    //    private readonly ICalculationPersistence _persistence;

    //    public CalculationWorkflow(
    //        ICalculationService calcService,
    //        ICalculationPersistence persistence)
    //    {
    //        _calcService = calcService;
    //        _persistence = persistence;
    //        //_ensTbls = ensTbls;
    //    }

    //    public async Task<SaieSession> LoadSaieAsync(PlngenDto program, TierspDto tier, List<Gtabl> ensTbls)
    //    {
    //        // Use the internal calculator directly or expose via ICalculationService
    //        // For now, create session inline (or inject ISaieCalculator)
    //        var session = new SaieSession
    //        {
    //            Program = program,
    //            Tier = tier,
    //            EnsTbls = ensTbls,
    //            RubVarRows = program.Rubvars.Select(v => new RubVarRow
    //            {
    //                Id = v.Id,
    //                Irub = v.Id,
    //                Scdrub = v.Scdrub,
    //                Atyp = v.Atyp ?? 0,
    //                Vgpe = v.Vgpe,
    //                Liba = v.Liba,
    //                Abg = v.Liba,
    //                InputValue = string.Empty,
    //                Iraw = string.Empty,
    //                //SourceRubvar = v,
    //                Details = v.Rubfmts.Select(f => new RubFmtRow
    //                {
    //                    Id = f.Id,
    //                    Irub = v.Id,
    //                    Ifmt = f.Id,
    //                    Scdfmt = f.Zcdrub,
    //                    Atyp = f.Ztyp ?? 0,
    //                    Vgpe = f.Vgpe,
    //                    Liba = f.Liba,
    //                    Ftsrc = f.Ftsrc,
    //                    InputValue = string.Empty,
    //                    Iraw = string.Empty
    //                    ////SourceRubfmt = f
    //                }).ToList()
    //            }).ToList(),
    //            RubFmtRows = program.Rubvars
    //                .SelectMany(v => v.Rubfmts.Select(f => new RubFmtRow
    //                {
    //                    Id = f.Id,
    //                    Irub = v.Id,
    //                    Ifmt = f.Id,
    //                    Scdfmt = f.Zcdrub,
    //                    Atyp = f.Ztyp ?? 0,
    //                    Vgpe = f.Vgpe,
    //                    Liba = f.Liba,
    //                    Ftsrc = f.Ftsrc,
    //                    InputValue = string.Empty,
    //                    Iraw = string.Empty
    //                    ////SourceRubfmt = f
    //                }))
    //                .ToList()
    //        };

    //        return session;
    //    }

    //    public async Task<SaieSession> CalculateSaieAsync(CalcContext ctx, SaieSession session)
    //        => await _calcService.CalculateSaieAsync(ctx, session);

    //    public async Task<SaieSession> CalculateAndSaveSaieAsync(CalcContext ctx, SaieSession session, PendingSaveMode inSaveMode)
    //    {
    //        session = await _calcService.CalculateSaieAsync(ctx, session);
    //        await _persistence.SaveSaieAsync(ctx, session, inSaveMode);
    //        return session;
    //    }
    //    public async Task<List<CalcSession>> CalculateCalcAsync(IEnumerable<CalcContext> contexts)
    //    {
    //        var results = new List<CalcSession>();

    //        foreach (var ctx in contexts)
    //        {
    //            var session = new CalcSession
    //            {
    //                Program = ctx.Program,
    //                Tier = ctx.Tier
    //            };

    //            session = await _calcService.RunCalcAsync(ctx, session);
    //            results.Add(session);
    //        }

    //        return results;
    //    }

    //    public async Task<List<CalcSession>> CalculateAndSaveCalcAsync(IEnumerable<CalcContext> contexts)
    //    {
    //        var results = new List<CalcSession>();

    //        foreach (var ctx in contexts)
    //        {
    //            var session = new CalcSession
    //            {
    //                Program = ctx.Program,
    //                Tier = ctx.Tier
    //            };

    //            session = await _calcService.RunCalcAsync(ctx, session);
    //            await _persistence.SaveCalcAsync(ctx, session);
    //            results.Add(session);
    //        }

    //        return results;
    //    }
    //}
    // ... rest of methods unchanged
}
    //public interface ICalculationWorkflow
    //{
    //    Task<SaieSession> LoadSaieAsync(PlngenDto program, TierspDto tier);
    //    Task<SaieSession> CalculateSaieAsync(CalcContext ctx, SaieSession session);
    //    Task<SaieSession> CalculateAndSaveSaieAsync(CalcContext ctx, SaieSession session, PendingSaveMode inSaveMode);

    //    Task<List<CalcSession>> CalculateCalcAsync(IEnumerable<CalcContext> contexts);
    //    Task<List<CalcSession>> CalculateAndSaveCalcAsync(IEnumerable<CalcContext> contexts);
    //}
    //public sealed class CalculationWorkflow : ICalculationWorkflow
    //{
    //    private readonly ICalculationService _calcService;
    //    private readonly ICalculationPersistence _persistence;

    //    public CalculationWorkflow(
    //        ICalculationService calcService,
    //        ICalculationPersistence persistence)
    //    {
    //        _calcService = calcService;
    //        _persistence = persistence;
    //    }

    //    public Task<SaieSession> LoadSaieAsync(PlngenDto program, TierspDto tier)
    //    {
    //        var session = new SaieSession
    //        {
    //            Program = program,
    //            Tier = tier,
    //            RubVarRows = program.Rubvars.Select(v => new RubVarRow
    //            {
    //                Id = v.Id,
    //                Irub = v.Id,
    //                Scdrub = v.Scdrub,
    //                Atyp = v.Atyp ?? 0,
    //                Vgpe = v.Vgpe,
    //                Liba = v.Liba,
    //                Abg = v.Liba,
    //                InputValue = string.Empty,
    //                Iraw = string.Empty,
    //                SourceRubvar = v,
    //                Details = v.Rubfmts.Select(f => new RubFmtRow
    //                {
    //                    Id = f.Id,
    //                    Irub = v.Id,
    //                    Ifmt = f.Id,
    //                    Scdfmt = f.Zcdrub,
    //                    Atyp = f.Ztyp ?? 0,
    //                    Vgpe = f.Vgpe,
    //                    Liba = f.Liba,
    //                    Ftsrc = f.Ftsrc,
    //                    InputValue = string.Empty,
    //                    Iraw = string.Empty,
    //                    SourceRubfmt = f
    //                }).ToList()
    //            }).ToList()
    //        };

    //        return Task.FromResult(session);
    //    }

    //    public async Task<SaieSession> CalculateSaieAsync(CalcContext ctx, SaieSession session)
    //    {
    //        return await _calcService.CalculateSaieAsync(ctx, session);
    //    }

    //    public async Task<SaieSession> CalculateAndSaveSaieAsync(CalcContext ctx, SaieSession session, PendingSaveMode inSaveMode)
    //    {
    //        session = await _calcService.CalculateSaieAsync(ctx, session);
    //        await _persistence.SaveSaieAsync(ctx, session, inSaveMode);
    //        return session;
    //    }

    //    public async Task<List<CalcSession>> CalculateCalcAsync(IEnumerable<CalcContext> contexts)
    //    {
    //        var results = new List<CalcSession>();

    //        foreach (var ctx in contexts)
    //        {
    //            var session = new CalcSession
    //            {
    //                Program = ctx.Program,
    //                Tier = ctx.Tier
    //            };

    //            session = await _calcService.RunCalcAsync(ctx, session);
    //            results.Add(session);
    //        }

    //        return results;
    //    }

    //    public async Task<List<CalcSession>> CalculateAndSaveCalcAsync(IEnumerable<CalcContext> contexts)
    //    {
    //        var results = new List<CalcSession>();

    //        foreach (var ctx in contexts)
    //        {
    //            var session = new CalcSession
    //            {
    //                Program = ctx.Program,
    //                Tier = ctx.Tier
    //            };

    //            session = await _calcService.RunCalcAsync(ctx, session);
    //            await _persistence.SaveCalcAsync(ctx, session);
    //            results.Add(session);
    //        }

    //        return results;
    //    }
    //}
//}