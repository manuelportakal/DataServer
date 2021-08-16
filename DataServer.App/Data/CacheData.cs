using DataServer.Domain;
using DataServer.Infrastructure.Caching;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServer.App.Data
{
    public class CacheData
    {
        public static CustomCacheWrapper<Agent> _agentCacheService;
        public static CustomCacheWrapper<Entry> _entryCacheService;

        public static void Load(IMemoryCache memoryCache)
        {
            if (_agentCacheService == null)
            {
                _agentCacheService = new CustomCacheWrapper<Agent>(memoryCache);
            }

            if (_entryCacheService == null)
            {
                _entryCacheService = new CustomCacheWrapper<Entry>(memoryCache);
            }

        }
    }


}
