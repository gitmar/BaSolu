using System.Globalization;

namespace GxTie.Services.Uicontrols
{
    public static class StringExtensions
    {
        public static (object? result, bool success) ConvertUserInputSafe(this string? value, int atyp)
        {
            value = value?.Trim();

            if (string.IsNullOrWhiteSpace(value))
                return (null, false);

            switch (atyp)
            {
                case 1:
                    return (value, true);

                case 2:
                    if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i))
                        return (i, true);
                    return (null, false);

                case 3:
                    if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var d))
                        return (d, true);
                    return (null, false);

                case 4:
                    if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                        return (dt, true);
                    return (null, false);

                case 5:
                    if (bool.TryParse(value, out var b))
                        return (b, true);
                    return (null, false);

                default:
                    return (value, true);
            }
        }
    }
}