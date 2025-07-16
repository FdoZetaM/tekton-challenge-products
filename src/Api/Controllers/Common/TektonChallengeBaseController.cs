namespace TektonChallengeProducts.Api.Controllers.Common;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class TektonChallengeBaseController : ControllerBase
{
}