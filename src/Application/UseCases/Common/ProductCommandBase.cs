namespace TektonChallengeProducts.Application.UseCases.Common;

using Domain.Enums;

public abstract record ProductCommandBase(
    string Name,
    Status Status,
    int Stock,
    string Description,
    decimal Price
);
