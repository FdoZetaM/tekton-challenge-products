namespace TektonChallengeProducts.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Controllers.Common;
using Application.UseCases.CreateProduct;
using Application.UseCases.GetProductById;

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

    [HttpGet("{id}")]
    public async Task<IResult> GetProductByIdAsync([FromRoute] string id)
    {
        if (!Guid.TryParse(id, out var guid))
        {
            return Results.BadRequest("Id inválido");
        }

        var product = await mediator.Send(new GetProductByIdQuery(guid));

        if (product is null)
            return Results.NotFound();

        return Results.Ok(product);
    }
}
