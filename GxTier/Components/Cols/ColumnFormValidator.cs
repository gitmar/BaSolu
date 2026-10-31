using GxShared.Cols.Models;

namespace GxTie.Components.Cols
{
    public sealed class ColumnFormValidator
    {
        private readonly ColumnPreferencesClient _prefs;
        public ColumnFormValidator(ColumnPreferencesClient prefs) => _prefs = prefs;
        public async Task<List<string>> ValidateManyAsync(
            int idorg, IEnumerable<(ColPrefContext Ctx, object Model, string Title)> items)
        {
            var problems = new List<string>();
            var loaded = new Dictionary<(int, int, int), List<LSelCol>?>();

            static string LabelFor(List<LSelCol>? cols, string name)
                => cols?.FirstOrDefault(c => string.Equals(c.ColName, name, StringComparison.OrdinalIgnoreCase))
                       is { } col && !string.IsNullOrWhiteSpace(col.Ulabel) ? col.Ulabel : name;

            foreach (var (ctx, model, title) in items)
            {
                var key = (ctx.Totyp, ctx.Efrm, ctx.Styp);
                if (!loaded.TryGetValue(key, out var cols))
                {
                    try { cols = await _prefs.LoadAsync(idorg, ctx); }
                    catch (HttpRequestException) { cols = null; }
                    loaded[key] = cols;
                }

                var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // 1. intrinsic: needs no configuration, so it runs even if loading failed
                foreach (var name in ColumnRules.GetRequiredByDto(model))
                    if (seen.Add(name)) problems.Add($"{title} : {LabelFor(cols, name)}");

                // 2. organization-required columns
                if (cols is null)
                {
                    problems.Add($"{title} : configuration des colonnes indisponible");
                    continue;
                }

                foreach (var c in ColumnRules.GetMissing(model, cols))
                    if (seen.Add(c.ColName)) problems.Add($"{title} : {LabelFor(cols, c.ColName)}");
            }
            return problems;
        }
    }
}
