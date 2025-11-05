using System;
using System.Globalization;

namespace GxTie.Helpers
{
    public class AbouAtyp
    {
        public string mytestob = "test";
    }
    public static class UserInputExtensions
    {
        public static (object Result, bool Success) ConvertUserInputSafe(this string input, int Atyp, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.InvariantCulture;

            try
            {
                switch (Atyp)
                {
                    case 1: return (input, true);
                    case 2: if (int.TryParse(input, NumberStyles.Integer, culture, out var i)) return (i, true); break;
                    case 3: if (long.TryParse(input, NumberStyles.Integer, culture, out var l)) return (l, true); break;
                    case 4: if (double.TryParse(input, NumberStyles.Float | NumberStyles.AllowThousands, culture, out var d)) return (d, true); break;
                    case 5: if (decimal.TryParse(input, NumberStyles.Number, culture, out var dec)) return (dec, true); break;
                    case 6: if (bool.TryParse(input, out var b)) return (b, true); break;
                    case 7: if (DateTime.TryParse(input, culture, DateTimeStyles.None, out var dt)) return (dt, true); break;
                }
            }
            catch { }

            return (input, false);
        }
    }
}