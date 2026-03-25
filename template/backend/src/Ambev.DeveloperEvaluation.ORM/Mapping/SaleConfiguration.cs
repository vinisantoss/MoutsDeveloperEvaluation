using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Configuration for Sale entity mapping
/// </summary>
public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable(nameof(Sale));

        builder.HasKey(ct => ct.Id);
        builder.Property(ct => ct.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(ct => ct.SaleNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(ct => ct.SaleDate)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(ct => ct.SaleAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(ct => ct.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(ct => ct.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(ct => ct.UpdatedAt)
            .HasColumnType("timestamp with time zone");

        builder.Property(ct => ct.CustomerId)
            .IsRequired();

        builder.Property(ct => ct.BranchId)
            .IsRequired();

        builder.HasOne(ct => ct.Customer)
            .WithMany()
            .HasForeignKey(ct => ct.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ct => ct.Branch)
            .WithMany()
            .HasForeignKey(ct => ct.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(ct => ct.Items)
            .WithOne()
            .HasForeignKey(ti => ti.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ct => ct.SaleNumber)
            .IsUnique()
            .HasDatabaseName("IX_Sale_SaleNumber");

        builder.HasIndex(ct => ct.SaleDate)
            .HasDatabaseName("IX_Sale_SaleDate");

        builder.HasIndex(ct => ct.CustomerId)
            .HasDatabaseName("IX_Sale_CustomerId");

        builder.HasIndex(ct => ct.BranchId)
            .HasDatabaseName("IX_Sale_BranchId");
    }
}
