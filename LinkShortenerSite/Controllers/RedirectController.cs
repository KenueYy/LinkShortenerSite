using Grpc.Net.Client;
using LinkShortenerClient;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener;

[ApiController, Route("redirect/{shortCode}")]
public class RedirectController : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetRedirect(string shortCode)
    {
        var link = await GetLink(shortCode);
        if (string.IsNullOrEmpty(link))
        {
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