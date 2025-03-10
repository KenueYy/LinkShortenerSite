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

public class DBService : LinkShortenerServer.DBService.DBServiceBase, IDBGrpcService, IDBService
{
    private readonly ApplicationContext _dbContext;
    private readonly IDistributedCache _cache; 

    public DBService(ApplicationContext dbContext, IDistributedCache cache)
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

    public override Task<ShortCodeResponse> Get(ShortCodeRequest request, ServerCallContext context)
    {
        var result = _dbContext.LinkTables.FirstOrDefault(x => x.LinkId == request.Code);
        return Task.FromResult(new ShortCodeResponse
        {
            Message = $"Your full link: https://{result.UserLink}",
            Link = result.UserLink
        });
    }

    public string Get(string shortCode)
    {
        StringBuilder result = new StringBuilder("https://");
        var link = _dbContext.LinkTables.FirstOrDefault(x => x.LinkId == shortCode);
            
        if (link == null)
        {
            return string.Empty;
        }
        _cache.Remove("hello1");
        _cache.SetString($"hello1", "world", new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });
        var res = _cache.GetString("hello1");
        result.Append(link.UserLink); 
        return result.ToString();
    }
}