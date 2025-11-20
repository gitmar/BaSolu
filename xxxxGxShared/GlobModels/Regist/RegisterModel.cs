using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.GlobModels
{
    public class RegisterModel
    {
        public int Id { get; set; } = 0;
        //verified by Code [Required(ErrorMessage = "You must enter a valid email address")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "You must enter a valid phone number.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phonenumber")]
        public string Phonenumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        [Required(ErrorMessage = "You must enter a valid Pseudo-name")]
        [StringLength(15, ErrorMessage="Pseudo-name cannot exceeded 15")]
        [UserNameValidation]
        [UserNameCharacterValidation]
        [Display(Name = "Pseudo name")]
        public string Username { get; set; } = string.Empty;
        //pour correction apres enregistrement
        public string Xmatri { get; set; } = string.Empty;
        public string Usrid { get; set; } = string.Empty;
        public string BlzAddress { get; set; } = string.Empty;
        public bool IsPret { get; set; } = false;
        public int Orig { get; set; } = 0;
        public int Uorga { get; set; } = 0;
        public int Uhier { get; set; } = 0;
        public int Site { get; set; } = 0;
        public int Doma { get; set; } = 0;
        public string Stdoma { get; set; } = string.Empty;
        public string Stsite { get; set; } = string.Empty;
        public string Stsigle { get; set; } = string.Empty;
        //public string? IdStr { get; set; }
        public int Utyp { get; set; } = 0; //correspd a tietypn1agent2operat3client
        public int Styp { get; set; } = 0;
        public int Atyp { get; set; } = 0;
        public int Etyp { get; set; } = 0;
        public int Siperm { get; set; } = 0; //1permanent-2occasionnel
        public int Qui { get; set; } = 0; //inscrit type/12345678
        public string Nom { get; set; } = string.Empty;//inscrit nom
        public string Pnom { get; set; } = string.Empty;//inscrit pnoms
        public int Dequi { get; set; } = 0;
        public int Toqui { get; set; } = 0;
        public string Deqid { get; set; } = string.Empty;
        public string DeqEmail { get; set; } = string.Empty; //inviter mail
        public string DeqUname { get; set; } = string.Empty; //inviter username
        public string DeqPhone { get; set; } = string.Empty; //inviter phone
        public string DeqNom { get; set; } = string.Empty; //inviter username
        public string DeqPnom { get; set; } = string.Empty; //inviter username
        public DateTime Datope { get; set; } = DateTime.Now;
        public string Dqsrole { get; set; } = string.Empty;
        public int Tqirole { get; set; } = 0;
        public string Tqsrole { get; set; } = string.Empty;
        public int Methp { get; set; } = 0;
        //public int IntOwner { get; set; } = 0;
        public int NaisPays { get; set; } = 0; // inscrit pays naiss
        public int NaisCity { get; set; } = 0; // inscrit city naiss
        public int ResPays { get; set; } = 0; // inscrit pays resid
        public int ResCity { get; set; } = 0;// incrit city resid
        public int CdePays { get; set; } = 0; // cde pour pays
        public int NaisDat { get; set; } = 0; //aaaammdd
        public int ResDat { get; set; } = 0; //aaaammdd
        public string CodInvit { get; set; } = string.Empty; // code invite
        public string Uip { get; set; } = string.Empty; //inscrit Ip
        public int Idorg { get; set; } = 0; //inscrit orga
        public int Idhie { get; set; } = 0; //inscrit hierar
        public int Idpst { get; set; } = 0; //inscrit poste
        //public string ? Straison { get; set; }
        public string Raison { get; set; } = string.Empty; //inscrit raison
        public string Insrais { get; set; } = string.Empty; //inscrit raison
        public string Whats { get; set; } = string.Empty; //inscrit whatsapp
        public string Insfb { get; set; } = string.Empty; //inscrit facebook
        //public string Siteweb { get; set; } = string.Empty; //inscrit siteweb
        public string Tel { get; set; } = string.Empty; //inscrit tel
        public string Tel1 { get; set; } = string.Empty; //inscrit tel1
        public string Tel2 { get; set; } = string.Empty;
        public string Restel1 { get; set; } = string.Empty;
        //public string? Phon { get; set; } //inscrit phonx
        public string Phon1 { get; set; } = string.Empty;
        public string Phon2 { get; set; } = string.Empty;
        //public DateTime? Datc { get; set; }
        //public DateTime? Dtdval { get; set; }
        //public DateTime? Dtfval { get; set; }
        public int Eta { get; set; } = 0;
        public int Idtie { get; set; } = 0; //inscrit idtie
        public int Tietyp { get; set; } = 0; //inscrit typtie
        public string ICoda { get; set; } = string.Empty;
        public int Nbalea { get; set; } = 0;
        public string Uidcli { get; set; } = string.Empty;
        public string Uiddev { get; set; } = string.Empty;
        public string Strplus { get; set; } = string.Empty;
        public int Siparent { get; set; } = 0;
        public int Pere { get; set; } = 0;
        public int Mere { get; set; } = 0;
        public int Siparain { get; set; } = 0; //1parrain
        public int Siparten { get; set; } = 0; //1parten
        public string Uadr1 { get; set; } = string.Empty;
        public string U2nom { get; set; } = string.Empty;
        public string Glang { get; set; } = string.Empty;
        public string Mlang { get; set; } = string.Empty; //Lang des menus et contenus
        public string? Llang { get; set; } = string.Empty; //Lang des libelles charges
        public int Optsaie { get; set; } = 0; //1saisieparRubrique/2saisieparIndividu
        public int Opthem { get; set; } = 0; //theme
        public int Siset { get; set; } = 0; //si orga installe/lie
        public int Sidown { get; set; } = 0;
        public bool Siowner { get; set; } = false;
        public string Infoplus { get; set; } = string.Empty;  // id invite si invite non integre
        public string Obsv { get; set; } = string.Empty;
    }
    public class UserNameValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value.ToString().ToLower().Contains("admin"))
            {
                return new ValidationResult("The username cannot contain the word 'admin'");
            }
            return ValidationResult.Success;
        }
    }
    public class RegiscorModel
    {
        //pour correction apres enregistrement
        public int Id { get; set; } = 0;
        public string Email { get; set; } = string.Empty;
        public string Phonenumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Usrid { get; set; } = string.Empty;
        public string BlzAddress { get; set; } = string.Empty;
        public int Orig { get; set; } = 0;
        public int Uorga { get; set; } = 0;
        public int Uhier { get; set; } = 0;
        public int Site { get; set; } = 0;
        public int Doma { get; set; } = 0;
        public string Stdoma { get; set; } = string.Empty;
        public string Stsite { get; set; } = string.Empty;
        public string Stsigle { get; set; } = string.Empty;
        public int Utyp { get; set; } = 0; //correspd a tietypn1agent2operat3client
        public int Styp { get; set; } = 0;
        public int Atyp { get; set; } = 0;
        public int Etyp { get; set; } = 0;
        public int Siperm { get; set; } = 0; //1permanent-2occasionnel
        public int Qui { get; set; } = 0; //inscrit type/12345678
        public string Nom { get; set; } = string.Empty;//inscrit nom
        public string Pnom { get; set; } = string.Empty;//inscrit pnoms
        public int Dequi { get; set; } = 0;
        public int Toqui { get; set; } = 0;
        public string Deqid { get; set; } = string.Empty;
        public string DeqEmail { get; set; } = string.Empty; //inviter mail
        public string Datope { get; set; } = string.Empty;
        public string Dqsrole { get; set; } = string.Empty;
        public int Tqirole { get; set; } = 0;
        //public int IntOwner { get; set; } = 0;
        public int NaisPays { get; set; } = 0; // inscrit pays naiss
        public int NaisCity { get; set; } = 0; // inscrit city naiss
        public int ResPays { get; set; } = 0; // inscrit pays resid
        public int ResCity { get; set; } = 0;// incrit city resid
        public int CdePays { get; set; } = 0; // cde pour pays
        public string CodInvit { get; set; } = string.Empty; // code invite
        public string Uip { get; set; } = string.Empty; //inscrit Ip
        public int Idorg { get; set; } = 0; //inscrit orga
        public int Idhie { get; set; } = 0; //inscrit hierar
        //public string ? Straison { get; set; }
        public string Raison { get; set; } = string.Empty; //inscrit raison
        public string Whats { get; set; } = string.Empty; //inscrit whatsapp
        public string Insfb { get; set; } = string.Empty; //inscrit facebook
        //public string Siteweb { get; set; } = string.Empty; //inscrit siteweb
        public string Tel { get; set; } = string.Empty; //inscrit tel
        public string Tel1 { get; set; } = string.Empty; //inscrit tel1
        public string Phon1 { get; set; } = string.Empty;
        public int Eta { get; set; } = 0;
        public int Idtie { get; set; } = 0; //inscrit idtie
        public int Tietyp { get; set; } = 0; //inscrit typtie
        public string ICoda { get; set; } = string.Empty;
        public string Uidcli { get; set; } = string.Empty;
        public string Uiddev { get; set; } = string.Empty;
        public int Siset { get; set; } = 0; //si orga installe/lie
        public int Sidown { get; set; } = 0;
        public bool Siowner { get; set; } = false;
        public string Obsv { get; set; } = string.Empty;
    }
    public class UserNameCharacterValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var username = value as string;
            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9]*$"))
            {
                return new ValidationResult("Username can only contain letters and numbers.");
            }
            return ValidationResult.Success;
        }
    }
}
