using GxFormula.Forasource;

using GxShared.GxDtos;
using GxShared.GxGuards;
using GxShared.Sess;

namespace GxTie.Services.Calculation
{
    public interface ICalculationWorkflow
    {
        Task<SaieSession> CreateSaieSessionAsync(PlngenDto program, TierspDto tier, List<Gtabl> ensTbls);
        Task<SaieSession> CalculateSaieAsync(CalcContext ctx, SaieSession session);
        Task<SaieSession> CalculateAndSaveSaieAsync(
            CalcContext ctx, SaieSession session, PendingSaveMode inSaveMode);
        Task<List<CalcSession>> CalculateCalcAsync(IEnumerable<CalcContext> contexts);
        Task<List<CalcSession>> CalculateAndSaveCalcAsync(IEnumerable<CalcContext> contexts);
    }
    public sealed class CalculationWorkflow : ICalculationWorkflow
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
            PlngenDto program, TierspDto tier, List<Gtabl> ensTbls)
            => _saieCalculator.InitializeAsync(program, tier, ensTbls);

        public Task<SaieSession> CalculateSaieAsync(CalcContext ctx, SaieSession session)
            => _calcService.CalculateSaieAsync(ctx, session);

        public async Task<SaieSession> CalculateAndSaveSaieAsync(
            CalcContext ctx, SaieSession session, PendingSaveMode inSaveMode)
        {
            session = await _calcService.CalculateSaieAsync(ctx, session);
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
    }
}
