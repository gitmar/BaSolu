using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace GxShared.Others
{
    public class Ugnjwt
    {
        public bool JwOk { get; set; } = false;
        public string JwToken { get; set; } = string.Empty;
        public JsonWebKey[] JwKeys { get; set; }
        public string Curkid { get; set; } = string.Empty;
    }
}
