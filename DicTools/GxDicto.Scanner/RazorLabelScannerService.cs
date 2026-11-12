using System.Globalization;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.EntityFrameworkCore;

namespace GxDicto.Scanner
{
       public class RazorLabelScannerService
    {
        public Task<List<ScannedLabel>> ScanAsync(string[] roots)
        {
            var labels = new Dictionary<string, ScannedLabel>(
                StringComparer.OrdinalIgnoreCase);

            foreach (var root in roots)
            {
                if (!Directory.Exists(root))
                {
                    continue;
                }

                var appName = Path.GetFileName(
                    root.TrimEnd(Path.DirectorySeparatorChar));

                var razorFiles = Directory.GetFiles(
                    root,
                    "*.razor",
                    SearchOption.AllDirectories);

                foreach (var file in razorFiles)
                {
                    var relativePath = Path
                        .GetRelativePath(root, file)
                        .Replace("\\", "/");

                    var content = File.ReadAllText(file);

                    var fileLabels = ExtractLabels(
                        content,
                        appName,
                        relativePath);

                    foreach (var label in fileLabels)
                    {
                        if (!labels.TryGetValue(
                                label.LabelKey,
                                out var existing))
                        {
                            labels[label.LabelKey] = label;
                        }
                        else
                        {
                            // Keep the richest label text
                            if (label.Label.Length >
                                existing.Label.Length)
                            {
                                labels[label.LabelKey] = label;
                            }
                        }
                    }
                }
            }

            return Task.FromResult(
                labels.Values
                    .OrderBy(x => x.LabelKey)
                    .ToList());
        }

        private List<ScannedLabel> ExtractLabels(
    string content,
    string appName,
    string relativePath)
        {
            var labels = new List<ScannedLabel>();

            content = Regex.Replace(
                content,
                @"@code\s*\{.*?\}",
                string.Empty,
                RegexOptions.Singleline);

            content = Regex.Replace(
                content,
                @"@\*.*?\*@",
                string.Empty,
                RegexOptions.Singleline);

            content = Regex.Replace(
                content,
                @"<svg.*?</svg>",
                string.Empty,
                RegexOptions.Singleline |
                RegexOptions.IgnoreCase);

            var tagRegex = new Regex(
                @"<(?<tag>label|button|h1|h2|h3|h4|h5|h6|span|p|a|title|div)[^>]*>(?<text>[^<]+)</\k<tag>>",
                RegexOptions.IgnoreCase);

            int labelIndex = 0;

            foreach (Match match in tagRegex.Matches(content))
            {
                var rawText =
                    match.Groups["text"]
                        .Value
                        .Trim();
                // Remove leading icons/emojis/symbols
                rawText = Regex.Replace(
                    rawText,
                    @"^[^\p{L}]+",
                    string.Empty);
                if (IsIconOnly(rawText))
                {
                    continue;
                }
                if (!IsValidLabel(rawText))
                {
                    continue;
                }
                var cleanText = Regex.Replace(
                    rawText,
                    @"\s+",
                    " ")
                    .Trim();

                labelIndex++;

                labels.Add(
                    new ScannedLabel
                    {
                        LabelKey = NormalizeLabel(cleanText),
                        ComponentPath = relativePath,
                        Gvars = 1,
                        Itb = 1,
                        Iele = labelIndex,
                        Label = cleanText
                    });
            }

            return labels;
        }
        private static string NormalizeLabel(
    string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            text = text.Normalize(
                NormalizationForm.FormD);

            var sb = new StringBuilder();

            foreach (var c in text)
            {
                if (CharUnicodeInfo
                    .GetUnicodeCategory(c)
                    != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            text = sb.ToString();

            text = Regex.Replace(
                text,
                @"[^A-Za-z0-9\s]",
                " ");

            text = Regex.Replace(
                text,
                @"\s+",
                " ")
                .Trim();

            var words =
                text.Split(
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries);

            return string.Concat(
                words.Select(
                    w => char.ToUpperInvariant(w[0])
                         + w.Substring(1)));
        }
        private static bool IsValidLabel(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }
            if (Regex.IsMatch(
                text, @"[.=><\?\(\)\[\]]"))
            {
                return false;
            }
            // Reject Razor expressions and C# code.
            if (text.Contains("@") ||
                text.Contains("{") ||
                text.Contains("}"))
            {
                return false;
            }

            // Reject HTML entities.
            if (text.Contains("&") && text.Contains(";"))
            {
                return false;
            }

            // Reject obvious C# identifiers.
            if (Regex.IsMatch(text, @"^[A-Za-z_][A-Za-z0-9_]*$"))
            {
                return false;
            }

            // Reject method calls.
            if (Regex.IsMatch(text, @"\b[A-Za-z_][A-Za-z0-9_]*\("))
            {
                return false;
            }

            // Require at least two letters or digits.
            if (text.Count(char.IsLetterOrDigit) < 2)
            {
                return false;
            }

            // Reject very long text; those are usually paragraphs or generated content.
            if (text.Length > 120)
            {
                return false;
            }

            return true;
        }
        private static bool IsIconOnly(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }

            // Remove spaces
            text = text.Trim();

            // Accept only labels containing at least one letter
            return !text.Any(char.IsLetter);
        }

        public async Task ExportMappingAsync(
    string outputPath,
    List<ScannedLabel> labels)
        {
            var builder =
                new StringBuilder();

            builder.AppendLine(
                "Iele|LabelKey|Label|ComponentPath");

            foreach (var label in labels
                .OrderBy(x => x.LabelKey))
            {
                builder.AppendLine(
                    $"{label.Iele}|{label.LabelKey}|{label.Label}|{label.ComponentPath}");
            }

            await File.WriteAllTextAsync(
                outputPath,
                builder.ToString());
        }
        public async Task SyncWithApiAsync(
    string apiBaseAddress,
    int organizationId,
    string language,
    List<ScannedLabel> labels)
        {
            using var http =
                new HttpClient
                {
                    BaseAddress =
                        new Uri(apiBaseAddress)
                };

            var request =
                new ScannerSyncRequest
                {
                    IdOrg = organizationId,
                    Lang = language,
                    Labels = labels
                };

            var response =
                await http.PostAsJsonAsync(
                    "api/DicaLocalization/scanner/sync",
                    request);
            Console.WriteLine(
  $"POST -> {http.BaseAddress}api/dicalocalization/scanner/sync");

            Console.WriteLine(
                $"StatusCode = {(int)response.StatusCode}");

            var axdy = await response.Content.ReadAsStringAsync();

            Console.WriteLine(axdy);
            //response.EnsureSuccessStatusCode();
        }
    }
}