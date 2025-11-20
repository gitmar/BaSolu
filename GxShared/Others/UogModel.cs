using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace GxShared.Others
{
    public class UsrModel
    {
        public int Id { get; set; } = 0;
        public int UorgId { get; set; } = 0;
        public string? UserId { get; set; } = string.Empty;
        public bool? SiSet { get; set; } = false;
        public bool Succeeded { get; set; } = false;
        public string? Atoken { get; set; } = string.Empty;
        public string? Rtoken { get; set; } = string.Empty;
        public int UhieId { get; set; } = 0;
        public bool YaOrga { get; set; } = false;
        public bool YaHier { get; set; } = false;
        public string Unom { get; set; } = string.Empty;
        public string Upnom { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Uphone { get; set; } = string.Empty;
        //public Dictionary<string, string>? Claims { get; set; } = new Dictionary<string, string>();
        public int Sicor { get; set; } = 0;
        public int Nbcor { get; set; } = 0;
        public DateTime? Datc { get; set; } = DateTime.Now;
        //public JsonObject UsJson { get; set; } = new JsonObject();
        public int ErrTyp { get; set; } = 0;
        public string? ErrMsg { get; set; } = string.Empty;
        public List<string> ErrList { get; set; } = new List<string>();
        //roles
        public ICollection<UsrRolModel>? UsrRols { get; set; } = new List<UsrRolModel>();
        //autorisations directes
        public ICollection<RolAuthModel> UsrAuts = new List<RolAuthModel>();
        //public string Cdinvit { get; set; } = string.Empty;
        public int Eta { get; set; } = 0;
    }
    
    public class UsrRolModel
    {
        public int Id { get; set; } = 0;
        public int Rlevel { get; set; } = 0;
        public string Rolid { get; set; } = string.Empty;
        public string Rolnom { get; set; } = string.Empty;
        public ICollection<RolAuthModel> URauts = new List<RolAuthModel>();
        public int Eta { get; set; } = 0;
    }
    public class RolModel
    {
        public int Id { get; set; } = 0;
        public int Rlevel { get; set; } = 0;
        public string Rolid { get; set; } = string.Empty;
        public string Rolnom { get; set; } = string.Empty;
        public int Eta { get; set; } = 0;
    }
    public class RolAuthModel
    {
        public int Id { get; set; } = 0;
        public int Gtyp { get; set; } = 0;
        public int Aseq { get; set; } = 0;
        public string Rolid { get; set; } = string.Empty;
        public string Usrid { get; set; } = string.Empty;
        public int Avar { get; set; } = 0;
        public int Alea { get; set; } = 0;
        public string Liba { get; set; } = string.Empty;
        public string Abg { get; set; } = string.Empty;
        public int Eta { get; set; } = 0;
    }
    //public class RolUsrModelxx
    //{
    //    public bool Succeeded { get; set; } = false;
    //    public string Message { get; set; } = string.Empty;
    //    public ICollection<UsrModel>? UetRoles = new List<UsrModel>();
    //    public ICollection<RolModel>? AllRoles = new List<RolModel>();
    //}
}