using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using ProjectTask.Application.Interfaces;
using StackExchange.Redis;

namespace ProjectTask.Infrastructure.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var data = await _cache.GetStringAsync(key);

        return data == null ? default : JsonSerializer.Deserialize<T>(data);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        await _cache.SetStringAsync(key, JsonSerializer.Serialize(value), options);
    }

    public Task RemoveAsync(string key)
        => _cache.RemoveAsync(key);
}