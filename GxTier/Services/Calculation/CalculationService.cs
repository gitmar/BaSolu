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
                Scdrub = line.LineNumber?.ToString(),
                Atyp = result.Type.HasValue ? (int)result.Type.Value : 0,
                Inptvalue = result.Value?.ToString(),
                Aval = result.Raw?.ToString(),
                Iraw = MyConverters.Trunc1000(result.Raw)?.ToString() ?? string.Empty
            };

        public static ResdonDto MapToResdon(CalcContext ctx, ProgramLine line, FormulaResult result)
            => new()
            {
                Itie = ctx.Itie,
                Ipln = ctx.Ipln ?? 0,
                Irub = line.Irub ?? 0,
                Scdrub = line.LineNumber?.ToString(),
                Atyp = result.Type.HasValue ? (int)result.Type.Value : 0,
                Inptvalue = result.Value?.ToString(),
                Aval = result.Raw?.ToString(),
                Iraw = MyConverters.Trunc1000(result.Raw)?.ToString() ?? string.Empty
            };

        public static ResbroDto MapToResbro(CalcContext ctx, ProgramLine line, FormulaResult result)
            => new()
            {
                Itie = ctx.Itie,
                Ipln = ctx.Ipln ?? 0,
                Irub = line.Irub ?? 0,
                Scdrub = line.LineNumber?.ToString(),
                Atyp = result.Type.HasValue ? (int)result.Type.Value : 0,
                Inptvalue = result.Value?.ToString(),
                Aval = result.Raw?.ToString(),
                Iraw = MyConverters.Trunc1000(result.Raw)?.ToString() ?? string.Empty
            };

        public static ResdetDto MapToResdet(CalcContext ctx, ProgramLine line, FormulaResult result)
            => new()
            {
                Itie = ctx.Itie ?? 0,
                Ires = 0,
                Ipln = ctx.Ipln ?? 0,
                Irub = line.Irub ?? 0,
                Ifmt = line.Ifmt ?? 0,
                Scdrub = line.LineNumber?.ToString() ?? string.Empty, // ResdetDto.Scdrub is [Required] non-null string
                Zcdrub = line.DetailCode,                              // was never set — needed to identify which detail this is
                Atyp = result.Type.HasValue ? (int)result.Type.Value : 0,
                Inptvalue = result.Value?.ToString(),
                Aval = result.Raw?.ToString(),
                Iraw = MyConverters.Trunc1000(result.Raw)?.ToString() ?? string.Empty
            };

        public static ResbdetDto MapToResbdet(CalcContext ctx, ProgramLine line, FormulaResult result)
            => new()
            {
                Itie = ctx.Itie ?? 0,
                Ibro = 0,
                Ipln = ctx.Ipln ?? 0,
                Irub = line.Irub ?? 0,
                Ifmt = line.Ifmt ?? 0,
                Scdrub = line.LineNumber?.ToString() ?? string.Empty,
                Zcdrub = line.DetailCode,
                Atyp = result.Type.HasValue ? (int)result.Type.Value : 0,
                Inptvalue = result.Value?.ToString(),
                Aval = result.Raw?.ToString(),
                Iraw = MyConverters.Trunc1000(result.Raw)?.ToString() ?? string.Empty
            };
    }
}