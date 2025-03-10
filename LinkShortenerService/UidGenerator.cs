namespace LinkShortener;

public static class UidGenerator
{
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    
    public static string Generate()
    {
        return new string(Guid.NewGuid()
            .ToByteArray()
            .Select(b => chars[b % chars.Length])
            .Take(5)
            .ToArray());
    }
}