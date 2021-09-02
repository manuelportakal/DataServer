using DataServer.Domain;
using DataServer.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServer.Infrastructure.Caching
{
    public class CustomCacheWrapper<T> where T : IDataCode
    {
        private readonly IMemoryCache _cache;
        public CustomCacheWrapper(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T Read(string dataCode)
        {

            var cacheKey = $"{dataCode}";

            _cache.TryGetValue(cacheKey, out T entry);
            return entry;

        }

        public bool Write(T entry)
        {
            // Set cache options.
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                // Keep in cache for this time
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6),
                Priority = CacheItemPriority.Normal
            };

            // Save data in cache.
            var cacheKey = $"{entry.Code}";
            _cache.Set(cacheKey, entry, cacheEntryOptions);

            return true;
        }
    }
}
