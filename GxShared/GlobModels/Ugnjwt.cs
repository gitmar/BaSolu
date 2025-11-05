using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.GlobModels
{
    public class Ugnjwt
    {
        public bool JwOk { get; set; } = false;
        public string JwToken { get; set; } = string.Empty;
        public JsonWebKey[] JwKeys { get; set; }
        public string Curkid { get; set; } = string.Empty;
    }
}
