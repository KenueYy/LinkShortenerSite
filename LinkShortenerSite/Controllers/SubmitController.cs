using Grpc.Net.Client;
using LinkShortenerClient;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener;


[Route("api/[controller]")]
[ApiController]
public class SubmitController : ControllerBase
{
    public static string HOST => "https://localhost:7065";
    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] UserInput input)
    {
        Console.WriteLine($"Вы отправили: {input.Input}");

        var request = input.Input.Replace($"https:/", "");
        using var channel = GrpcChannel.ForAddress("http://localhost:5213");
        var client = new DBService.DBServiceClient(channel);

        var response = await client.CreateAsync(new LinkRequest { Link = request });

        return Ok(new { message = $"{HOST}/{response.Code}" });
    }
}

public class UserInput
{
    public string Input { get; set; }
}
