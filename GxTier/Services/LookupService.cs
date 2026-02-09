using System.Net.Http;
using System.Reflection;

using GxWapi.DaModels;

namespace GxTie.Services
{
    public class LookupService
    {
        public List<Gsglne> ColDefs { get; }  // ✅ PROPERTY declared at CLASS level
        private readonly List<Gsglne> _tblDefs; 
        private readonly List<Gsglne> _colDefs;
        public LookupService(List<Gsglne> colDefs, List<Gsglne> tblDefs)
        {
            ColDefs = colDefs ?? throw new ArgumentNullException(nameof(colDefs));  // ✅ Assign to PROPERTY
            _colDefs = ColDefs;  // ✅ Now works
            _tblDefs = tblDefs;
        }
        // NEW: Jacc-based visible fields
        public List<string> GetFieldsByJacc(int idorg, int jaccValue = 1)
        {
            return ColDefs
                .Where(g => g.Idorg == idorg && g.Scdrub != null && g.Jacc == jaccValue)
                .Select(g => g.Scdrub!)
                .Distinct()
                .OrderBy(name => name)
                .ToList();
        }
        // Primary lookups for tiewel fields (ztyp==8)
        public Task<Dictionary<string, List<LookupItem>>> GetTiewelLookupsAsync(int idorg)
        {
            var result = new Dictionary<string, List<LookupItem>>();
            var tiewelFields = _colDefs
                .Where(c => c.Idorg == idorg && c.Ztyp == 8 && !string.IsNullOrWhiteSpace(c.Scdrub))
                .ToList();

            foreach (var field in tiewelFields)
            {
                var tableItems = _tblDefs
                    .Where(item => item.Idorg == idorg && item.Pitb == field.Ztbl && item.Ydpd == 0)
                    .OrderBy(item => item.Yrng)
                    .Select(item => new LookupItem
                    { //items in gsglnes otyp=4,pitb=table
                        Value = item.Abg ?? item.Elea.ToString(),
                        Text = item.Liba ?? item.Scdrub,
                        Order = item.Pele ?? 0
                    })
                    .ToList();

                if (tableItems.Any())
                    result[field.Scdrub] = tableItems;
            }

            return Task.FromResult(result);
        }
        public Task<Dictionary<string, List<LookupItem>>> GetTiwaflLookupsAsync(int idorg)
        {
            var result = new Dictionary<string, List<LookupItem>>();
            var tiewelFields = _colDefs
                .Where(c => c.Idorg == idorg && c.Ztyp == 8 && !string.IsNullOrWhiteSpace(c.Scdrub))
                .ToList();

            foreach (var field in tiewelFields)
            {
                var tableItems = _tblDefs
                    .Where(item => item.Idorg == idorg && item.Pitb == field.Ztbl && item.Ydpd == 0)
                    .OrderBy(item => item.Yrng)
                    .Select(item => new LookupItem
                    { //items in gsglnes otyp=4,pitb=table
                        Value = item.Abg ?? item.Elea.ToString(),
                        Text = item.Liba ?? item.Scdrub,
                        Order = item.Pele ?? 0
                    })
                    .ToList();

                if (tableItems.Any())
                    result[field.Scdrub] = tableItems;
            }

            return Task.FromResult(result);
        }
        // Visible fields (jacc=1)
        public Dictionary<string, bool> GetVisibleFields(int idorg)
        {
            return _colDefs
                .Where(g => g.Idorg == idorg && !string.IsNullOrEmpty(g.Scdrub) && g.Jacc == 1)
                .Select(g => g.Scdrub)
                .Distinct()
                .ToDictionary(name => name, _ => true);
        }
        // 🔥 NEW: Generic property initialization for ANY entity
        public (Dictionary<string, string> strings, Dictionary<string, int?> ints, Dictionary<string, decimal?> decimals,
            Dictionary<string, string> labels)
            InitializeAllPropsWithLabels(int idorg, Type entityType, object? entity, IEnumerable<string> fieldNames)
        {
        //    InitializeAllProps(Type entityType, object? entity, IEnumerable<string> fieldNames)
        //{
            var stringProps = new Dictionary<string, string>();
            var intProps = new Dictionary<string, int?>();
            var decimalProps = new Dictionary<string, decimal?>();
            var labels = GetFieldLabels(idorg, fieldNames);  // 🔥 Auto-labels!

            foreach (var fieldName in fieldNames)
            {
                var prop = entityType.GetProperty(fieldName);
                if (prop == null)
                {
                    stringProps[fieldName] = "";
                    continue;
                }

                try
                {
                    if (entity == null)
                    {
                        stringProps[fieldName] = "";
                        continue;
                    }

                    var value = prop.GetValue(entity);

                    switch (prop.PropertyType.Name)
                    {
                        case "String":
                            stringProps[fieldName] = value?.ToString() ?? "";
                            break;
                        case "Int32":
                            intProps[fieldName] = value switch { int i => i, _ => null };
                            break;
                        case "Decimal":
                            decimalProps[fieldName] = value switch { decimal d => d, _ => null };
                            break;
                        default:
                            stringProps[fieldName] = value?.ToString() ?? "";
                            break;
                    }
                }
                catch
                {
                    stringProps[fieldName] = "";
                }
            }
            return (stringProps, intProps, decimalProps, labels);  // ✅ Exact tuple match
            //return (stringProps, intProps, decimalProps, labels);
        }
        // 🔥 NEW: Copy properties back to entity
        public void CopyPropsToEntity(object entity,
            Dictionary<string, string> stringProps,
            Dictionary<string, int?> intProps,
            Dictionary<string, decimal?> decimalProps)
        {
            var entityType = entity.GetType();

            foreach (var kvp in stringProps)
            {
                var prop = entityType.GetProperty(kvp.Key);
                if (prop?.PropertyType == typeof(string))
                {
                    try { prop.SetValue(entity, kvp.Value); } catch { }
                }
            }
            foreach (var kvp in intProps)
            {
                var prop = entityType.GetProperty(kvp.Key);
                if (prop?.PropertyType == typeof(int) || prop?.PropertyType == typeof(int?))
                {
                    try { prop.SetValue(entity, kvp.Value); } catch { }
                }
            }

            foreach (var kvp in decimalProps)
            {
                var prop = entityType.GetProperty(kvp.Key);
                if (prop?.PropertyType == typeof(decimal) || prop?.PropertyType == typeof(decimal?))
                {
                    try { prop.SetValue(entity, kvp.Value); } catch { }
                }
            }
        }
        // 🔥 NEW: Parser helpers
        public int? ParseInt(string? value) =>
            string.IsNullOrWhiteSpace(value) ? null : int.TryParse(value, out int result) ? result : null;
        public decimal? ParseDecimal(string? value) =>
            string.IsNullOrWhiteSpace(value) ? null : decimal.TryParse(value, out decimal result) ? result : null;

