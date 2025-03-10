namespace LinkShortener;

public class Globals
{
    public static bool IsProduction = false;
    
#if IsProduction
    public static string GRPC_SHORTENER_HOST = Environment.GetEnvironmentVariable("GRPC_SERVER_IP");
    public static string HTTPS_SHORTENER_HOST = Environment.GetEnvironmentVariable("HTTPS_SERVER_IP");
#else
    public static string GRPC_SHORTENER_HOST = "localhost:12122";
    public static string HTTPS_SHORTENER_HOST = "localhost:12122";
#endif

}