using System;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNews.Api.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }
        
        public bool TryRead(int id, out string result)
        {
            return _cache.TryGetValue(id, out result);
        }

        public void Set(int id, string content, int expirationHours)
        {
            // Set cache saving options.
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep story in cache for 6 hours before re-fetching it.
                .SetAbsoluteExpiration(TimeSpan.FromHours(expirationHours));
            
            // Save story in cache
            _cache.Set(id, content, cacheEntryOptions);
        }
    }
}