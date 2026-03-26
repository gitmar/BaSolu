using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

using GxShared.Helpers;
using GxShared.Identity;
using GxShared.Others;
using GxShared.ShdServices;

using GxWapi.DaModels;
using GxWapi.Data;
using GxWapi.Services;

using Humanizer;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json.Linq;

using NuGet.Common;

using static System.Net.WebRequestMethods;


namespace GxWapi.Areas.Identity.Pages.Account
{
    public class GxRegisterModel : PageModel
    {
        private readonly DxsoluContext _bcontext;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly LinkSerialiser _linkSerialiser;
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _ucontext;
        private readonly ITableService _tableService;

        public GxRegisterModel(
            DxsoluContext bcontext,
            RoleManager<ApplicationRole> roleManager,
            LinkSerialiser linkSerialiser,
            ApplicationDbContext ucontext,
            HttpClient httpClient,
            ITableService tableService)
        {
            _bcontext = bcontext;
            _roleManager = roleManager;
            _linkSerialiser = linkSerialiser;
            _ucontext = ucontext;
            _httpClient = httpClient;
            _tableService = tableService;
        }

        //[BindProperty]
        //public InputModel myInput { get; set; } = new();

        // ✅ UI SUPPORT DATA (NOT BOUND)
        public TableSet _myCatal { get; private set; } = new();
        public List<RoleDto> _myRoles { get; set; } = new();
        public string? Imessage { get; set; }
        public string Successmsg { get; set; } = string.Empty;
        public bool IsValid { get; private set; }
        //nom modifiable data
        public string SxOrig { get; set; } = string.Empty;
        public string Raison { get; set; } = string.Empty;
        public string Sigle { get; set; } = string.Empty;
        public string Sidom { get; set; } = string.Empty;
        public string WebUrl { get; set; } = string.Empty;
        public int IxRole { get; set; } = 0;
        public bool siAutoRegis { get; set; } = false;
        // ✅ Page properties (separate from form)
        [BindProperty]
        public InviteLinkData? InviteData { get; set; }
        // ✅ InputModel stays clean for form binding
        [BindProperty]
        public RegisterDto MyRegisDto { get; set; } = new();
        public IActionResult OnGet(string? cdlink)
        {
            siAutoRegis = false;
            if (string.IsNullOrEmpty(cdlink))
            {
                InviteData = null;
                return BadRequest("le lien est obligatoire");
            }
            try
            {
                // ✅ Get tables
                RendsTablesAndRoles();
                InviteData = _linkSerialiser.DecodeAndDeserialize<InviteLinkData>(cdlink);
                
                if (InviteData == null)
                    return BadRequest("Lien invalide ou corrompu");
                if (InviteData.Idorg == 0)
                    return BadRequest("Organisation invalide");
                var inOrga = _bcontext.Gxorgas
                    .FirstOrDefault(og => og.Idorg == InviteData.Idorg);
                // ✅ Retrieve curOrga
                if (inOrga == null)
                    return BadRequest("Institution obligatoire");
                
                // ✅ Verification Registration type
                RegisType regtype = RegisType.Standard;
                if (InviteData.Ivrole == 1)
                {
                    regtype = RegisType.Support;
                } else if (InviteData.Ivrole == 2)
                {
                    regtype = RegisType.Owner;
                } else if (InviteData.Ivrole == 3)
                {
                    regtype = RegisType.Admin;
                }
                // ✅ Pre-fill ALL fields from link
                MyRegisDto = new RegisterDto
                {
                    Email = InviteData.Email,
                    Username = InviteData.Username,
                    Nom = InviteData.Nom,
                    Pnom = InviteData.Pnom,
                    PhoneNumber = InviteData.PhoneNumber,
                    RgType = regtype, //pour enregistrement en hidden input
                    EmType = EmailType.Confirmation, //sera gere par Register
                    Refapl = InviteData.Refapl ?? "ref.xyz",
                };
                // ✅ non modifiable data
                Sidom = inOrga.Sidom;
                Raison = inOrga.Raison;
                Sigle = inOrga.Sigle;
                WebUrl = inOrga.Weburl;
                IxRole = InviteData.Ivrole;
            }
            catch
            {
                InviteData = null;
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //    return Page();
            //+ validated old InviteData data
            MyRegisDto.Idorg = InviteData.Idorg;
            MyRegisDto.Utyp = InviteData.Utyp;
            MyRegisDto.Ivrole = InviteData.Ivrole;
            MyRegisDto.Refapl = InviteData.Refapl;
            MyRegisDto.FbackUrl = InviteData.FbackUrl;
            MyRegisDto.Itoken = InviteData.Itoken;
            MyRegisDto.Ihie = InviteData.Ihie;
            MyRegisDto.Ipst = InviteData.Ipst;
            MyRegisDto.Email = InviteData.Email ?? MyRegisDto.Brevman.FirstOrDefault()?.Email;
            //MyRegisDto.EmType = InviteData.EmType;
            //MyRegisDto.RgType = InviteData.RgType;
            try
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";

                var response = await _httpClient.PostAsJsonAsync(
                    $"{baseUrl}/api/regis/register",
                    MyRegisDto);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", error);
                    return Page();
                    //return RedirectToPage("Success");
                }
            } catch(Exception ex)
            {
                return BadRequest($"Erreur enregistrement {ex.Message}");
            }
            return RedirectToPage("registersuccess");
        }
        private async void RendsTablesAndRoles()
        {
            //tables
            TableSet rdtable = await _tableService.GetTablesAsync();
            if (rdtable == null)
            {
                _myCatal = new TableSet();
            }
            _myCatal = rdtable;
            //roles
            _myRoles = _roleManager.Roles
                .Select(r => new RoleDto
                {
                    Name = r.Name,
                    Level = r.Level,
                    Description = r.Description
                })
                .OrderBy(ur => ur.Level)
                .ToList();
        }
        public class RInputModel
        {
            public string Raison { get; set; } = string.Empty;
            public string Sigle { get; set; } = string.Empty;
            public string Sidom { get; set; } = string.Empty;
            public string WebUrl { get; set; } = string.Empty;
            public EmailType EmType { get; set; }
            public RegisType Rgtype { get; set; }
        }
        public class RoleDto
        {
            public string Name { get; set; }
            public int Level { get; set; }
            public string Description { get; set; }
        }
    }
}
