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
    
    public override Task<LinkResponse> Create(LinkRequest request, ServerCallContext context)
    {
        var linkInfo = new LinkInfo
        {
            LinkId = UidGenerator.Generate(), UserLink = request.Link
        };
            
        _dbContext.LinkTables.AddRange(linkInfo);
        _dbContext.SaveChanges();
        return Task.FromResult(new LinkResponse
        { 
            Message = $"Your Short Link Code : {linkInfo.LinkId}",
            Code = linkInfo.LinkId
        });
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
        
        await _cache.SetStringAsync(cachedCountKey, count.ToString());

        Console.WriteLine("Call DB");
        var link = Get(request.Code);

        if (count >= 3)
        {
            Console.WriteLine("Write link in cache");
            await _cache.SetStringAsync(cachedLinkKey, link);
            await _cache.RemoveAsync(cachedCountKey);
        }
        
        return new ShortCodeResponse{Message = $"Your link : {link}", Link = link};
        
    }

    public string Get(string shortCode)
    {
        StringBuilder result = new StringBuilder("https://");
        var link = _dbContext.LinkTables.FirstOrDefault(x => x.LinkId == shortCode);
            
        if (link == null)
        {
            return string.Empty;
        }
        result.Append(link.UserLink); 
        return result.ToString();
    }
}