namespace TektonChallengeProducts.Application.UseCases.GetProductById;

public record ProductResponse
{
    private const decimal percentageConverter = 100m;

    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public int Stock { get; init; }
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public decimal DiscountPercentage { get; init; }
    public decimal FinalPrice => Price * (percentageConverter - DiscountPercentage) / percentageConverter;
}
