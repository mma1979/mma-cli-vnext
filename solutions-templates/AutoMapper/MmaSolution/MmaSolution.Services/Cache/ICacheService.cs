namespace MmaSolution.Services.Chache
{
    public interface ICacheService
    {
        T Get<T>(string key);
        T Set<T>(string key, T value);
        T Set<T>(string key, T value, TimeSpan expiration);
        void Remove(string key);
        void Clear();
        void Clear(string pattern);
    }

}
