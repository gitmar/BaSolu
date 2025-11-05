using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;

namespace GxShared.GlobModels
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
        public bool Isauthen { get; set; } = false;
        public bool Vuok { get; set; } = false;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phonenumber { get; set; } = string.Empty;
        public string Nompnom { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public bool Yaorga { get; set; } = false;
        public bool SiOwner { get; set; } = false;
        public int Uorgid { get; set; } = 0;
        public bool Siset { get; set; } = false;
        public bool SiCoordo { get; set; } = false;
        public List<string>? Roles { get; set; } = new List<string>();
        //public ICollection<Gtoken> Tokens { get; set;} = new List<Gtoken>();
        public string Message {  get; set; } = string.Empty;
        public ICollection<Glxorg> Usrdoms { get; set; } = new List<Glxorg>();
        public ICollection<string>? ErrNumb { get; set; } = new List<string>();
        public ICollection<string>? ErrList { get; set; } = new List<string>();
    }
    public class Glxorg
    {
        public int Idorg { get; set; } = 0;
        public string Raison { get; set; } = string.Empty;
        public string Stsigle { get; set; } = string.Empty;
        public string Stdom { get; set; } = string.Empty;
        public int Dom { get; set; } = 0;
        public string Liba { get; set; } = string.Empty;
        public string Abg { get; set; } = string.Empty;
    }
}
