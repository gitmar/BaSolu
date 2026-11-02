namespace GxTie.Components.Cols
{
    public class ColCondits
    {
        public static bool IsSupported(int totyp, int efrm, int styp)
            => totyp is >= 1 and <= 6
            && efrm is 1 or 2
            && (totyp == 1 ? styp is >= 1 and <= 6 : styp == 1);
    }
}
