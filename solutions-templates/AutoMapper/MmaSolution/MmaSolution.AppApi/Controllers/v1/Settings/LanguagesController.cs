using MmaSolution.Core.Database.Localization;

namespace MmaSolution.AppApi.Controllers.v1.Settings;

[Route("api/v{version:apiVersion}/languages")]
[ApiVersion(1.0)]
[ApiController]
[ApiExplorerSettings(IgnoreApi = false)]
public class LanguagesController : BaseController
{

    private readonly ILogger<LanguagesController> _logger;
    private readonly ApplicationDbContext _context;
    public LanguagesController(Services.Translator translator, ILogger<LanguagesController> logger, ApplicationDbContext context) : base(translator)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    [RequiredPermission("Read")]
    public IActionResult Get()
    {
        return Ok(_context.Languages.ToList());
    }

    [HttpGet("{languageId}")]
    [RequiredPermission("Read")]
    public IActionResult GetCurrent(int languageId)
    {
        return Ok(_context.Languages.Find(languageId));
    }

    [HttpPost]
    [RequiredPermission("Create")]
    public IActionResult Set([FromBody] Language language)
    {
        _context.Languages.Add(language);
        _context.SaveChanges();
        return Ok(language);
    }

    [HttpPut("{languageId}")]
    [RequiredPermission("Update")]
    public IActionResult Update(int languageId, Language language)
    {
        _context.Languages.Update(language);
        _context.SaveChanges();
        return Ok(language);
    }

    [HttpDelete("{languageId}")]
    [RequiredPermission("Update,Delete")]
    public IActionResult Delete(int languageId)
    {
        var language = _context.Languages.Find(languageId);
        _context.Resources.RemoveRange(language.Resources);
        _context.Languages.Remove(language);
        _context.SaveChanges();
        return Ok();
    }


    [HttpGet("{languageId}/resources")]
    [RequiredPermission("Read")]
    public IActionResult GetResources(int languageId)
    {
        return Ok(_context.Resources.Where(r => r.LanguageId == languageId).ToList());
    }

    [HttpGet("{languageId}/resources/{resourceId}")]
    [RequiredPermission("Read")]
    public IActionResult GetCurrent(int languageId, int resourceId)
    {
        return Ok(_context.Resources
            .FirstOrDefault(r => r.LanguageId == languageId && r.Id == resourceId));
    }

    [HttpPost("{languageId}/resources")]
    [RequiredPermission("Create")]
    public IActionResult SetResource([FromBody] Resource resource)
    {
        _context.Resources.Add(resource);
        _context.SaveChanges();
        return Ok(resource);
    }

    [HttpPut("{languageId}/resources/{resourceId}")]
    [RequiredPermission("Update")]
    public IActionResult ResetResource(int languageId, int resourceId, Resource resource)
    {
        _context.Resources.Update(resource);
        _context.SaveChanges();
        return Ok(resource);
    }

    [HttpDelete("{languageId}/resources/{resourceId}")]
    [RequiredPermission("Update,Delete")]
    public IActionResult DleteResource(int languageId, int resourceId)
    {
        var resource = _context.Resources.Find(resourceId);
        _context.Resources.Remove(resource);
        _context.SaveChanges();
        return Ok();
    }
}