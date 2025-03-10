using Microsoft.EntityFrameworkCore;

namespace LinkShortener;

public class ApplicationContext : DbContext
{
    public DbSet<LinkInfo> LinkTables { get; set; } = null!;
 
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql($"Host={Globals.DB_SERVER_ADDRES};" +
                                     $"Port={Globals.DB_SERVER_PORT};" +
                                     $"Database={Globals.DB_NAME};" +
                                     $"Username={Globals.DB_USER_NAME};" +
                                     $"Password={Globals.DB_USER_PASSWORD}");
        }
    }
}