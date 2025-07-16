namespace TektonChallengeProducts.Api.Controllers;

using Api.Controllers.Common;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[ApiVersion("1.0")]
public class HealthController : TektonChallengeBaseController
{
    [HttpGet()]
    public async Task<IResult> CheckAsync()
    {
        var result = await Task.FromResult("OK");

        return Results.Ok(result);
    }
}
