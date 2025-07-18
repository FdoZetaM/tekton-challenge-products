namespace TektonChallengeProducts.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Controllers.Common;
using Application.Resources;
using Application.UseCases.CreateProduct;
using Application.UseCases.GetProductById;
using Application.UseCases.UpdateProduct;

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
            return Results.BadRequest(ValidationMessagesResources.InvalidOrMissingId);
        }

        var product = await mediator.Send(new GetProductByIdQuery(guid));

        if (product is null)
            return Results.NotFound();

        return Results.Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IResult> UpdateProductAsync([FromRoute] string id,
                                                  [FromBody] UpdateProductCommand command)
    {
        if (!Guid.TryParse(id, out var guid) || guid != command.Id)
        {
            return Results.BadRequest(ValidationMessagesResources.InvalidOrMissingId);
        }

        await mediator.Send(command);

        return Results.NoContent();
    }
}
