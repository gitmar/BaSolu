using System.Text;

using GxDicto.Api.Data;
using GxDicto.Api.Models;
using GxDicto.DModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GxDicto.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DicoLocalizationController : ControllerBase
    {
        private readonly LocalizationDbContext _db;

        public DicoLocalizationController(LocalizationDbContext db)
        {
            _db = db;
        }

        [HttpGet("{idorg:int}/{language}")]
        public ActionResult<Dictionary<string, string>> GetTranslations(int orgId, string language)
        {
            var dict = _db.LocalizationEntries
                          .Where(e => e.Idorg == orgId && e.Language == language)
                          .ToDictionary(e => e.Key, e => e.Value);
            return Ok(dict);
        }

        //[HttpPost("admin/entries")]
        [HttpPost("{idorg:int}/admin/entries")]
        public IActionResult AddEntry(int orgId, [FromBody] LocalizationEntry entry)
        {
            if (_db.LocalizationEntries.Any(e => e.Idorg == orgId && e.Language == entry.Language && e.Key == entry.Key))
                return Conflict($"Key {entry.Key} already exists.");

            entry.Idorg = orgId;
            _db.LocalizationEntries.Add(entry);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetTranslations),
                new { orgId, language = entry.Language }, entry);
        }

        [HttpPost("scanner/sync")]
        public async Task<IActionResult> SyncScannerResults(
            [FromBody] ScannerSyncRequest request)
        {
            if (request.IdOrg != -1)
            {
                return BadRequest(
                    "Scanner synchronization must use IdOrg = -1.");
            }

            if (!string.Equals(
                    request.Lang,
                    "fr",
                    StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(
                    "Scanner synchronization must use Lang = 'fr'.");
            }
            foreach (var item in request.Labels)
            {
                await _db.Database.ExecuteSqlInterpolatedAsync(
                    $@"EXEC dbo.sp_Gdbse_UpsertScannerLabel
                        @LabelKey = {item.LabelKey},
                        @Label    = {item.Label},
                        @Gvars    = {item.Gvars},
                        @Itb      = {item.Itb}");
            }
            return Ok(new
            {
                insertedOrUpdated = request.Labels.Count
            });
        }
        // GET api/localization/{orgId}/{language}
        //[HttpGet("{language}")]
        //public ActionResult<Dictionary<string, string>> GetTranslations(int orgId, string language)
        //{
        //    var dict = _repo.GetTranslations(orgId, language);
        //    return Ok(dict);
        //}
    }
}