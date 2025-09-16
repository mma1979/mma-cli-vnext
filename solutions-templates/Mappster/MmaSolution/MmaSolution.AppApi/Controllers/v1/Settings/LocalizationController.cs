namespace MmaSolution.AppApi.Controllers.v1.Settings;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1.0)]
[ApiController]
[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = false)]
public class LocalizationController : ControllerBase
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<LocalizationController> _logger;

    public LocalizationController(IServiceScopeFactory scopeFactory, ILogger<LocalizationController> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    [HttpGet("translate")]
    public string GetTranslation([FromQuery] string key, [FromQuery] string language = LanguageCode.Arabic)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<LocalizationSerivce>();
            var res = service.Translate(key, language);
            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{DateTime.UtcNow} - {nameof(GetTranslation)}({key}): {ex.Message}", ex);
            return key;
        }
    }

    [HttpGet("translate-async")]
    public async Task<string> GetTranslationAsync([FromQuery] string key, [FromQuery] string language = LanguageCode.Arabic)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<LocalizationSerivce>();
            var res = await service.TranslateAsync(key, language);
            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{DateTime.UtcNow} - {nameof(GetTranslation)}({key}): {ex.Message}", ex);
            return key;
        }
    }

    [HttpGet("translate-array")]
    public async Task<string[]> GetTranslationAsync([FromQuery] string[] keys, [FromQuery] string language = LanguageCode.Arabic)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<LocalizationSerivce>();
            var res = await service.TranslateAsync(keys, language);
            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{DateTime.UtcNow} - {nameof(GetTranslation)}([{string.Join(',', keys)}]): {ex.Message}", ex);
            return keys;
        }
    }

    [HttpGet("translate-dict")]
    public async Task<Dictionary<string, string>> GetTranslationDictAsync([FromQuery] string[] keys, [FromQuery] string language = LanguageCode.Arabic)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<LocalizationSerivce>();
            var res = await service.TranslateDictAsync(keys, language);
            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{DateTime.UtcNow} - {nameof(GetTranslation)}([{string.Join(',', keys)}]): {ex.Message}", ex);
            return keys.ToDictionary(e => e, e => e);
        }
    }

}