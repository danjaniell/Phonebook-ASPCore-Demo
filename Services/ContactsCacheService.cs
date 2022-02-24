using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Phonebook.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Phonebook.Services.Base;

namespace Phonebook.Services
{
    public class ContactsCacheService : ICacheService<Contact>
    {
        private readonly IDistributedCache _distributedCache;
        private readonly DistributedCacheEntryOptions _options;
        private const string Prefix = "contacts_";

        public ContactsCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            _options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(120),
                SlidingExpiration = TimeSpan.FromSeconds(60)
            };
        }

        public async Task<Contact> Get(int id)
        {
            var key = Prefix + id;
            var cache = await _distributedCache.GetStringAsync(key);

            if (cache is null) { return null; }

            var contact = JsonConvert.DeserializeObject<Contact>(cache);
            return contact;
        }

        public async Task Set(Contact content)
        {
            var key = Prefix + content.Id;
            var contactStr = JsonConvert.SerializeObject(content);
            await _distributedCache.SetStringAsync(key, contactStr, _options);
        }
    }
}