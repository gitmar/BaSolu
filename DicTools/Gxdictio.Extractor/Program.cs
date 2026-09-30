// GxDicto.Extractor/Program.cs
using System.Text.RegularExpressions;

if (args.Length == 0)
{
    Console.WriteLine("Usage: GxDicto.Extractor <path-to-blazor-project-or-solution>");
    return;
}

var root = Path.GetFullPath(args[0]);

if (!Directory.Exists(root))
{
    Console.WriteLine($"Directory not found: {root}");
    return;
}

var files = Directory.GetFiles(root, "*.*", SearchOption.AllDirectories)
    .Where(f => f.EndsWith(".razor", StringComparison.OrdinalIgnoreCase)
             || f.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
    .ToArray();

// Matches:
// @T("Some.Key")
// @T("Some.Key", "Default text")
// T("Some.Key")
// T("Some.Key", "Default text")
var keyPattern = new Regex(
    @"(?:@T|[^@\w]T)\(\s*""(?<key>[^""]+)""\s*(,\s*""(?<default>[^""]*)""\s*)?\)",
    RegexOptions.Compiled);

var keys = new HashSet<string>();

foreach (var file in files)
{
    // Skip generated folders
    if (file.Contains("/obj/", StringComparison.OrdinalIgnoreCase) ||
        file.Contains("\\obj\\", StringComparison.OrdinalIgnoreCase) ||
        file.Contains("/bin/", StringComparison.OrdinalIgnoreCase) ||
        file.Contains("\\bin\\", StringComparison.OrdinalIgnoreCase))
        continue;

    string content;
    try
    {
        content = await File.ReadAllTextAsync(file);
    }
    catch
    {
        continue;
    }

    var matches = keyPattern.Matches(content);
    foreach (Match m in matches)
    {
        var key = m.Groups["key"].Value;
        if (!string.IsNullOrWhiteSpace(key))
            keys.Add(key);
    }
}

var outputPath = "localization-keys.txt";
await File.WriteAllLinesAsync(outputPath, keys.Order());

Console.WriteLine($"Extracted {keys.Count} keys to {Path.GetFullPath(outputPath)}");