using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.GlobModels
{
    public class UsrOgSetModel
    {
        public int Id { get; set; }
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
        public int Ishare { get; set; } = 0;
        public int Ipays { get; set; } = 0;
        public int Ilang { get; set; } = 0;
        public int Olang { get; set; } = 0;
        public int Imethp { get; set; } = 0;
        public int Idoma { get; set; } = 0;
        public string Sdoma { get; set; } = string.Empty;
        public string Scible { get; set; } = string.Empty;
        public int Simain { get; set; } = 0;
        public int Gdom { get; set; } = 0;
        public int Iqmax { get; set; } = 1;
        public int Iwurl { get; set; } = 0;
        public string? Weburl { get; set; }
        public int Irole { get; set; } = 0;
        public string Srole { get; set; } = string.Empty; // pour verification
        public int Utyp { get; set; } = 0; //correspd a tietypn1agent2operat3client
        public int Dequi { get; set; } = 0;
        public int Toqui { get; set; } = 0;
        public string VndEmail { get; set; } = string.Empty; //inviter mail
        public string VndPhone { get; set; } = string.Empty; //inviter phone
        public string VndNom { get; set; } = string.Empty; //inviter username
        public string VndPnom { get; set; } = string.Empty; //inviter username
        public DateTime Datope { get; set; } = DateTime.Now;
        public string Uip { get; set; } = string.Empty; //inscrit Ip
        public string FbackUrl { get; set; } = string.Empty;
        public int Eta { get; set; } = 0;
        public string Uadr1 { get; set; } = string.Empty;
        public string Infoplus { get; set; } = string.Empty;  // id invite si invite non integre
        public int Ordid { get; set; } = 0; //replique Id
        public string Oidstr { get; set; } = string.Empty;
        public int Cdeid { get; set; } = 0; //replique Id
        public string Sordid { get; set; } = string.Empty;
        public int Isite { get; set; } = 0;
        public int Iprovi { get; set; } = 0;
        public int Iville { get; set; } = 0;
        //unOrga.Obsv = inObj.ocomnts;
        //pour correction apres enregistrement
        public int Orig { get; set; } = 0;
        public int Siset { get; set; } = 0; //si orga installe/lie
        public int Sidown { get; set; } = 0;
        public bool SiSupport { get; set; } = false;
        public string CodInvit { get; set; } = string.Empty;
        public string Obsv { get; set; } = string.Empty;
    }
}
