namespace URLShortener.Data;

using StackExchange.Redis;
using System;

public sealed class ShortUrlRepository
{
    private readonly ConnectionMultiplexer redisConnection;
    private readonly IDatabase redisDatabase;

    public ShortUrlRepository(ConnectionMultiplexer redisConnection)
    {
        this.redisConnection = redisConnection;
        this.redisDatabase = redisConnection.GetDatabase();
    }

    public async Task Create(ShortUrl shortUrl)
    {
        if (await Exists(shortUrl.Path))
            throw new Exception($"Shortened URL with path '{shortUrl.Path}' already exists.");

        var urlWasSet = await redisDatabase.StringSetAsync(shortUrl.Path, shortUrl.Destination);
        if (!urlWasSet)
        {
            throw new Exception("Failed to create shortened URL");
        }
    }

    public async Task Update(ShortUrl shortUrl)
    {
        if (await Exists(shortUrl.Path) == false)
            throw new Exception($"Shortened URL with Path '{shortUrl.Path}' does not exist.");

        var urlWasSet = await redisDatabase.StringSetAsync(shortUrl.Path, shortUrl.Destination);
        if (!urlWasSet)
            throw new Exception($"Failed to update shortened URL.");
    }

    public async Task Delete(string path)
    {

    }
}