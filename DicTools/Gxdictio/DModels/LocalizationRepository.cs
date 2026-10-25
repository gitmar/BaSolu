using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxDicto.DModels
{
    public class LocalizationRepository
    {
        // In-memory store: orgId -> language -> key -> value
        private readonly Dictionary<int, Dictionary<string, Dictionary<string, string>>> _store = new();

        public Dictionary<string, string> GetTranslations(int orgId, string language)
        {
            if (_store.TryGetValue(orgId, out var langs) &&
                langs.TryGetValue(language, out var dict))
            {
                return dict;
            }
            return new Dictionary<string, string>();
        }

        public bool Exists(int orgId, string language, string key)
        {
            return _store.TryGetValue(orgId, out var langs) &&
                   langs.TryGetValue(language, out var dict) &&
                   dict.ContainsKey(key);
        }

        public void AddEntry(int orgId, LocalizationEntry entry)
        {
            if (!_store.ContainsKey(orgId))
                _store[orgId] = new Dictionary<string, Dictionary<string, string>>();

            if (!_store[orgId].ContainsKey(entry.Language))
                _store[orgId][entry.Language] = new Dictionary<string, string>();

            _store[orgId][entry.Language][entry.Key] = entry.Value;
        }

        //// Add new language to org using stored procedure
        //public async Task<int> AddLanguageToOrgAsync(int idorg, string language)
        //{
        //    var result = await _db.Database.ExecuteSqlInterpolatedAsync(
        //        $"EXEC dbo.sp_Localization_AddLanguageToOrg @Idorg = {idorg}, @NewLanguage = {language}");
        //    return result;
        //}
    }
}

