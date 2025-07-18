namespace TektonChallengeProducts.Application.UseCases.GetProductById;

using MediatR;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductResponse?>;
