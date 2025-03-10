namespace LinkShortener;

public class Globals
{
    public static string DB_SERVER_ADDRES = Environment.GetEnvironmentVariable("DB_SERVER_ADDRES");
    public static string DB_SERVER_PORT = Environment.GetEnvironmentVariable("DB_SERVER_PORT");
    public static string DB_NAME = Environment.GetEnvironmentVariable("DB_NAME");
    public static string DB_USER_NAME = Environment.GetEnvironmentVariable("DB_USER_NAME");
    public static string DB_USER_PASSWORD = Environment.GetEnvironmentVariable("DB_USER_PASSWORD");
}