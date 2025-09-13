using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MmaSolution.AppApi.Infrastrcture.Attributes;
using MmaSolution.Services.Settings;

using System;
using System.Threading.Tasks;

namespace MmaSolution.AppApi.Controllers.v1.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysSettingsController : ControllerBase
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SysSettingsController> _logger;

        public SysSettingsController(IServiceScopeFactory scopeFactory, ILogger<SysSettingsController> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        [HttpGet("GetValue/{key}")]
        [RequiredPermission("Read")]
        public IActionResult GetValue(string key)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                 var service = scope.ServiceProvider.GetRequiredService<SysSettingsService>();
                var res = service.GetSetting(key);
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(GetValue)}({key}): {ex.Message}", ex);
                return BadRequest();
            }
        }

        [HttpGet("GetValueAsync/{key}")]
        [RequiredPermission("Read")]
        public async Task<IActionResult> GetValueAsync(string key)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<SysSettingsService>();
                var res = await service.GetSettingAsync(key);
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(GetValueAsync)}({key}): {ex.Message}", ex);
                return BadRequest();
            }
        }


        [HttpGet("GetDict")]
        [RequiredPermission("Read")]
        public IActionResult GetDict([FromQuery]string keys)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<SysSettingsService>();
                var res = service.GetSettings(keys);
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(GetDict)}([{string.Join(',', keys)}]): {ex.Message}", ex);
                return BadRequest();
            }
        }

        [HttpGet("GetDictAsync")]
        [RequiredPermission("Read")]
        public async Task< IActionResult> GetDictAsync([FromQuery] string keys)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<SysSettingsService>();
                var res = await service.GetSettingsAsync(keys);
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(GetDictAsync)}([{string.Join(',', keys)}]): {ex.Message}", ex);
                return BadRequest();
            }
        }
    }
}
