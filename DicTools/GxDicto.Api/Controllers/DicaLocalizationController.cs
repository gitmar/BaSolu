using GxDicto.Api.Data;
using GxDicto.Api.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GxDicto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DicaLocalizationController : ControllerBase
    {
        //private readonly LocalizationDbContext _db;
        private readonly dxdocuContext _db;

        public DicaLocalizationController(dxdocuContext db)
        {
            _db = db;
        }

        [HttpGet("{orgId:int}/{language}")]
        public ActionResult<Dictionary<string, string>> GetTranslations(int orgId, string language)
        {
            var dict = _db.LocalizationEntries
                          .Where(e => e.Idorg == orgId && e.Language == language)
                          .ToDictionary(e => e.Key, e => e.Value);
            return Ok(dict);
        }

        [HttpGet("{orgId:int}/admin/entries")]
        public async Task<ActionResult<List<LocalizationEntry>>> GetAdminEntries(int orgId)
        {
            var entries = await _db.LocalizationEntries
                .Where(e => e.Idorg == orgId)
                .ToListAsync();

            return Ok(entries);
        }

        [HttpPost("{orgId:int}/admin/entries")]
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
    }
}
