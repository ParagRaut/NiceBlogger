using Microsoft.Extensions.Caching.Distributed;

namespace NiceBlogger.Infrastructure.Extensions;

public static class CacheOptions
{
    //Lower expiration time for demo purposes. Not to use such lower value in production scenario.
    public static DistributedCacheEntryOptions DefaultExpiration =>
        new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10) };
}
