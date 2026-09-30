using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// GxDicto/DModels/LocalizationEntry.cs
namespace GxDicto.DModels
{
    public class LocalizationEntry
    {
        public int Id { get; set; }
        public int Idorg { get; set; }
        public string Key { get; set; } = "";
        public string Language { get; set; } = "";
        public string Value { get; set; } = "";
        // Optional: default/base language value
        public string? BaseValue { get; set; }
    }
}
