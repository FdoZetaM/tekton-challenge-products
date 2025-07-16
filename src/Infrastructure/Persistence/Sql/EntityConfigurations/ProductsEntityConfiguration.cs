namespace TektonChallengeProducts.Infrastructure.Persistence.Sql.EntityConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

public class ProductsEntityConfiguration : IEntityTypeConfiguration<Product>
{
    private const string tableName = "Products";

    private byte columnOrder = 0;

    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(tableName);

        builder.HasKey(p => p.Id)
               .HasName("PK_Products");
        builder.Property(p => p.Id)
               .HasColumnOrder(columnOrder++);

        builder.Property(p => p.Status)
               .HasColumnOrder(columnOrder++)
               .IsRequired();

        builder.Property(p => p.Stock)
               .HasColumnOrder(columnOrder++)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.Description)
               .HasColumnOrder(columnOrder++)
               .HasMaxLength(500);

        builder.Property(p => p.Price)
               .HasColumnOrder(columnOrder++)
               .IsRequired()
               .HasColumnType("decimal(18,2)");
        
        builder.Property(p => p.DiscountPercentage)
               .HasColumnOrder(columnOrder++)
               .IsRequired()
               .HasColumnType("decimal(4,3)");
    }
}
