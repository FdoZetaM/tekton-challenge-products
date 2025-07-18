namespace TektonChallengeProducts.Infrastructure.Services;

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Application;
using Domain.Abstractions.Services;

public class CacheService : ICacheService
{
    private static readonly TimeSpan defaultExpiration = TimeSpan.FromMinutes(5);
    private readonly IMemoryCache memoryCache;

    public CacheService(IMemoryCache memoryCache)
    {
        this.memoryCache = memoryCache;

        var statusDict = new Dictionary<Domain.Enums.Status, string>
        {
            { Domain.Enums.Status.Active, "Active" },
            { Domain.Enums.Status.Inactive, "Inactive" }
        };

        memoryCache.Set(CacheKeys.ProductStatus, statusDict);
    }

    public Task<T?> GetAsync<T>(string key)
    {
        if (memoryCache.TryGetValue(key, out T? value))
        {
            return Task.FromResult(value);
        }

        return Task.FromResult(default(T));
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        memoryCache.Set(key, value, expiration ?? defaultExpiration);

        return Task.CompletedTask;
    }
}
