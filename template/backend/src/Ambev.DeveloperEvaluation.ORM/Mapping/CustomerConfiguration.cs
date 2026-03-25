using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Configuration for Customer entity mapping
/// </summary>
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable(nameof(Customer));

        builder.HasKey(bp => bp.Id);
        builder.Property(bp => bp.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(bp => bp.ExternalId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(bp => bp.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(bp => bp.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(bp => bp.Document)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(bp => bp.ExternalId)
            .IsUnique()
            .HasDatabaseName("IX_Customer_ExternalId");
    }
}
