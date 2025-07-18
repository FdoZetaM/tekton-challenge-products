namespace TektonChallengeProducts.Application.UseCases.UpdateProduct;

using MediatR;
using Application.UseCases.Common;
using Domain.Enums;

public record UpdateProductCommand( Guid Id,
                                    string Name,
                                    Status Status,
                                    int Stock,
                                    string Description,
                                    decimal Price
) : ProductCommandBase(Name, Status, Stock, Description, Price), IRequest<Guid>;
