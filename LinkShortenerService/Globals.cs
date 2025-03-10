namespace LinkShortener;

public class Globals
{
    public static bool IsProduction = false;
    
#if true
    public static string DB_SERVER_ADDRESS = Environment.GetEnvironmentVariable("DB_SERVER_ADDRESS");
    public static string DB_SERVER_PORT = Environment.GetEnvironmentVariable("DB_SERVER_PORT");
    public static string DB_NAME = Environment.GetEnvironmentVariable("DB_NAME");
    public static string DB_USER_NAME = Environment.GetEnvironmentVariable("DB_USER_NAME");
    public static string DB_USER_PASSWORD = Environment.GetEnvironmentVariable("DB_USER_PASSWORD");
    public static string CACHE_SERVER_IP = Environment.GetEnvironmentVariable("CACHE_SERVER_IP");
    public static string REDIS_PORT = Environment.GetEnvironmentVariable("REDIS_PORT");
#else
#endif
    
    
}