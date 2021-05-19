using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace TenderWin
{
    public class MemoryCache<TItem>
    {
        private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public async Task<TItem> GetOrCreate(object key, Func<Task<TItem>> createItem)
        {
            TItem cacheEntry;
            if (!_cache.TryGetValue(key, out cacheEntry))
            {
                cacheEntry = await createItem();
                _cache.Set(key, cacheEntry);
            }
            return cacheEntry;
        }
    }
}
