using GxFormula.Forasource;

using GxShared.GxDtos;

using GxTie.StaticHelpers;

namespace GxTie.Services.Calculation
{
    public interface ICalculationService
    {
        Task<SaieSession> CalculateSaieAsync(CalcContext ctx, SaieSession session);
        Task<CalcSession> RunCalcAsync(CalcContext ctx, CalcSession session);
    }
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