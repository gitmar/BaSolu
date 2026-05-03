using System.Security.Claims;
using GxShared.Others;
using GxShared.GxDtos;
using GxShared.Sess;

namespace GxTie.ClieModels
{
    public class MyShareVars()
    {
        public bool isAuthenticated { get; set; } = false;
        public ClaimsPrincipal? authUser { get; set; }
        //public int Idorg { get; set; } = 0;
        //public string Usrid { get; set; } = string.Empty;
        public int Ugpe { get; set; } = 0;
        public List<Gpfixe> LsFixes { get; set; } = new();
        public List<Gpvar> LsVars { get; set; } = new();
        public List<Gplan> LsPlans { get; set; } = new();
        public List<Gpvar> LsTabls { get; set; } = new();
        public List<Gpdivh> LsDivs { get; set; } = new();
        public List<RubhieDto> LsHies { get; set; } = new();
        //public List<Gsglne> LsCols { get; set; } = new();
        public List<Gtvar> TbElems { get; set; } = new();
        public bool MainOptsOk { get; set; } = false;
        public List<GsesioDto> LsSess { get; set; } = new();
        public GxorgaDto curOrga { get; set; } = new();
        public PlngenDto curProg { get; set; } = new();
        public GsesioDto curExo { get; set; } = new();
        public GsesioDto curSesio { get; set; } = new();
        public RubhieDto curSite { get; set; } = new();
    }   
    public class MyMenuVars()
    {
        public int Ugpe { get; set; } = 0;
        public int Uexo { get; set; } = 0;
        public int Usesio { get; set; } = 0;
        public int Usite { get; set; } = 0;
    }
}
