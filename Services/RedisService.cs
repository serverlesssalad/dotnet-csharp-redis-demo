using StackExchange.Redis;
using System;
using System.Threading.Tasks;

public class RedisService
{
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _db;

    public RedisService(string connectionString)
    {
        _redis = ConnectionMultiplexer.Connect(connectionString);
        _db = _redis.GetDatabase();
    }

    public async Task SetValueAsync(string key, string value)
    {
        await _db.StringSetAsync(key, value);
    }

    public async Task<string?> GetValueAsync(string key)
    {
        return await _db.StringGetAsync(key);
    }

    public async Task<bool> DeleteValueAsync(string key)
    {
        return await _db.KeyDeleteAsync(key);
    }
}
