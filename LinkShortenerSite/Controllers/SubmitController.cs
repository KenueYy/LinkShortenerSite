using System.Text;
using Grpc.Net.Client;
using LinkShortenerClient;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener;


[Route("api/[controller]")]
[ApiController]
public class SubmitController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] UserInput input)
    {
        Console.WriteLine($"Вы отправили: {input.Input}");      
        Console.WriteLine($"Address:{Globals.GRPC_SHORTENER_HOST}");
        var request = Convert.ToBase64String(Encoding.UTF8.GetBytes(input.Input));
        using var channel = GrpcChannel.ForAddress(Globals.GRPC_SHORTENER_HOST);
        var client = new DataManager.DataManagerClient(channel);

        var response = await client.CreateAsync(new LinkRequest { Link = request });

        return Ok(new { message = $"{Globals.HTTPS_SHORTENER_HOST}/{response.Code}" });
    }
}

public class UserInput
{
    public string Input { get; set; }
}
