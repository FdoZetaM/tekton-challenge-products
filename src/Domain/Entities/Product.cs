namespace TektonChallengeProducts.Domain.Entities;

using Domain.Enums;
using Domain.Entities.Base;

public class Product : Entity<Guid>
{
    public Product( Status status,
                    int stock,
                    string description,
                    decimal price,
                    decimal discountPercentage ) : base(Guid.NewGuid())
    {
        this.Status = status;
        this.Stock = stock;
        this.Description = description;
        this.Price = price;
        this.DiscountPercentage = discountPercentage;
    }

    public Status Status { get; private set; }

    public int Stock { get; private set; }

    public string Description { get; private set; }

    public decimal Price { get; private set; }

    public decimal DiscountPercentage  { get; private set; }
}
