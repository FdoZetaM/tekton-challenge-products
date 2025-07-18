namespace TektonChallengeProducts.Application.UseCases.GetProductById;

public record ProductResponse
{
    private const decimal percentageConverter = 100m;

    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public int Stock { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal FinalPrice => Price * (percentageConverter - DiscountPercentage) / percentageConverter;
}
