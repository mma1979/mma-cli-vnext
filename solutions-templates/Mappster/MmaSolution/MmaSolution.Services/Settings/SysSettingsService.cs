using AngleSharp.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MmaSolution.EntityFramework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MmaSolution.Services.Settings
{
    public class SysSettingsService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SysSettingsService> _logger;

        public SysSettingsService(IServiceScopeFactory scopeFactory, ILogger<SysSettingsService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public string GetSetting(string key)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var value = context.SysSettings
                    .FirstOrDefault(e=>e.SysKey==key)
                    ?.SysValue;

                if(string.IsNullOrEmpty(value))
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(GetSetting)}({key}): Value is null");
                    return null;
                }

                return value;

            }
            catch (Exception ex)
            {

                _logger.LogError($"{DateTime.UtcNow} - {nameof(GetSetting)}({key}): {ex.Message}", ex);
                return null;
            }

        }

        public async Task<string> GetSettingAsync(string key)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var value = (await context.SysSettings
                    .FirstOrDefaultAsync(e => e.SysKey == key))
                    ?.SysValue;

                if (string.IsNullOrEmpty(value))
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(GetSetting)}({key}): Value is null");
                    return null;
                }

                return value;

            }
            catch (Exception ex)
            {

                _logger.LogError($"{DateTime.UtcNow} - {nameof(GetSetting)}({key}): {ex.Message}", ex);
                return null;
            }

        }

        public Dictionary<string,string> GetSettings(params string[] keys)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var dict = context.SysSettings
                    .Where(e => keys.Contains(e.SysValue))
                    .ToDictionary(e => e.SysKey, e => e.SysValue);

                if (dict is null || dict.Count <=0)
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(GetSetting)}([{string.Join(',',keys)}]): Value is null");
                    return null;
                }

                return dict;

            }
            catch (Exception ex)
            {

                _logger.LogError($"{DateTime.UtcNow} - {nameof(GetSetting)}([{string.Join(',', keys)}]): {ex.Message}", ex);
                return null;
            }

        }

        public async Task<Dictionary<string, string>> GetSettingsAsync(params string[] keys)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var dict = await context.SysSettings
                    .Where(e => keys.Contains(e.SysValue))
                    .ToDictionaryAsync(e => e.SysKey, e => e.SysValue);

                if (dict is null || dict.Count <= 0)
                {
                    _logger.LogWarning($"{DateTime.UtcNow} - {nameof(GetSetting)}([{string.Join(',', keys)}]): Value is null");
                    return null;
                }

                return dict;

            }
            catch (Exception ex)
            {

                _logger.LogError($"{DateTime.UtcNow} - {nameof(GetSetting)}([{string.Join(',', keys)}]): {ex.Message}", ex);
                return null;
            }

        }
    }
}
