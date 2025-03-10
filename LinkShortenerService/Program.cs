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
        builder.Services.AddScoped<DBService>();
        
        
        builder.Services.AddStackExchangeRedisCache(options => {
            options.Configuration = $"{Globals.SERVER_IP}:{Globals.REDIS_PORT}";
        });

        builder.Services.AddHostedService<CacheSyncService>();
        
        var app = builder.Build();

        app.MapGrpcService<DBService>();

        app.MapControllers();

        app.MapGet("/", () => "Use a gRPC client to communicate.");
        app.Run();
    }
    
}
