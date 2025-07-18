namespace TektonChallengeProducts.Application.UseCases.CreateProduct;

using MediatR;
using Domain.Enums;
using Application.UseCases.Common;

public record CreateProductCommand(string Name,
                                    Status Status,
                                    int Stock,
                                    string Description,
                                    decimal Price
) : ProductCommandBase(Name, Status, Stock, Description, Price), IRequest<Guid>;
