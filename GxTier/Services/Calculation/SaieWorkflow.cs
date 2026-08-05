using GxFormula.Forasource;

using GxShared.GxDtos;
using GxShared.GxGuards;

namespace GxTie.Services.Calculation
{
    public interface ISaieWorkflowService
    {
        Task<SaieSession> LoadSaieAsync(PlngenDto program, TierspDto tier);
        Task<SaieSession> CalculateSaieAsync(CalcContext ctx, SaieSession session);
        Task<SaieSession> CalculateAndSaveSaieAsync(CalcContext ctx, SaieSession session, PendingSaveMode inSaveMode);
        Task SaveSaieAsync(CalcContext ctx, SaieSession session, PendingSaveMode inSaveMode);
    }
    public sealed class SaieWorkflowService : ISaieWorkflowService
    {
        private readonly ICalculationService _calcService;
        private readonly ICalculationPersistence _calcPersistence;
        private readonly ISaieSessionFactory _sessionFactory;

        public SaieWorkflowService(
            ICalculationService calcService,
            ICalculationPersistence calcPersistence,
            ISaieSessionFactory sessionFactory)
        {
            _calcService = calcService;
            _calcPersistence = calcPersistence;
            _sessionFactory = sessionFactory;
        }

        public Task<SaieSession> LoadSaieAsync(PlngenDto program, TierspDto tier)
            => Task.FromResult(_sessionFactory.Create(program, tier));

        public Task<SaieSession> CalculateSaieAsync(CalcContext ctx, SaieSession session)
            => _calcService.CalculateSaieAsync(ctx, session);

        public async Task<SaieSession> CalculateAndSaveSaieAsync(CalcContext ctx, SaieSession session, PendingSaveMode inSaveMode)
        {
            session = await _calcService.CalculateSaieAsync(ctx, session);
            await _calcPersistence.SaveSaieAsync(ctx, session, inSaveMode);
            return session;
        }

        public Task SaveSaieAsync(CalcContext ctx, SaieSession session, PendingSaveMode inSaveMode)
            => _calcPersistence.SaveSaieAsync(ctx, session, inSaveMode);
    }
    public interface ISaieSessionFactory
    {
        SaieSession Create(PlngenDto program, TierspDto tier);
    }

    public sealed class SaieSessionFactory : ISaieSessionFactory
    {
        public SaieSession Create(PlngenDto program, TierspDto tier)
        {
            return new SaieSession
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
                        Iraw = string.Empty
                        ////SourceRubfmt = f
                    }).ToList()
                }).ToList()
            };
        }
    }
}
