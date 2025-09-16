namespace MmaSolution.Services.Settings
{
    public class LocalizationSerivce
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<LocalizationSerivce> _logger;
        private readonly ICacheService _cacheService;

        public LocalizationSerivce(IServiceScopeFactory scopeFactory, ILogger<LocalizationSerivce> logger, ICacheService cacheService)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _cacheService = cacheService;
        }

        public string Translate(string key, string language = LanguageCode.Arabic)
        {
            try
            {
                var cacheKey = $"{key}_{language}";
                var cached = _cacheService.Get<string>(cacheKey);
                if(!string.IsNullOrEmpty(cached)) return cached;

                using var scope = _scopeFactory.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var value = context.Resources
                    .Include(e=>e.Language)
                    .FirstOrDefault(e => e.Key == key && e.Language.LanguageCode == language)?.Value;

                if (string.IsNullOrEmpty(value))
                {
                    _logger.LogInformation($"{DateTime.UtcNow} - {nameof(Translate)}({key}): Key nnot found");
                    return key;
                }
                _cacheService.Set(cacheKey, value); 
                return value;

            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(Translate)}({key}): {ex.Message}", ex);
                return key;
            }
        }

        public async Task<string> TranslateAsync(string key, string language = LanguageCode.Arabic)
        {
            try
            {
                var cacheKey = $"{key}_{language}";
                var cached = _cacheService.Get<string>(cacheKey);
                if (!string.IsNullOrEmpty(cached)) return cached;

                using var scope = _scopeFactory.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var value = (await context.Resources
                    .Include(e=>e.Language)
                    .FirstOrDefaultAsync(e => e.Key == key && e.Language.LanguageCode == language))?.Value;

                if (string.IsNullOrEmpty(value))
                {
                    _logger.LogInformation($"{DateTime.UtcNow} - {nameof(Translate)}({key}): Key nnot found");
                    return key;
                }
                _cacheService.Set(cacheKey, value);
                return value;

            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(Translate)}({key}): {ex.Message}", ex);
                return key;
            }
        }

        public async Task<string[]> TranslateAsync(string[] keys, string language = LanguageCode.Arabic)
{
try
            {
                var cacheKey = $"{string.Join('_', keys)}_{language}_arr";
                var cached = _cacheService.Get<string[]>(cacheKey);
                if (cached != default) return cached; 
                
                using var scope = _scopeFactory.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var tasks = keys.Select(async k=> (await context.Resources
                .Include(e=>e.Language)
                .FirstOrDefaultAsync(e=>e.Key==k && e.Language.LanguageCode == language)) ?? new Core.Database.Localization.Resource { Key=k, Value=k});
               
                var values = (await Task.WhenAll(tasks)).Select(x=>x.Value).ToArray();

                if (!values.Any())
                {
                    _logger.LogInformation($"{DateTime.UtcNow} - {nameof(Translate)}([{string.Join(',', keys)}]): Keys not found");
                    return keys;
}
                _cacheService.Set(cacheKey, values);
                return values;

            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} -{nameof(Translate)}([{string.Join(',', keys)}]): {ex.Message}", ex);
                return keys;
            }
        }

        public async Task<Dictionary<string, string>> TranslateDictAsync(string[] keys, string language = LanguageCode.Arabic)
        {
            try
            {
                var cacheKey = $"{string.Join('_', keys)}_{language}_dict";
                var cached = _cacheService.Get<Dictionary<string, string>>(cacheKey);
                if (cached != default) return cached;

                using var scope = _scopeFactory.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var tasks = keys.Select(async k => (await context.Resources
                .Include(e=>e.Language)
                .FirstOrDefaultAsync(e => e.Key == k && e.Language.LanguageCode == language)) ?? new Core.Database.Localization.Resource { Key = k, Value = k });

                var dict = (await Task.WhenAll(tasks)).ToDictionary(e=>e.Key,e=>e.Value);
                if (dict.Count <= 0)
                {
                    _logger.LogInformation($"{DateTime.UtcNow} - {nameof(Translate)}([{string.Join(',', keys)}]): Keys not found");
                    return keys.ToDictionary(e => e, e => e);
                }
                _cacheService.Set(cacheKey, dict);
                return dict;

            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} -{nameof(Translate)}([{string.Join(',', keys)}]): {ex.Message}", ex);
                return keys.ToDictionary(e => e, e => e);
            }
        }
    }
}
