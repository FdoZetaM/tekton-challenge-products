namespace TektonChallengeProducts.Application.UseCases.CreateProduct;

using MediatR;
using Domain.Enums;

public record CreateProductCommand( Status Status,
                                    int Stock,
                                    string Description,
                                    decimal Price ) : IRequest<Guid>;
