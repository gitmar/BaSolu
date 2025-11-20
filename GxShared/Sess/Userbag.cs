using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

using GxShared;
using GxShared.Sess;
using GxShared.Others;

namespace GxShared.Sess
{
    public class Userbag
    {
        [Key]
        public long Id { get; set; }
        public string Userid { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool Succeeded { get; set; } = false;
        public int Affecta { get; set; } = 0;
        public int Fonctio { get; set; } = 0;
        public int Grade { get; set; } = 0;
        public bool Isauthen { get; set; } = false;
        public bool Yauser { get; set; } = false;
        public bool Yaorga { get; set; } = false;
        public int Idorg { get; set; } = 0;
        public string Graison { get; set; } = string.Empty;
        public string Gsigle { get; set; } = string.Empty;
        public int Uhieid { get; set; } = 0;
        //user attributs
        public int SiOrga { get; set; } = 0; //Admin orga
        public bool Simain { get; set; } = false;
        public bool SiOwner { get; set; } = false;
        // ✅ Add this flag
        public bool IsPuzzleReady { get; set; } = false;
        public Gpuser Usprof { get; set; } = new Gpuser();
        public Gporga Usorga { get; set; } = new Gporga();
        public ICollection<AdmOrga> Allorgas { get; set; } = new List<AdmOrga>();
        public ICollection<Grole> Uroles { get; set; } = new List<Grole>();
        public ICollection<Gpfixe> Ufixes { get; set; } = new List<Gpfixe>();
        //public ICollection<Mys> Shvars { get; set; } = new();
        public ICollection<string> ErrList { get; set; } = new List<string>();
    }
    public class Gpuser
    {
        public string Userid { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int Utyp { get; set; } = 0;
        public int Rlmin { get; set; } = 0;
        public int Rlmax { get; set; } = 0;
        public string Unom { get; set; } = string.Empty;
        public string Upnom { get; set; } = string.Empty;
        public string Ufulnom { get; set; } = string.Empty;
        public string Uemail { get; set; } = string.Empty;
        public string Uphone { get; set; } = string.Empty;
        public int Eta { get; set; } = 0;
    }
    public class Gpfixe
    {
        public int Fxseq { get; set; } = 0;
        public int Code { get; set; } = 0;
        public int Gvars { get; set; } = 0;
        public int Itb { get; set; } = 0;
        public int Elea { get; set; } = 0;
        public int Ele2 { get; set; } = 0;
        public string Liba { get; set; } = string.Empty;
        public string Abg { get; set; } = string.Empty;
        public int Totyp { get; set; } = 0;
        public int Gp1 { get; set; } = 0;
        public int Gp2 { get; set; } = 0;
        public int Gp3 { get; set; } = 0;
        public int Gp4 { get; set; } = 0;
        public int Gp5 { get; set; } = 0;
        public int Gp6 { get; set; } = 0;
        public int Gp7 { get; set; } = 0;
        public string Scdrub { get; set; } = string.Empty;
        public int Sicur { get; set; } = 0;
        public int Eta { get; set; } = 0;
    }
    public class Gporga
    {
        public int Idorg { get; set; } = 0;
        public int Code { get; set; } = 0;
        public string Liba { get; set; } = string.Empty;
        public string Abg { get; set; } = string.Empty;
        public string Raison { get; set; } = string.Empty;
        public string Sigle { get; set; } = string.Empty;
        public string Stcible { get; set; } = string.Empty;
        public int Gdtie { get; set; } = 0;
        public int Torg { get; set; } = 0;
        public int Dom { get; set; } = 0;
        public int Pays { get; set; } = 0;
        public string Stdom { get; set; } = string.Empty;
        public string Stserver { get; set; } = string.Empty;
        public int? Schba { get; set; } = 0;
        public int? Schbg { get; set; } = 0;
        public int? Schbk { get; set; } = 0;
        public int? Sicnjoint { get; set; } = 0;
        public int? Siparent { get; set; } = 0;
        public int? Sienfant { get; set; } = 0;
        public int? Siparain { get; set; } = 0;
        public int? Sifiheul { get; set; } = 0;
        public int? Cnjmax { get; set; } = 0;
        public int? Parmax { get; set; } = 0;
        public int? Enfmax { get; set; } = 0;
        public int? Paimax { get; set; } = 0;
        public int? Simaint { get; set; } = 0;
        public int? Flhmax { get; set; } = 0;
        public bool Sicoordo { get; set; } = false;
        public string CurIp { get; set; } = string.Empty;
        public ICollection<Gpvar> Uvars { get; set; } = new List<Gpvar>();
        //public ICollection<Gphie> Uhies { get; set; } = new List<Gphie>();
        public ICollection<Gplan> Uplns { get; set; } = new List<Gplan>();
    }
    public class Gpvar
    {
        public int Xseq { get; set; } = 0;
        public int Code { get; set; } = 0;
        public int Xsac { get; set; } = 0;
        public int Gvars { get; set; } = 0;
        public int Itb { get; set; } = 0;
        public int Ade { get; set; } = 0;
        public int Elea { get; set; } = 0;
        public int Sigrid { get; set; } = 0;
        public int Ele2 { get; set; } = 0;
        public string Liba { get; set; } = string.Empty;
        public string Abg { get; set; } = string.Empty;
        public string Sliba { get; set; } = string.Empty;
        public string Sabg { get; set; } = string.Empty;
        public int Tovers { get; set; } = 0;
        public int Idorg { get; set; } = 0;
        public int Idhie { get; set; } = 0;
        public int Dpdce { get; set; } = 0;
        public int Xdpd { get; set; } = 0;
        public int Ydpd { get; set; } = 0;
        public int Epai { get; set; } = 0;
        public int Totyp { get; set; } = 0;
        public int Ityp { get; set; } = 0;
        public int Etyp { get; set; } = 0;
        public int Ztyp { get; set; } = 0;
        public int Jtyp { get; set; } = 0;
        public int Ktyp { get; set; } = 0;
        public int Ltyp { get; set; } = 0;
        public int Zgpe { get; set; } = 0;
        public int Zfro { get; set; } = 0; 
        public string Scdrub { get; set; } = string.Empty;
        public int Ivala { get; set; } = 0;
        public string Svala { get; set; } = string.Empty;
        public float Rvala { get; set; } = 0;
        public int Pseq { get; set; } = 0;
        public int Gtyp { get; set; } = 0;
        public int Eta { get; set; } = 0;

    }
    //public class Gphie
    //{
    //    public int Idhie { get; set; } = 0;
    //    public int Code { get; set; } = 0;
    //    public string Refa { get; set; } = string.Empty;
    //    public int Tyhie { get; set; } = 0;
    //    public int Siroot { get; set; } = 0;
    //    public int Sidiv { get; set; } = 0;
    //    public int Idorg { get; set; } = 0;
    //    public string Liba { get; set; } = string.Empty;
    //    public string Abg { get; set; } = string.Empty;
    //    public int Horiz { get; set; } = 0;
    //    public int Gaudr { get; set; } = 0;
    //    public int Idpln { get; set; } = 0;
    //    public int Udpd { get; set; } = 0;
    //    public bool Sigpe { get; set; } = false; //gpe menu
    //    public string Scdrub { get; set; } = string.Empty;
    //    public int Eta { get; set; } = 0;
    //}
    public class Gplan
    {
        public int Idpln { get; set; } = 0;
        public int Code { get; set; } = 0;
        public int Ptyp { get; set; } = 0;
        public int Xsac { get; set; } = 0;
        public int Elea { get; set; } = 0;
        public int Ele2 { get; set; } = 0;
        public int Idorg { get; set; } = 0;
        public int Idhie { get; set; } = 0;
        public string Liba { get; set; } = string.Empty;
        public string Abg { get; set; } = string.Empty;
        public int Todom { get; set; } = 0;
        public int Tovue { get; set; } = 0;
        public int Toatr { get; set; } = 0;
        public int Topln { get; set; } = 0;
        public int Totyp { get; set; } = 0;
        public int Tostyp { get; set; } = 0;
        public int Totie { get; set; } = 0;
        public int Tovers { get; set; } = 0;
        public int Eta { get; set; } = 0;
    }
    public class AdmOrga
    {
        public int Idorg { get; set; } = 0;
        public string Raison { get; set; } = string.Empty;
        public string Sigle { get; set; } = string.Empty;
        public int Simain { get; set; } = 0;
        public string WebUrl { get; set; } = string.Empty;
        public int Usog { get; set; } = 0;
    }

