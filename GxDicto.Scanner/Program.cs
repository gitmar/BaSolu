// GxDicto.Scanner/Program.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using GxDicto.Scanner;

var apiBaseAddress = "https://localhost:7095"; // API port
using var http = new HttpClient { BaseAddress = new Uri(apiBaseAddress) };

var scanner = new RazorLabelScannerService(http);

var defaultLanguage = "fr";   // base org dictionary language
var baseOrgId = -1;

// --- Scan all roots globally ---
var razorRoots = new[]
{
    @"C:\AACurDEV\BaSolu\GxStk",
    @"C:\AACurDEV\BaSolu\GxPilo",
    @"C:\AACurDEV\BaSolu\GxTie"
};

var allKeys = await scanner.ScanAsync(razorRoots);

// Sync with API (always 'fr' for base org)
await scanner.SyncKeysWithApiAsync(apiBaseAddress, defaultLanguage, baseOrgId, allKeys);

// Export mapping file for review
var allMappingFile = Path.Combine(Environment.CurrentDirectory, "LabelMapping.txt");
await scanner.ExportMappingAsync(allMappingFile, allKeys);

Console.WriteLine($"All-roots mapping file written to {allMappingFile}");
Console.WriteLine($"Total unique keys extracted from all roots: {allKeys.Count}");


//using GxDicto.Scanner;


//var apiBaseAddress = "https://localhost:7095"; // API port
//using var http = new HttpClient { BaseAddress = new Uri(apiBaseAddress) };

//var scanner = new RazorLabelScannerService(http);

//var defaultLanguage = "fr";
//var baseOrgId = -1;

//// --- Scan a single component ---
//var componentFile = @"C:\AACurDEV\BaSolu\GxPilo\Layout\NavMenu.razor";
//var componentKeys = await scanner.ScanComponentAsync(componentFile);
//await scanner.SyncKeysWithApiAsync(apiBaseAddress, defaultLanguage, baseOrgId, componentKeys);

//var componentMappingFile = Path.Combine(Environment.CurrentDirectory, "NavMenuMapping.txt");
//await scanner.ExportMappingAsync(componentMappingFile, componentKeys);

//Console.WriteLine($"Component mapping file written to {componentMappingFile}");
//Console.WriteLine($"Total unique keys extracted from component: {componentKeys.Count}");

//// --- Scan all roots ---
//var razorRoots = new[]
//{
//    @"C:\AACurDEV\BaSolu\GxStk",
//    @"C:\AACurDEV\BaSolu\GxPilo",
//    @"C:\AACurDEV\BaSolu\GxTie"
//};

//var allKeys = await scanner.ScanAsync(razorRoots);
//await scanner.SyncKeysWithApiAsync(apiBaseAddress, defaultLanguage, baseOrgId, allKeys);

//var allMappingFile = Path.Combine(Environment.CurrentDirectory, "LabelMapping.txt");
//await scanner.ExportMappingAsync(allMappingFile, allKeys);

//Console.WriteLine($"All-roots mapping file written to {allMappingFile}");
//Console.WriteLine($"Total unique keys extracted from all roots: {allKeys.Count}");
