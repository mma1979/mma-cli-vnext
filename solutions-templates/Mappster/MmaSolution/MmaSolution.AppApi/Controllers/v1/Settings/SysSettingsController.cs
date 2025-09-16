namespace MmaSolution.AppApi.Controllers.v1.Settings;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1.0)]
[ApiController]
public class SysSettingsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SysSettingsController> _logger;

    public SysSettingsController(IConfiguration configuration, ILogger<SysSettingsController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [HttpGet("GetValue/{key}")]
    [RequiredPermission("Read")]
    public IActionResult GetValue(string key)
    {
        try
        {

            var res = _configuration.GetValue<Dictionary<string, object>>(key);
            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{DateTime.UtcNow} - {nameof(GetValue)}({key}): {ex.Message}", ex);
            return BadRequest();
        }
    }

}