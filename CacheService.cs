using System;
using System.Linq;
using System.Collections.Generic;

namespace WykoBot
{
    public class CacheService
    {
        const int maxCacheSize = 100;
        private readonly Queue<int> queue;
        private readonly Dictionary<int, CacheItem> cache;

        public CacheService()
        {
            cache = new Dictionary<int, CacheItem>();
            queue = new Queue<int>();
        }
        internal List<CacheItem> CreateNewNotications(List<FeedItem> items)
        {
            var newItems = items.Where(r => !cache.ContainsKey(r.HashCode)).ToList();
            var result = new List<CacheItem>();
            foreach (var newItem in newItems)
            {
                queue.Enqueue(newItem.HashCode);
                var item = new CacheItem(newItem, string.Empty);
                result.Add(item);
                cache.Add(newItem.HashCode, item);
            }
            while (queue.Count > maxCacheSize)
            {
                var key = queue.Dequeue();
                cache.Remove(key);
            }
            return result;
        }
    }


}