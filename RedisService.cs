using StackExchange.Redis;

public class RedisService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<bool> SetAsync(string key, string value,int time)
    {
        var db = _connectionMultiplexer.GetDatabase();
        return await db.StringSetAsync(key, value, TimeSpan.FromSeconds(time));
    }

}
