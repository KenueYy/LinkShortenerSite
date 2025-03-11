using System.Text;
using Grpc.Core;
using LinkShortenerServer;
using Microsoft.Extensions.Caching.Distributed;

namespace LinkShortener;

public interface IDBGrpcService
{
    public Task<LinkResponse> Create(LinkRequest request, ServerCallContext context);
    public Task<ShortCodeResponse> Get(ShortCodeRequest request, ServerCallContext context);
}

public interface IDBService
{
    public string Get(string shortCode);
}

public class DataManager : LinkShortenerServer.DataManager.DataManagerBase, IDBGrpcService, IDBService
{
    private readonly ApplicationContext _dbContext;
    private readonly IDistributedCache _cache; 

    public DataManager(ApplicationContext dbContext, IDistributedCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }
    
    public override async Task<LinkResponse> Create(LinkRequest request, ServerCallContext context)
    {
        var linkInfo = new LinkInfo
        {
            LinkId = UidGenerator.Generate(), UserLink = Encoding.UTF8.GetString(Convert.FromBase64String(request.Link))
        };
            
        await SetInCache(linkInfo.LinkId, linkInfo.UserLink, TimeSpan.FromMinutes(15));

        Task.Run(async () =>
        {
            _dbContext.LinkTables.AddRange(linkInfo);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        });
        
        return new LinkResponse
        { 
            Message = $"Your Short Link Code : {linkInfo.LinkId}",
            Code = linkInfo.LinkId
        };
    }

    public override async Task<ShortCodeResponse> Get(ShortCodeRequest request, ServerCallContext context)
    {
        var cachedCountKey = $"shortlink_count:{request.Code}";
        var cachedLinkKey = $"shortlink:{request.Code}";
        var cachedLink = await _cache.GetStringAsync(cachedLinkKey);
        
        if (!string.IsNullOrEmpty(cachedLink))
        {
            Console.WriteLine("Link from cache");
            return new ShortCodeResponse{Message = $"Your link : {cachedLink}",Link = cachedLink};
        }

        var cachedCountValue = await _cache.GetStringAsync(cachedCountKey);
        var count = string.IsNullOrEmpty(cachedCountValue) ? 0 : int.Parse(cachedCountValue);
        count++;
        
        await SetInCache(cachedCountKey, count.ToString(), TimeSpan.FromHours(3));

        Console.WriteLine("Call DB");
        var link = Get(request.Code);
        link = Convert.ToBase64String(Encoding.UTF8.GetBytes(link));
        
        if (count >= 3)
        {
            Console.WriteLine("Write link in cache");
            await _cache.RemoveAsync(cachedCountKey);
        }
        
        return new ShortCodeResponse{Message = $"Your link : {link}", Link = link};
        
    }

    public string Get(string shortCode)
    {
        var link = _dbContext.LinkTables.FirstOrDefault(x => x.LinkId == shortCode);
            
        if (link == null || string.IsNullOrEmpty(link.UserLink))
        {
            return string.Empty;
        }

        return link.UserLink;
    }
    
    private async Task SetInCache(string key, string value, TimeSpan lifeTime)
    {
        await _cache.SetStringAsync(key, value, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = lifeTime
        });
    }
}