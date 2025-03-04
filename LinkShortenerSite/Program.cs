namespace LinkShortener;

class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddDirectoryBrowser();
        var app = builder.Build();

        app.UseStaticFiles();

        app.MapControllers();

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}