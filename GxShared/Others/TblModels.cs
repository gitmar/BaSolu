using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.Others
{
    public class TableSet
    {
        public List<TableItem> Langues { get; set; } = new();
        public List<TableItem> Enviros { get; set; } = new();
        public List<TableItem> Sites { get; set; } = new();
        public List<TableItem> Countries { get; set; } = new();
        public List<TableItem> Regions { get; set; } = new();
        public List<TableItem> Domas { get; set; } = new();
        public List<TableItem> Gdoms { get; set; } = new();
        public List<TableItem> Paiements { get; set; } = new();
        public List<TableItem> Etapes { get; set; } = new();
        public List<TableItem> Statuts { get; set; } = new();
        public List<TableItem> Operats { get; set; } = new();
        public List<TableItem> Ouinons { get; set; } = new();
        public List<TableItem> Roles { get; set; } = new();
        public List<TableItem> Etaclies { get; set; } = new();
        public List<TableItem> Etaordas { get; set; } = new();
    }

    public class TableItem
    {
        public int Elea { get; set; } = 0;
        public string Liba { get; set; } = string.Empty;
        public string Sliba { get; set; } = string.Empty;
        public string Abg { get; set; } = string.Empty;
    }
}
