namespace MmaSolution.AppApi.Controllers.v1.Settings
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(1.0)]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CacheController> _logger;

        public CacheController(IServiceScopeFactory scopeFactory, ILogger<CacheController> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        [HttpDelete("clear")]
        [AllowAnonymous]
        public IActionResult Clear()
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<ICacheService>();
                service.Clear();
                return Ok();
            }
            catch (Exception ex)
            {

                _logger.LogError($"{DateTime.UtcNow} - {nameof(Clear)}(): {ex.Message}", ex);
                return BadRequest();
            }
        }

    }
}
