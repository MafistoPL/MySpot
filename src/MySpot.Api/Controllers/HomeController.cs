using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MySpot.Api.Controllers;

[Route("")]
public class HomeController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public HomeController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public ActionResult<string> Get() => _configuration["app:name"];
}