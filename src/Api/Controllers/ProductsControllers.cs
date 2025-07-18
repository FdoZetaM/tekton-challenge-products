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

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="command">Product data to create.</param>
    /// <returns>Id of the created product.</returns>
    [HttpPost()]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> CreateProductAsync([FromBody] CreateProductCommand command)
    {
        var id = await mediator.Send(command);
        return Results.Created($"/api/products/{HttpContext.GetRequestedApiVersion()}/{id}", id);
    }

    /// <summary>
    /// Gets product details by Id.
    /// </summary>
    /// <param name="id">Product Id.</param>
    /// <returns>Product details.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="id">Product Id.</param>
    /// <param name="command">Updated product data.</param>
    /// <returns>No content if the update was successful.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
