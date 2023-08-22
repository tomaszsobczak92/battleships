using Guestline.Games.Battleships.Server.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace Guestline.Games.Battleships.Server.Infrastructures
{
    public class MemoryCacheWrapper : IMemoryCacheWrapper
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheWrapper(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public void Add(Guid key, object value, TimeSpan? expiration = null)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions();

            if (expiration.HasValue)
            {
                cacheEntryOptions.AbsoluteExpirationRelativeToNow = expiration;
            }

            _cache.Set(key, value, cacheEntryOptions);
        }

        public T Get<T>(Guid key) where T : class
        {
            _cache.TryGetValue(key, out object? value);

            if (value == null)
            {
                throw new Exception($"Cached value is null for key {key}");
            }

            return (T)value;
        }
    }
}
