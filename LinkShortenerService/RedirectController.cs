using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace LinkShortener;

[ApiController, Route("{shortCode}")]
public class RedirectController : ControllerBase
{
    private readonly DataManager _dataManager;

    public RedirectController(DataManager dataManager) 
    {
        _dataManager = dataManager;
    }
    
    [HttpGet]
    public async Task<IResult> Get(string shortCode)
    {
        var link = await _dataManager.GetLink(shortCode);
        if (!string.IsNullOrEmpty(link))
        {
            return Results.Redirect(link);
        }
        return Results.StatusCode(404);
    }
}