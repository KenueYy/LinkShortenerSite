using Grpc.Net.Client;
using LinkShortenerClient;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener;

[ApiController, Route("{shortCode}")]
public class RedirectController : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetRedirect(string shortCode)
    {
        Console.WriteLine($"Link shortCode: {shortCode}");
        var link = await GetLink(shortCode);
        if (string.IsNullOrEmpty(link))
        {
            Console.WriteLine($"Link shortCode is empty");
            return Results.StatusCode(404);
        }

        return Results.Redirect(link);
    }

    public async Task<string> GetLink(string shortCode)
    {
        using var channel = GrpcChannel.ForAddress(Globals.GRPC_SHORTENER_HOST);
        var client = new DataManager.DataManagerClient(channel);

        var response = await client.GetAsync(new ShortCodeRequest{ Code = shortCode });

        return response.Link;
    }
}