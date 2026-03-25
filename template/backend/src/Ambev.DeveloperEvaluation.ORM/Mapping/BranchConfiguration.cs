using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Configuration for Branch entity mapping
/// </summary>
public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable(nameof(Branch));

        builder.HasKey(ou => ou.Id);
        builder.Property(ou => ou.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(ou => ou.ExternalId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(ou => ou.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ou => ou.Location)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(ou => ou.ExternalId)
            .IsUnique()
            .HasDatabaseName("IX_Branch_ExternalId");
    }
}
