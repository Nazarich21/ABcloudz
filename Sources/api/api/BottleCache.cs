using System;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api
{
    public class BottleCache : IBottleCache
    {
        private MemoryCache _cache { get; set; }
        public BottleCache()
        {
            _cache = new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = 100
            });
        }

        public Bottle Get(int id)
        {
            _cache.TryGetValue(GetCacheKey(id), out Bottle bottle);
            return bottle;
        }

        public void Remove(int id)
        {
            _cache.Remove(GetCacheKey(id));
        }

        public void Set(Bottle bottle)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSize(1);
            _cache.Set(GetCacheKey(bottle.GetId()), bottle, cacheEntryOptions);
        }

        private string GetCacheKey(int id)
        {
            return $"Bottle-{id}";
        }
    }
}
