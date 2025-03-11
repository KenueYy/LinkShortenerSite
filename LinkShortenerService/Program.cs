using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener;
class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddGrpc();
        builder.Services.AddControllers();
        builder.Services.AddDbContext<ApplicationContext>();
        builder.Services.AddScoped<DataManager>();
        builder.Services.AddHostedService<CacheSyncService>();
        
        
        builder.Services.AddStackExchangeRedisCache(options => {
            options.Configuration = $"{Globals.CACHE_SERVER_IP}:{Globals.REDIS_PORT}";
        });

        var app = builder.Build();
        app.MapGrpcService<DataManager>();
        app.MapControllers();
        app.MapGet("/", () => "Use a gRPC client to communicate.");
        app.Run();
    }
    
}
