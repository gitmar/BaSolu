using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.GlobModels
{
    public class UsrMail
    {
        public int Id { get; set; } = 0;
        public int Typg { get; set; } = 0;
        public string From { get; set; } = string.Empty;
        public string E2mail { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public string Tophone { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int Uorgv { get; set; } = 0;
        public string Ucode { get; set; } = string.Empty;
        public string Uinfos { get; set; } = string.Empty;
        public string BlzAddress { get; set; } = string.Empty;
    }
}
