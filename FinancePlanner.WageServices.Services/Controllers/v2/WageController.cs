using System.Net;
using FinancePlanner.WageServices.Services.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FinancePlanner.WageServices.Services.Controllers.v2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ValidateModelFilter]
public class WageController : ControllerBase
{
    [MapToApiVersion("2.0")]
    [HttpGet("Test")]
    [ProducesResponseType(typeof(ActionResult), (int)HttpStatusCode.OK)]
    public IActionResult Index()
    {
        return Ok(new { Status = "V2 Test Passed" });
    }
}