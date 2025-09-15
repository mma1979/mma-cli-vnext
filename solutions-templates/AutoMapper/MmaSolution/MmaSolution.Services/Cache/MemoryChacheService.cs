namespace MmaSolution.Services.Chache
{
    public class MemoryChacheService : ICacheService
    {

        private readonly IMemoryCache _cache;

        public MemoryChacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T Get<T>(string key)
        {
            var value = _cache.Get<T>(key);

            if (value == null)
            {
                return default;
            }

            return value;
        }

        public T Set<T>(string key, T value)
        {

            _cache.Set(key, value);

            return value;
        }

        public T Set<T>(string key, T value, TimeSpan expiration)
        {

            _cache.Set(key, value);

            return value;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void Clear()
        {
            if (_cache != null && _cache is MemoryCache memCache)
            {
                memCache.Compact(1.0);
            }
        }

        public void Clear(string pattern)
        {
            throw new NotImplementedException();
        }
    }

    public static class MemoryChacheExtensions
    {
        public static IServiceCollection AddMemoryChacheService(this IServiceCollection serviceCollection, Action<ICacheService> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        {
            var IMemoryCache = serviceCollection.BuildServiceProvider().GetRequiredService<IMemoryCache>();
            var srv = new MemoryChacheService(IMemoryCache);
            if (optionsAction != null)
                optionsAction(new MemoryChacheService(IMemoryCache));

            serviceCollection.TryAdd(
                new ServiceDescriptor(typeof(ICacheService),
                provider => srv,
                optionsLifetime));

            return serviceCollection;
        }
    }
}
