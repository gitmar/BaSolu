// GxDicto.Scanner/Program.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using GxDicto.DModels;

// Configure your API base address
var apiBaseAddress = "https://localhost:7095"; // adjust if different
var defaultLanguage = "en";
var baseOrgId = -1;

// Roots to scan (for App)
var razorRoots = new[]
{
    @"C:\AACurDEV\BaSolu\GxStk",
    @"C:\AACurDEV\BaSolu\GxPilo",
    @"C:\AACurDEV\BaSolu\GxTie"
};
Console.WriteLine($"Scanning Razor files and syncing keys with API at {apiBaseAddress}");
Console.WriteLine($"Default language: {defaultLanguage}, Base organization: {baseOrgId}");
Console.WriteLine();

var allKeys = new HashSet<string>(StringComparer.Ordinal);

foreach (var razorRoot in razorRoots)
{
    if (!Directory.Exists(razorRoot))
    {
        Console.WriteLine($"Root not found: {razorRoot}");
        continue;
    }

    var razorFiles = Directory.GetFiles(razorRoot, "*.razor", SearchOption.AllDirectories);
    Console.WriteLine($"Found {razorFiles.Length} .razor files under {razorRoot}");

    foreach (var file in razorFiles)
    {
        var content = File.ReadAllText(file);

        // Remove @code { ... } blocks
        content = Regex.Replace(content, "@code\\s*\\{[^}]*\\}", "", RegexOptions.Singleline);

        // Remove @functions, @namespace, @using, @inject, @implements, @attribute lines
        content = Regex.Replace(content, "^\\s*@[a-zA-Z]+[^\r\n]*", "", RegexOptions.Multiline);

        // Remove Razor comments @* ... *@
        content = Regex.Replace(content, "@\\*.*?\\*@", "", RegexOptions.Singleline);

        var lines = content.Split('\n');

        foreach (var line in lines)
        {
            // Skip lines that are clearly code
            if (line.Contains("private ") ||
                line.Contains("public ") ||
                line.Contains("internal ") ||
                line.Contains("protected ") ||
                line.Contains("var ") ||
                line.Contains("func", StringComparison.OrdinalIgnoreCase) ||
                line.Contains("Func<") ||
                line.Contains("EventCallback") ||
                line.Contains("Dictionary<") ||
                line.Contains("List<") ||
                line.Contains("IEnumerable<") ||
                line.Contains("Enumerable.Empty") ||
                line.Trim().StartsWith("//") ||
                line.Trim().StartsWith("/*"))
            {
                continue;
            }

            // Match >text< patterns
            var matches = Regex.Matches(line, @">([^<>]+)<");

            foreach (Match m in matches)
            {
                var text = m.Groups[1].Value.Trim();

                if (string.IsNullOrWhiteSpace(text))
                    continue;

                // Skip obvious code / markup
                if (text.StartsWith("@"))
                    continue;
                if (text.Contains("@"))
                    continue;
                if (text.StartsWith("{") || text.StartsWith("}"))
                    continue;
                if (text.Contains("=") && (text.Contains("\"") || text.Contains("'")))
                    continue;
                if (text.Contains("=>"))
                    continue;
                if (text.Contains("new(") || text.Contains("new ("))
                    continue;
                if (text.Contains("List<") || text.Contains("Dictionary<") || text.Contains("Func<"))
                    continue;
                if (text.Contains("EventCallback") || text.Contains("Parameter"))
                    continue;

                // Skip very short noise
                if (text.Length == 1 && !char.IsLetterOrDigit(text[0]))
                    continue;

                // Skip very long lines
                if (text.Length > 120)
                    continue;

                // Skip lines that are mostly punctuation
                var letterCount = text.Count(char.IsLetterOrDigit);
                if (letterCount < 3)
                    continue;

                // Skip keys starting with ? or (
                if (text.StartsWith("?") || text.StartsWith("("))
                    continue;

                // Skip HTML entities like &nbsp;
                if (text.StartsWith("&") && text.Contains(";"))
                    continue;

                allKeys.Add(text);
            }
        }
    }
}

Console.WriteLine();
Console.WriteLine($"Total unique keys extracted: {allKeys.Count}");
Console.WriteLine();

// Sync with API (non-static local function so it can capture baseOrgId)
await SyncKeysWithApiAsync(apiBaseAddress, defaultLanguage, baseOrgId, allKeys);

async Task SyncKeysWithApiAsync(string apiBase, string language, int orgId, HashSet<string> keys)
{
    using var http = new HttpClient { BaseAddress = new Uri(apiBase) };

    // Get existing keys for this org + language
    Console.WriteLine($"Fetching existing keys for organization {orgId}, language '{language}'...");
    var existingDict = await http.GetFromJsonAsync<Dictionary<string, string>>(
        $"/api/localization/{orgId}/{language}");

    var existingKeys = existingDict?.Keys.ToHashSet(StringComparer.Ordinal)
                        ?? new HashSet<string>(StringComparer.Ordinal);

    Console.WriteLine($"Existing keys for org {orgId}, language '{language}': {existingKeys.Count}");
    Console.WriteLine();

    var missingKeys = keys
        .Where(k => !existingKeys.Contains(k))
        .OrderBy(k => k)
        .ToList();

    if (!missingKeys.Any())
    {
        Console.WriteLine("No missing keys. All extracted keys already exist in the database for this org/language.");
        return;
    }

    Console.WriteLine($"Missing keys to insert: {missingKeys.Count}");
    Console.WriteLine();

    int inserted = 0;
    int failed = 0;

    foreach (var key in missingKeys)
    {
        var entry = new LocalizationEntry
        {
            Idorg = orgId,
            Key = key,
            Language = language,
            Value = key,        // default value = key
            BaseValue = key     // optional
        };

        try
        {
            var response = await http.PostAsJsonAsync($"/api/localization/{orgId}/admin/entries", entry);
            if (response.IsSuccessStatusCode)
            {
                inserted++;
                Console.WriteLine($"[OK]  {key}");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                // Key already exists – treat as success
                inserted++;
                Console.WriteLine($"[SKIP] {key} (already exists)");
            }
            else
            {
                failed++;
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ERR] {key} – {response.StatusCode} – {error}");
            }
        }
        catch (Exception ex)
        {
            failed++;
            Console.WriteLine($"[ERR] {key} – {ex.Message}");
        }
    }

    Console.WriteLine();
    Console.WriteLine($"Inserted: {inserted}, Failed: {failed}");
}