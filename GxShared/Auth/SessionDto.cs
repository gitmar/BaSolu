using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GxShared.Others;

namespace GxShared.Auth
{
    public class SessionDto
    {
        public string Userid { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string OrgName { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();
        public List<Grole> FullRoles { get; set; } = new();
    };
}
