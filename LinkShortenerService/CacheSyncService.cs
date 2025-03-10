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
            await SyncCacheToDatabase();
            await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
        }
    }

    private async Task SyncCacheToDatabase()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    }
}