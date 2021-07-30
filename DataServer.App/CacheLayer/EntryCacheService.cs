using DataServer.Domain;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServer.App.CacheLayer
{
    public class EntryCacheService
    {
        private readonly IMemoryCache _cache;
        public EntryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Entry Read(string dataCode)
        {
            var cacheKey = $"entries-{dataCode}";

            if (_cache.TryGetValue(cacheKey, out Entry entry))
                return entry;
            else
                return null;
        }

        public bool Write(Entry entry)
        {
            // Set cache options.
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                // Keep in cache for this time
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6),
                Priority = CacheItemPriority.Normal
            };

            // Save data in cache.
            var cacheKey = $"entries-{entry.DataCode}";
            _cache.Set(cacheKey, entry, cacheEntryOptions);

            return true;
        }
    }
}
