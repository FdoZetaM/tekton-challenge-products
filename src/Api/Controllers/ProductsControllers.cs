namespace TektonChallengeProducts.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Controllers.Common;
using Application.UseCases.CreateProduct;

[ApiController]
[ApiVersion("1.0")]
public class ProductsController(IMediator mediator) : TektonChallengeBaseController
{
    private readonly IMediator mediator = mediator;

    [HttpPost()]
    public async Task<IResult> CreateProductAsync([FromBody] CreateProductCommand command)
    {
        var id = await mediator.Send(command);
        return Results.Created($"/api/products/{HttpContext.GetRequestedApiVersion()}/{id}", id);
    }
}
