using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GxShared.Sess;


namespace GxShared.Auth
{
    public class LoginResult
    {
        public string Userid { get; set; } = string.Empty;
        public string Rtoken { get; set; } = string.Empty;
        public string Atoken { get; set; } = string.Empty;
        public string Etoken { get; set; } = string.Empty;
        public DateTime? Adexp { get; set; }
        public DateTime? Rdexp { get; set; }
        public DateTime? Edexp { get; set; }
        public string Curkid { get; set; } = string.Empty;
        //public JsonWebKey[] Ukeys { get; set; } = new JsonWebKey[] { };
        public bool Succeeded { get; set; } = false;
        public bool Completed { get; set; } = false;
        public bool IsAuthenticated { get; set; } = false;
        public bool Vuok { get; set; } = false;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phonenumber { get; set; } = string.Empty;
        public string Nompnom { get; set; } = string.Empty;
        public string OgRaison { get; set; } = string.Empty;
        public string OgSigle { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public bool Yaorga { get; set; } = false;
        public bool SiOwner { get; set; } = false;
        public bool SiAdmin { get; set; } = false;
        public bool SiSupport { get; set; } = false;
        public int LgOrgid { get; set; } = 0;
        public int Siorga { get; set; } = 0;
        //public bool SiCoordo { get; set; } = false;
        public List<string>? Roles { get; set; } = new List<string>();
        //public ICollection<Gtoken> Tokens { get; set;} = new List<Gtoken>();
        public string Message { get; set; } = string.Empty;
        public ICollection<AdmOrga> AllOrgas { get; set; } = new List<AdmOrga>();
        //public ICollection<Glxorg> Usrdoms { get; set; } = new List<Glxorg>();
        public ICollection<string>? ErrNumb { get; set; } = new List<string>();
        public ICollection<string>? ErrList { get; set; } = new List<string>();
    }
}