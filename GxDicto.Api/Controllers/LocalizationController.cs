using GxDicto.Api.Data;
using GxDicto.DModels;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GxDicto.Api.Controllers
{
    [ApiController]
    [Route("api/localization/{orgId}")]
    public class LocalizationController : ControllerBase
    {
        private readonly LocalizationDbContext _db;

        public LocalizationController(LocalizationDbContext db)
        {
            _db = db;
        }

        [HttpGet("{language}")]
        public ActionResult<Dictionary<string, string>> GetTranslations(int orgId, string language)
        {
            var dict = _db.LocalizationEntries
                          .Where(e => e.Idorg == orgId && e.Language == language)
                          .ToDictionary(e => e.Key, e => e.Value);
            return Ok(dict);
        }

        [HttpPost("admin/entries")]
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
    }
    //[ApiController]
    //[Route("api/localization/{orgId}")]
    //public class LocalizationController : ControllerBase
    //{
    //    private readonly LocalizationRepository _repo;

    //    public LocalizationController(LocalizationRepository repo)
    //    {
    //        _repo = repo;
    //    }

    //    // GET api/localization/{orgId}/{language}
    //    [HttpGet("{language}")]
    //    public ActionResult<Dictionary<string, string>> GetTranslations(int orgId, string language)
    //    {
    //        var dict = _repo.GetTranslations(orgId, language);
    //        return Ok(dict);
    //    }

    //    // POST api/localization/{orgId}/admin/entries
    //    [HttpPost("admin/entries")]
    //    public IActionResult AddEntry(int orgId, [FromBody] LocalizationEntry entry)
    //    {
    //        if (_repo.Exists(orgId, entry.Language, entry.Key))
    //            return Conflict($"Key {entry.Key} already exists.");

    //        _repo.AddEntry(orgId, entry);
    //        return CreatedAtAction(nameof(GetTranslations),
    //            new { orgId, language = entry.Language }, entry);
    //    }
    //}
}
