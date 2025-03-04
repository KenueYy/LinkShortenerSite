namespace LinkShortener;

public class Globals
{
    public static string GRPC_SHORTENER_HOST = Environment.GetEnvironmentVariable("GRPC_SERVER_IP");
    public static string HTTPS_SHORTENER_HOST = Environment.GetEnvironmentVariable("HTTPS_SERVER_IP");
}