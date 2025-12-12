using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.Others
{
    public class CdlinkDto
    {
        public int Idorg { get; set; } = 0;
        public int Ordid { get; set; } = 0;
        public int Custid { get; set; } = 0;
        public string? Weburl { get; set; }
        public string? Email { get; set; }
        public string? Sendermail { get; set; }
    }
}
