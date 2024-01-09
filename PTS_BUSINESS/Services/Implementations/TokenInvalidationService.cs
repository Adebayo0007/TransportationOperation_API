/*using Microsoft.Extensions.Caching.Distributed;
using PTS_BUSINESS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_BUSINESS.Services.Implementations
{
    public class TokenInvalidationService : ITokenInvalidationService
    {
        private readonly IDistributedCache _distributedCache;

        public TokenInvalidationService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task InvalidateTokenAsync(string token)
        {
            await _distributedCache.SetStringAsync(token, "invalid", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(8) // Set the same expiration as the token
            });
        }

        public async Task ValidateTokenAsync(string token)
        {
            // Remove the token from the distributed cache to mark it as valid
            await _distributedCache.RemoveAsync(token);
        }

        public async Task<string> IsTokenInvalidAsync(string token)
        {
            var cachedValue = await _distributedCache.GetStringAsync(token);
            return cachedValue ?? null;
        }

    }
}
*/