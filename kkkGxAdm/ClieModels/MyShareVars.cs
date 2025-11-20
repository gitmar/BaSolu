using System.Security.Claims;
using GxShared.GlobModels;
using GxWapi.DaModels;

namespace GxAdm.ClieModels
{
    public class MyShareVars()
    {
        public bool isAuthenticated { get; set; }
        public ClaimsPrincipal authUser { get; set; }
        //public int Idorg { get; set; } = 0;
        //public string Usrid { get; set; } = string.Empty;
        public int Ugpe { get; set; } = 0;
        public List<Gpfixe> LsFixes { get; set; } = new();
        public List<Gpvar> LsVars { get; set; } = new();
        public List<Gplan> LsPlans { get; set; } = new();
        public List<Gpvar> LsTabls { get; set; } = new();
        public List<Gpdivh> LsDivs { get; set; } = new();
        public List<Rubhie> LsHies { get; set; } = new();
        public List<Gsglne> LsCols { get; set; } = new();
        public List<Gsglne> TbElems { get; set; } = new();
        public bool MainOptsOk { get; set; } = false;
        public List<Gsesio> LsSess { get; set; } = new();
        public Gxorga curOrga { get; set; } = new();
        public Plngen curProg { get; set; } = new();
        public Gsesio curExo { get; set; } = new();
        public Gsesio curSesio { get; set; } = new();
        public Rubhie curSite { get; set; } = new();
    }   
    public class MyMenuVars()
    {
        public int Ugpe { get; set; }
        public int Uexo { get; set; }
        public int Usesio { get; set; }
        public int Usite { get; set; }
    }
}
