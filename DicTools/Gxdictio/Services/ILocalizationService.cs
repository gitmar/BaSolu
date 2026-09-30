// GxDicto/Services/ILocalizationService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Microsoft.Extensions.Logging;


// Localization/Services/ILocalizationService.cs
namespace GxDicto.Services
{
    public interface ILocalizationService
    {
        string CurrentLanguage { get; }
        Task SetLanguageAsync(string language);
        string T(string key, string? defaultText = null);
        // Optional: expose all keys for the current language (useful for debugging / admin)
        IReadOnlyDictionary<string, string> GetAllTranslations();
    }
    public class LocalizationService : ILocalizationService
    {
        private readonly HttpClient _http;
        private readonly ILogger<LocalizationService> _logger;

        private string _currentLanguage = "en";
        private Dictionary<string, string> _translations = new();

        public LocalizationService(HttpClient http, ILogger<LocalizationService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public string CurrentLanguage => _currentLanguage;

        public async Task SetLanguageAsync(string language)
        {
            if (_currentLanguage == language && _translations.Count > 0)
                return;

            _currentLanguage = language;

            try
            {
                // Expected API: GET api/localization/{language} -> { "Key1": "Value1", "Key2": "Value2", ... }
                var response = await _http.GetFromJsonAsync<Dictionary<string, string>>(
                    $"api/localization/{language}");

                _translations = response ?? new Dictionary<string, string>();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to load translations for {Language}", language);
                _translations = new Dictionary<string, string>();
            }
        }

        public string T(string key, string? defaultText = null)
        {
            if (_translations.TryGetValue(key, out var value))
                return value;

            return defaultText ?? key;
        }

        public IReadOnlyDictionary<string, string> GetAllTranslations()
            => _translations.AsReadOnly();
    }
}

//    public interface ILocalizationService
//    {
//        string CurrentLanguage { get; }
//        Task SetLanguageAsync(string language);
//        string T(string key, string? defaultText = null);
//    }
//    public class LocalizationService : ILocalizationService
//    {
//        private readonly HttpClient _http;
//        private readonly ILogger<LocalizationService> _logger;

//        private string _currentLanguage = "en";
//        private Dictionary<string, string> _translations = new();

//        public LocalizationService(HttpClient http, ILogger<LocalizationService> logger)
//        {
//            _http = http;
//            _logger = logger;
//        }

//        public string CurrentLanguage => _currentLanguage;

//        public async Task SetLanguageAsync(string language)
//        {
//            if (_currentLanguage == language && _translations.Count > 0)
//                return;

//            _currentLanguage = language;

//            try
//            {
//                var response = await _http.GetFromJsonAsync<Dictionary<string, string>>(
//                    $"api/localization/{language}");

//                _translations = response ?? new Dictionary<string, string>();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogWarning(ex, "Failed to load translations for {Language}", language);
//                _translations = new Dictionary<string, string>();
//            }
//        }

//        public string T(string key, string? defaultText = null)
//        {
//            if (_translations.TryGetValue(key, out var value))
//                return value;

//            return defaultText ?? key;
//        }
//    }
//}