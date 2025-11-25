using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.Others
{
    public class UsrOgSetModel
    {
        public int Id { get; set; } = 0;
        public int Orig { get; set; } = 0; //Ref Origine call
        public int Idorg { get; set; } = 0;
        public int Idhie { get; set; } = 0;
        public int Idpst { get; set; } = 0;
        public string Userid { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string UsernameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Xmatri { get; set; } = string.Empty;
        public string Raison { get; set; } = string.Empty;
        public string Sigle { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string Pnom { get; set; } = string.Empty;
        public string Phonenumber { get; set; } = string.Empty;
        public string Phon2 { get; set; } = string.Empty;
        public string Whatsapp { get; set; } = string.Empty;
        public int Ipays { get; set; } = 0;
        public int Ilang { get; set; } = 0;
        public int Olang { get; set; } = 0;
        public int Imethp { get; set; } = 0;
        public int Iqmax { get; set; } = 1;
        public int Ishare { get; set; } = 0;
        public int Iwurl { get; set; } = 0;
        public string? Weburl { get; set; }
        public int Irole { get; set; } = 0;
        public string Srole { get; set; } = string.Empty; // pour verification
        public int Idoma { get; set; } = 0;
        public string Sidom { get; set; } = string.Empty; // pour verification
        public int Jdoma { get; set; } = 0;
        public string Sjdom { get; set; } = string.Empty; // pour verification
        public string Scible { get; set; } = string.Empty; // pour verification
        public int Utyp { get; set; } = 0; //correspd a tietypn1agent2operat3client
        public int Dequi { get; set; } = 0;
        public int Toqui { get; set; } = 0;
        public string? HlpEmail { get; set; } //support email
        public string? HlpWurl { get; set; }  //support siteweb
        public string? HlpPhone { get; set; } //support phone
        public string? HlpPhon2 { get; set; } //support phon2
        public string? HlpSrole { get; set; } //support Srole
        public string? HlpUname { get; set; } //support username
        public DateTime Datope { get; set; } = DateTime.Now;
        public string Uip { get; set; } = string.Empty; //inscrit Ip
        public string FbackUrl { get; set; } = string.Empty;
        public int ErrCode { get; set; } = 0;
        public string ErrMessage { get; set; } = string.Empty;
        public string Uadr1 { get; set; } = string.Empty;
        public int Ordid { get; set; } = 0;
        public int Clieid { get; set; } = 0;
        public int Agtid { get; set; } = 0;
        public string Oidstr { get; set; } = string.Empty;
        public int Ostep { get; set; } = 0;
        public int Iprovi { get; set; } = 0;
        public int Iville { get; set; } = 0;
        public int Sidown { get; set; } = 0;
        public int Simain { get; set; } = 0;
        public bool SiSupport { get; set; } = false;
        public string CodInvit { get; set; } = string.Empty;
        public int Eta { get; set; } = 0;
        public string Obsv { get; set; } = string.Empty;
    }
    public class UsrCredsModel
    {
        public int Idorg { get; set; } = 0;
        public int Ordid { get; set; } = 0;
        public string UsernameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        //generalisaition dispo apres generation order
        public string FbackUrl { get; set; } = string.Empty;
    }
}
