// GxDicto.Rewriter/Program.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using GxDicto.DModels;

var apiBaseAddress = "https://localhost:7095";
var baseOrgId = -1;

var sourceRoot = @"C:\AACurDEV\BaSolu\GxTier";
var backupRoot = @"C:\AACurDEV\GxBackup\GxTier";

Console.WriteLine($"Loading keys from base org {baseOrgId} at {apiBaseAddress}");
using var http = new HttpClient { BaseAddress = new Uri(apiBaseAddress) };

var allEntries = await http.GetFromJsonAsync<List<LocalizationEntry>>(
        $"/api/localization/{baseOrgId}/admin/entries")
    ?? new List<LocalizationEntry>();

// Use the Key as the dictionary key; default text will be the same as Key
var validKeys = allEntries
    .Select(e => e.Key)
    .ToHashSet(StringComparer.Ordinal);

Console.WriteLine($"Loaded {validKeys.Count} unique keys from base dictionary.");

var razorFiles = Directory.GetFiles(sourceRoot, "*.razor", SearchOption.AllDirectories);
Console.WriteLine($"Found {razorFiles.Length} .razor files under {sourceRoot}");

int filesChanged = 0;
int replacements = 0;

foreach (var sourceFile in razorFiles)
{
    // Compute relative path and backup path
    var relativePath = Path.GetRelativePath(sourceRoot, sourceFile);
    var backupFile = Path.Combine(backupRoot, relativePath);

    var backupDir = Path.GetDirectoryName(backupFile)!;
    if (!Directory.Exists(backupDir))
        Directory.CreateDirectory(backupDir);

    var content = File.ReadAllText(sourceFile);

    // Apply replacements on the full content
    var newContent = Regex.Replace(content, @">([^<>]+)<", m =>
    {
        var text = m.Groups[1].Value.Trim();

        // Skip if it contains Razor syntax
        if (text.Contains("@"))
            return m.Value;

        // Skip if not a known key
        if (!validKeys.Contains(text))
            return m.Value;

        // Replace with @T("key", "default text")
        return $">@T(\"{text}\", \"{text.Replace("\"", "\\\"")}\")<";
    });

    // Always write the backup file (even if unchanged, for consistency)
    File.WriteAllText(backupFile, newContent);

    if (newContent != content)
    {
        filesChanged++;
        Console.WriteLine($"Modified: {relativePath}");
    }
}

Console.WriteLine();
Console.WriteLine($"Source root:   {sourceRoot}");
Console.WriteLine($"Backup root:   {backupRoot}");
Console.WriteLine($"Files processed: {razorFiles.Length}");
Console.WriteLine($"Files changed: {filesChanged}");
Console.WriteLine($"Total replacements: {replacements}");
Console.WriteLine();
Console.WriteLine("Next steps:");
Console.WriteLine("1. Review changes in C:\\AACurDEV\\GxBackup\\GxTier.");
Console.WriteLine("2. If satisfied, copy modified files back to GxTier (or merge via Git).");
Console.WriteLine("3. Build and test GxTier.");