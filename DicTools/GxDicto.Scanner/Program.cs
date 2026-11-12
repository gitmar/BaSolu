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

var apiBaseAddress = "https://localhost:7097";

const string defaultLanguage = "fr";
const int baseOrgId = -1;

var razorRoots = new[]
{
    @"C:\AACurDEV\BaSolu\GxStk",
    @"C:\AACurDEV\BaSolu\GxPilo",
    @"C:\AACurDEV\BaSolu\GxTie"
};

var scanner = new RazorLabelScannerService();

var allLabels = await scanner.ScanAsync(razorRoots);

Console.WriteLine(
    $"Total unique labels extracted: {allLabels.Count}");

var mappingFile = Path.Combine(
    Environment.CurrentDirectory,
    "LabelMapping.txt");

await scanner.ExportMappingAsync(
    mappingFile,
    allLabels);

Console.WriteLine(
    $"Mapping file written to: {mappingFile}");

await scanner.SyncWithApiAsync(
    apiBaseAddress,
    baseOrgId,
    defaultLanguage,
    allLabels);

Console.WriteLine(
    "FINISHED - French master labels synchronized into gdbse.");