        // Single table lookup
        public Task<List<LookupItem>> GetTableItemsAsync(int tableId, int idorg)
        {
            var items = _colDefs
                .Where(item => item.Idorg == idorg && item.Pitb == tableId && item.Ydpd == 0)
                .OrderBy(item => item.Yrng)
                .Select(item => new LookupItem
                {
                    Value = item.Abg ?? item.Elea.ToString(),
                    Text = item.Liba ?? item.Scdrub,
                    Order = item.Yrng ?? 0
                })
                .ToList();
            return Task.FromResult(items);
        }
        public List<string> GetVisibleFieldsByJacc(int idorg, int jaccValue = 1)
        {
            return ColDefs
                .Where(g => g.Idorg == idorg
                         && g.Scdrub != null
                         && g.Jacc == jaccValue)
                .Select(g => g.Scdrub)
                .Distinct()
                .ToList();
        }
        public Dictionary<string, string> GetFieldLabels(int idorg, IEnumerable<string> fieldNames)
        {
            return fieldNames.ToDictionary(
                fieldName => fieldName,
                fieldName => GetFieldLabel(idorg, fieldName)  // Uses your single method
            );
        }
        public string GetFieldLabel(int idorg, string fieldName)
        {
            return ColDefs
                .Where(g => g.Idorg == idorg && g.Scdrub == fieldName)
                .OrderByDescending(g => !string.IsNullOrEmpty(g.Oliba))
                .ThenByDescending(g => !string.IsNullOrEmpty(g.Liba))
                .Select(g => g.Oliba ?? g.Liba ?? g.Scdrub!)
                .FirstOrDefault() ?? fieldName;
        }
    }
    // DTOs
    public class LookupItem
    {
        public string Value { get; set; } = "";
        public string Text { get; set; } = "";
        public int Order { get; set; }
    }
    // In _Imports.razor or component top
public static class DictionaryExtensions
{
    public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, 
        TKey key, TValue defaultValue = default)
    {
        return dict.TryGetValue(key, out var value) ? value : defaultValue;
    }
}
}