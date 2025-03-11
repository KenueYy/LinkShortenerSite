using Microsoft.Extensions.Caching.Distributed;

namespace LinkShortener;

public class CacheSyncService : BackgroundService
{
    private readonly IDistributedCache _cache;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CacheSyncService(IServiceScopeFactory serviceScopeFactory, IDistributedCache cache)
    {
        _cache = cache;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {

            await Task.Delay(1000, cancellationToken);
        }
    }

    public async Task SyncCacheToDatabase(LinkInfo linkInfo)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        
        dbContext.LinkTables.AddRange(linkInfo);
        await dbContext.SaveChangesAsync();
    }
}