using System.Net;
using FinancePlanner.Shared.Models.WageServices;
using FinancePlanner.WageServices.Services.Filters;
using FinancePlanner.WageServices.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancePlanner.WageServices.Services.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize("ApiScope")]
[ValidateModelFilter]
public class WageController : ControllerBase
{
    private readonly IPreTaxService _preTaxService;
    private readonly IPostTaxService _postTaxService;

    public WageController(IPreTaxService preTaxService, IPostTaxService postTaxService)
    {
        _preTaxService = preTaxService;
        _postTaxService = postTaxService;
    }

    [AllowAnonymous]
    [MapToApiVersion("1.0")]
    [HttpGet("Test")]
    [ProducesResponseType(typeof(ActionResult), (int)HttpStatusCode.OK)]
    public IActionResult Index()
    {
        return Ok(new { Status = "V1 Test Passed" });
    }

    [MapToApiVersion("1.0")]
    [HttpPost("CalculateTotalTaxableWages")]
    [ProducesResponseType(typeof(ActionResult<PreTaxDeductionResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<PreTaxDeductionResponse> CalculateTotalTaxableWages([FromBody] PreTaxDeductionRequest request)
    {
        PreTaxDeductionResponse response = _preTaxService.CalculateTaxableWages(request);
        return Ok(response);
    }

    [MapToApiVersion("1.0")]
    [HttpPost("CalculatePostTaxDeductions")]
    [ProducesResponseType(typeof(ActionResult<PostTaxDeductionResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<PostTaxDeductionResponse> CalculatePostTaxDeductions([FromBody] PostTaxDeductionRequest request)
    {
        PostTaxDeductionResponse response = _postTaxService.CalculatePostTaxDeductions(request);
        return Ok(response);
    }
}