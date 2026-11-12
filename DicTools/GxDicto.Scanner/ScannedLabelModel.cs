using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxDicto.Scanner
{
    public sealed class ScannerSyncRequest
    {
        public int IdOrg { get; set; }
        public string Lang { get; set; } = string.Empty;
        public List<ScannedLabel> Labels { get; set; } = [];
    }
    public sealed class ScannedLabel
    {
        public string LabelKey { get; set; } = string.Empty;
        public string ComponentPath { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public int Gvars { get; set; }
        public int Itb { get; set; }
        public int Iele { get; set; }
    }
}