    //public class Gpses
    //{
    //    public int Idses { get; set; } = 0;
    //    public int Dseq { get; set; } = 0;
    //    public int Code { get; set; } = 0;
    //    public int Idpln { get; set; } = 0;
    //    public int Styp { get; set; } = 0;
    //    public int Nbs { get; set; } = 0;
    //    public int Tperi { get; set; } = 0;
    //    public string Speri { get; set; } = string.Empty;
    //    public string Sdeb { get; set; } = string.Empty;
    //    public string Sfin { get; set; } = string.Empty;
    //    public int Ideb { get; set; } = 0;
    //    public int Ifin { get; set; } = 0;
    //    public string Liba { get; set; } = string.Empty;
    //    public string Abg { get; set; } = string.Empty;
    //    public int Tovers { get; set; } = 0;
    //    public int Idorg { get; set; } = 0;
    //}
    //public class Ifix
    //{
    //    public int Fxseq { get; set; }
    //    public int? Itb { get; set; }
    //    public int? Elea { get; set; }
    //    public int? Ele2 { get; set; }
    //    public int? Ele3 { get; set; }
    //    public int? Gvars { get; set; }
    //    public int? Gp1 { get; set; }
    //    public int? Gp2 { get; set; }
    //    public int? Gp3 { get; set; }
    //    public string? Liba { get; set; }
    //    public string? Lib2 { get; set; }
    //    public string? Abg { get; set; }
    //    public int? Codi { get; set; }
    //    public int? Eta { get; set; }
    //    public int? Sicur { get; set; }
    //    public string? Refa { get; set; }
    //    public string? Scdrub { get; set; }
    //    public int? Etyp { get; set; }
    //    public int? Eta { get; set; }
    //}
}

public class Apirole
{
    public string Rname { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Level { get; set; } = 0;
    public bool Sicur { get; set; } = false;
}