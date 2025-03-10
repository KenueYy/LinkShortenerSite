using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace LinkShortener;

public class DataManager
{
    private readonly IDBService _dbService;
    private readonly IDistributedCache _cache;

    public DataManager(DBService dbService, IDistributedCache cache)
    {
        _cache = cache;
        _dbService = dbService;
    }

    public async Task<string> GetLink(string shortCode)
    {
        var cachedCountKey = $"shortlink_count:{shortCode}";
        var cachedLinkKey = $"shortlink:{shortCode}";
        var cachedLink = await _cache.GetStringAsync(cachedLinkKey);
        
        if (!string.IsNullOrEmpty(cachedLink))
        {
            Console.WriteLine("Link from cache");
            return cachedLink;
        }

        var cachedCountValue = await _cache.GetStringAsync(cachedCountKey);
        var count = string.IsNullOrEmpty(cachedCountValue) ? 0 : int.Parse(cachedCountValue);
        count++;
        
        await _cache.SetStringAsync(cachedCountKey, count.ToString());

        Console.WriteLine("Call DB");
        var link = _dbService.Get(shortCode);

        if (count >= 3)
        {
            Console.WriteLine("Write link in cache");
            await _cache.SetStringAsync(cachedLinkKey, link);
            await _cache.RemoveAsync(cachedCountKey);
        }
        
        return link;
    }
}