using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using GxDicto.DModels;

namespace GxDicto.Scanner
{
    public class RazorLabelScannerService
    {
        private readonly HttpClient _http;

        public RazorLabelScannerService(HttpClient http)
        {
            _http = http;
        }

        public async Task<HashSet<string>> ScanAsync(string[] roots)
        {
            var allKeys = new HashSet<string>(StringComparer.Ordinal);

            foreach (var root in roots)
            {
                if (!Directory.Exists(root)) continue;

                var razorFiles = Directory.GetFiles(root, "*.razor", SearchOption.AllDirectories);

                foreach (var file in razorFiles)
                {
                    var content = File.ReadAllText(file);
                    ProcessFile(content, allKeys);
                }
            }

            return allKeys;
        }

        public async Task<HashSet<string>> ScanComponentAsync(string componentFile)
        {
            var keys = new HashSet<string>(StringComparer.Ordinal);

            if (File.Exists(componentFile))
            {
                var content = File.ReadAllText(componentFile);
                ProcessFile(content, keys);
            }

            return keys;
        }
        private void ProcessFile(string content, HashSet<string> keys)
        {
            // Clean Razor markup
            content = Regex.Replace(content, @"@code\s*\{[^}]*\}", "", RegexOptions.Singleline);
            content = Regex.Replace(content, @"@\*.*?\*@", "", RegexOptions.Singleline);

            // Remove entire <svg> blocks
            content = Regex.Replace(content, @"<svg.*?</svg>", "",
                RegexOptions.Singleline | RegexOptions.IgnoreCase);

            var tagRegex = new Regex(@"<(?<tag>\w+)[^>]*>(?<text>[^<]+)</\k<tag>>");

            foreach (Match match in tagRegex.Matches(content))
            {
                var tag = match.Groups["tag"].Value;
                var text = match.Groups["text"].Value.Trim();

                // Skip <i> tags (Bootstrap icons)
                if (tag.Equals("i", StringComparison.OrdinalIgnoreCase))
                    continue;

                // Remove emojis and symbols, preserving letters, digits and spaces
                text = Regex.Replace(text, @"[^\p{L}\p{N}\s]", "").Trim();

                if (IsValidLabel(text))
                    keys.Add(text);
            }
        }
        //private void ProcessFile(string content, HashSet<string> keys)
        //{
        //    // Clean Razor markup
        //    content = Regex.Replace(content, "@code\\s*\\{[^}]*\\}", "", RegexOptions.Singleline);
        //    content = Regex.Replace(content, "@\\*.*?\\*@", "", RegexOptions.Singleline);

        //    var lines = content.Split('\n');
        //    foreach (var line in lines)
        //    {
        //        var matches = Regex.Matches(line, @">([^<>]+)<");
        //        foreach (Match m in matches)
        //        {
        //            var text = m.Groups[1].Value.Trim();
        //            if (IsValidLabel(text))
        //                keys.Add(text);
        //        }
        //    }
        //}

        private bool IsValidLabel(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            if (text.StartsWith("@") || text.Contains("@")) return false;
            if (text.Length < 3 || text.Length > 120) return false;
            if (text.Count(char.IsLetterOrDigit) < 3) return false;
            if (text.StartsWith("&") && text.Contains(";")) return false;
            return true;
        }

        private string GenerateKey(string text)
        {
            var cleaned = new string(text.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());
            var words = cleaned.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return string.Concat(words.Select(w =>
                char.ToUpperInvariant(w[0]) + w.Substring(1).ToLowerInvariant()));
        }

        public async Task SyncKeysWithApiAsync(string apiBase, string language, int orgId, HashSet<string> keys)
        {
            var existingDict = await _http.GetFromJsonAsync<Dictionary<string, string>>(
                $"api/localization/{orgId}/{language}");


            var existingKeys = existingDict?.Keys.ToHashSet(StringComparer.Ordinal)
                                ?? new HashSet<string>(StringComparer.Ordinal);

            var missingKeys = keys.Where(k => !existingKeys.Contains(GenerateKey(k))).ToList();

            foreach (var rawText in missingKeys)
            {
                var key = GenerateKey(rawText);

                var entry = new LocalizationEntry
                {
                    Idorg = orgId,
                    Key = key,
                    Language = language,
                    Value = rawText,
                    BaseValue = rawText
                };

                await _http.PostAsJsonAsync($"/api/localization/{orgId}/admin/entries", entry);
            }
        }

        public async Task ExportMappingAsync(string outputPath, IEnumerable<string> rawTexts)
        {
            var lines = rawTexts.Select(raw => $"{raw} => {GenerateKey(raw)}");
            await File.WriteAllLinesAsync(outputPath, lines);
        }
    }
}