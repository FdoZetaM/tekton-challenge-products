namespace TektonChallengeProducts.Domain.Entities;

using Domain.Enums;
using Domain.Entities.Base;

public class Product : Entity<Guid>
{
    public Product(Status status,
                    int stock,
                    string description,
                    decimal price) : base(Guid.NewGuid())
    {
        this.Status = status;
        this.Stock = stock;
        this.Description = description;
        this.Price = price;
    }

    public Status Status { get; private set; }

    public int Stock { get; private set; }

    public string Description { get; private set; }

    public decimal Price { get; private set; }

    public byte DiscountPercentage { get; private set; }

    public void SetDiscountPercentage(byte discountPercentage)
    {
        if (discountPercentage < 0 || discountPercentage > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(discountPercentage), "Discount percentage must be between 0 and 100.");
        }

        this.DiscountPercentage = discountPercentage;
    }
}
