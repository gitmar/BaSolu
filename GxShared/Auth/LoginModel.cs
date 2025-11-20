using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.Auth
{
    public class LoginModel
    {
        //[Required]
        public int Orig { get; set; } = 0;
        //optinnel sauf pour coordo et creator
        public int Idorg { get; set; } = 0;
        public string Email { get; set; } = string.Empty;
        public string Phonenumber { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string UsernameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string NwPassword { get; set; } = string.Empty;
        public string Userid { get; set; } = string.Empty;
        public bool IsHelpLogin { get; set; } = false;
        public string Nom { get; set; } = string.Empty;
        public string Pnom { get; set; } = string.Empty;
        public string Raison { get; set; } = string.Empty;
        public string LogToken { get; set; } = string.Empty;
        public bool LogAuth { get; set; } = false;
        public bool RememberMe { get; set; } = false;
        public bool IsPret { get; set; } = false;
        public int LogStep { get; set; } = 0;
    }
}
