using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.GlobModels.Auth
{
    public class LoginResponse
    {
        // Auth/session
        public string Atoken { get; set; } = string.Empty;
        public string Rtoken { get; set; } = string.Empty;
        public DateTime Adexp { get; set; }
        public string Userid { get; set; } = string.Empty;
        public int Idorg { get; set; } = 0;
        // Identity metadata
        public string FullName { get; set; } = string.Empty;
        public string OrgName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        // Optional flags
        public bool IsAuthenticated { get; set; } = false;
        public bool IsOwner { get; set; } = false;
        //for Admin login
        public ICollection<AdmOrga> Admorgas { get; set; } = new List<AdmOrga>();
    }
    public class AdmOrga
    {
        public int Idorg { get; set; } = 0;
        public string Raison { get; set; } = string.Empty;
        public string Sigle { get; set; } = string.Empty;
    }
}
