using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MmaSolution.Services.Chache;

using System;

namespace MmaSolution.AppApi.Controllers.v1.Settings
{
    [Route("api/[controller]")]
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
