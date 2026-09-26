using GxFormula.Forasource;

using GxShared.GxDtos;
using GxShared.GxGuards;
using GxShared.Sess;

namespace GxTie.Services.Calculation
{
    public interface ZICalculationWorkflow
    {
        Task<SaieSession> CreateSaieSessionAsync(PlngenDto program, TierspDto tier, List<Gtabl> ensTbls, List<Gpgrid> ensGpdata);
        Task<List<CalcSession>> CalculateCalcAsync(IEnumerable<CalcContext> contexts);
        Task<List<SaieSession>> CalculateSaieAsync(IEnumerable<CalcContext> contexts);
        Task<CalcSession> CalculateTrackAsync(CalcContext ctx, CalcSession session);
        Task<CalcSession> CalculateAndTrackSaieAsync(
            CalcContext ctx, CalcSession session, PendingSaveMode inSaveMode);
        
        Task<List<CalcSession>> CalculateAndTrackCalcAsync(IEnumerable<CalcContext> contexts);
    }
    public sealed class CalculationWorkflow : ZICalculationWorkflow
    {
        private readonly ICalculationService _calcService;
        private readonly ICalculationPersistence _persistence;
        private readonly ISaieCalculator _saieCalculator;

        public CalculationWorkflow(
            ICalculationService calcService,
            ICalculationPersistence persistence,
            ISaieCalculator saieCalculator)
        {
            _calcService = calcService;
            _persistence = persistence;
            _saieCalculator = saieCalculator;
        }

        public Task<SaieSession> CreateSaieSessionAsync(
            PlngenDto program, TierspDto tier, List<Gtabl> ensTbls, List<Gpgrid> ensGpdata)
            => _saieCalculator.InitializeAsync(program, tier, ensTbls, ensGpdata);

        public Task<CalcSession> CalculateTrackAsync(CalcContext ctx, CalcSession session)
    => _calcService.CalculateTrackAsync(ctx, session);


        public async Task<CalcSession> CalculateAndTrackSaieAsync(
            CalcContext ctx, CalcSession session, PendingSaveMode inSaveMode)
        {
            session = await _calcService.CalculateTrackAsync(ctx, session);
            await _persistence.TrackSaieChangesAsync(ctx, session);
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
        public async Task<List<SaieSession>> CalculateSaieAsync(IEnumerable<CalcContext> contexts)
        {
            var results = new List<SaieSession>();

            foreach (var ctx in contexts)
            {
                var session = new SaieSession
                {
                    Program = ctx.Program,
                    Tier = ctx.Tier
                };

                session = await _calcService.RunCalcAsync(ctx, session);
                results.Add(session);
            }

            return results;
        }
        public async Task<List<CalcSession>> CalculateAndTrackCalcAsync(IEnumerable<CalcContext> contexts)
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
                await _persistence.TrackCalcChangesAsync(ctx, session);
                results.Add(session);
            }

            return results;
        }
    }
}
