using GxShared.Others;

namespace GxTie.StaticHelpers
{
    public static class ObjectExtensions
    {
        public static TResult Let<T, TResult>(this T? obj, Func<T, TResult> func)
            where T : class => obj is null ? default! : func(obj);
    }
    public static class OptionSelectors
    {
        // Statut (int?)
        //private Func<Gptbl, int> StatutValue => g => g.Elea ?? 0;
        //private Func<Gptbl, string> StatutLabel => g => g.Liba ?? "";

        //public static readonly Func<Gptbl, int> StatutValue = g => g.Elea ?? 0;
        //public static readonly Func<Gptbl, string> StatutLabel = g => g.Liba ?? "";

        // Relat (int?)
        public static readonly Func<Gptbl, int> RelatValue = g => g.Elea ?? 0;
        public static readonly Func<Gptbl, string> RelatLabel = g => g.Liba ?? "";

        // Sexe (int?)
        public static readonly Func<Gptbl, int> SexeValue = g => g.Elea ?? 0;
        public static readonly Func<Gptbl, string> SexeLabel = g => g.Liba ?? "";

        // Sitmat (int?)
        public static readonly Func<Gptbl, int> SitmatValue = g => g.Elea ?? 0;
        public static readonly Func<Gptbl, string> SitmatLabel = g => g.Liba ?? "";

        // Ifonc (int?)
        public static readonly Func<Gptbl, int> IfoncValue = g => g.Elea ?? 0;
        public static readonly Func<Gptbl, string> IfoncLabel = g => g.Liba ?? "";

        // Igrad (int?)
        public static readonly Func<Gptbl, int> IgradValue = g => g.Elea ?? 0;
        public static readonly Func<Gptbl, string> IgradLabel = g => g.Liba ?? "";

        // Respon (int?)
        public static readonly Func<Gptbl, int> ResponValue = g => g.Elea ?? 0;
        public static readonly Func<Gptbl, string> ResponLabel = g => g.Liba ?? "";

        // Rpays (string)
        public static readonly Func<Gptbl, string> RpaysValue = g => g.Abg ?? "";
        public static readonly Func<Gptbl, string> RpaysLabel = g => g.Liba ?? "";

        // Nationalité (string)
        public static readonly Func<Gptbl, string> NatioValue = g => g.Abg ?? "";
        public static readonly Func<Gptbl, string> NatioLabel = g => g.Liba ?? "";
    }
}
