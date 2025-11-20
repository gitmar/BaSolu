using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.Auth
{
    public class EnrichedLoginResponse
    {
        public LoginResult Result { get; set; }
        public string FullName { get; set; }
        public string OrgName { get; set; }
        public List<string> Roles { get; set; }
    }
}
