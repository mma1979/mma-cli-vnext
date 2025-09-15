namespace MmaSolution.Services.Chache
{
    public class RedisCacheService : ICacheService
    {

        private readonly IDistributedCache _cache;
        private IConfigurationSection _redisConfig;


        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
            _redisConfig = GetConfiguration();
        }

        private static IConfigurationSection GetConfiguration()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);

            var Configuration = builder.Build();

            var config = Configuration.GetSection("Redis");

            return config;
        }

        public T Get<T>(string key)
        {
            if (!_redisConfig.GetValue<bool>("UseCache"))
            {
                return default;
            }

            var value = _cache.GetString(key);

            if (value != null)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }

            return default;
        }

        public T Set<T>(string key, T value)
        {
            if (!_redisConfig.GetValue<bool>("UseCache"))
            {
                return default;
            }
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_redisConfig.GetValue<int>("ExpireInHours")),
                SlidingExpiration = TimeSpan.FromMinutes(10)
            };

            _cache.SetString(key, JsonConvert.SerializeObject(value), options);

            return value;
        }

        public T Set<T>(string key, T value, TimeSpan expiration)
        {
            if (!_redisConfig.GetValue<bool>("UseCache"))
            {
                return default;
            }
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration,
                SlidingExpiration = TimeSpan.FromMinutes(10)
            };

            _cache.SetString(key, JsonConvert.SerializeObject(value), options);

            return value;
        }

        public void Remove(string key)
        {
            if (!_redisConfig.GetValue<bool>("UseCache"))
            {
                return;
            }
            _cache.Remove(key);
        }

        public void Clear()
        {
            if (!_redisConfig.GetValue<bool>("UseCache"))
            {
                return;
            }

            var Configuration = $"{_redisConfig.GetValue<string>("Server")}:{_redisConfig.GetValue<int>("Port")}";
            var ConfigurationOptions = Redis.ConfigurationOptions.Parse(Configuration);
            ConfigurationOptions.Password = _redisConfig.GetValue<string>("Password");
            ConfigurationOptions.AllowAdmin = true;
            Redis.ConnectionMultiplexer redis = Redis.ConnectionMultiplexer.Connect(ConfigurationOptions);
            var server = redis.GetServer(Configuration);
            server.FlushDatabase();
        }

        public void Clear(string pattern)
        {
            if (!_redisConfig.GetValue<bool>("UseCache"))
            {
                return;
            }
            var Configuration = $"{_redisConfig.GetValue<string>("Server")}:{_redisConfig.GetValue<int>("Port")}";
            var ConfigurationOptions = Redis.ConfigurationOptions.Parse(Configuration);
            ConfigurationOptions.Password = _redisConfig.GetValue<string>("Password");
            ConfigurationOptions.AllowAdmin = true;
            Redis.ConnectionMultiplexer redis = Redis.ConnectionMultiplexer.Connect(ConfigurationOptions);
            var server = redis.GetServer(Configuration);
            foreach (var key in server.Keys(pattern: pattern))
            {
                redis.GetDatabase().KeyDelete(key);
            }
        }
    }

    public static class RedisCacheExtensions
    {
        public static IServiceCollection AddRedisCacheService(this IServiceCollection serviceCollection, Action<ICacheService> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        {
            var IDistributedCache = serviceCollection.BuildServiceProvider().GetRequiredService<IDistributedCache>();
            var srv = new RedisCacheService(IDistributedCache);
            if (optionsAction != null)
                optionsAction(srv);
            serviceCollection.TryAdd(
                new ServiceDescriptor(typeof(ICacheService),
                provider => srv,
                optionsLifetime));

            return serviceCollection;
        }
    }
}
