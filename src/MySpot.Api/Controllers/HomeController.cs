using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySpot.Infrastructure;

namespace MySpot.Api.Controllers;

[Route("")]
public class HomeController : ControllerBase
{
    private readonly AppOptions _appOptions;

    // more robust, but it doesn't change while app is running
    // public HomeController(IOptions<AppOptions> options) 
    
    // slower but it changes while app is running
    public HomeController(IOptionsSnapshot<AppOptions> options)
    {
        _appOptions = options.Value;
    }

    [HttpGet]
    public ActionResult<string> Get() => _appOptions.Name;
}