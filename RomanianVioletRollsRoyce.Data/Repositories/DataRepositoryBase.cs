using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace RomanianVioletRollsRoyce.Data.Repositories
{
    public abstract class DataRepositoryBase
    {
        protected readonly IMemoryCache _cache;

        public DataRepositoryBase(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        protected async Task<ICollection<T>> GetAll<T>(char prefix)
        {
            var field = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            var collection = field.GetValue(_cache) as ICollection;
            var keys = new List<string>();
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    var methodInfo = item.GetType().GetProperty("Key");
                    var val = methodInfo.GetValue(item);
                    keys.Add(val.ToString());
                }
            }

            var data = new List<T>();
            foreach (var key in keys)
            {
                if (key.StartsWith($"{prefix}-")) { data.Add(_cache.Get<T>(key)); }
            }

            return data;
        }
    }
}
