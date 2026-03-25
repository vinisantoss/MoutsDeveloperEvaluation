using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Extensions;

public static class ModelBuilderSeedExtensions
{
    public static void SeedDefaultData(this ModelBuilder modelBuilder)
    {
        var hashedPasswordForDefaultUser = "$2a$11$ffL71qrWZMo1NezZ4KiunO190phLmiNoGDUpYFMZi99GpcDZJGunG";

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"), // hardcoded guid
                Username = "dev_admin",
                Email = "dev.admin@ambev.com",
                Password = hashedPasswordForDefaultUser,
                Phone = "+5511987654321",
                Status = UserStatus.Active,
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow
            }
        );

        // --- Customer Seed Data ---
        var customerId1 = Guid.NewGuid();
        var customerId2 = Guid.NewGuid();

        modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                Id = customerId1,
                ExternalId = "BP-001",
                Name = "Distribuidora São Paulo",
                Email = "contato@distribuidoraspaulo.com.br",
                Document = "12.345.678/0001-90"
            },
            new Customer
            {
                Id = customerId2,
                ExternalId = "BP-002",
                Name = "Distribuidora Rio de Janeiro",
                Email = "vendas@distribuidorario.com.br",
                Document = "98.765.432/0001-21"
            }
        );

        // --- Branch Seed Data ---
        var branchId1 = Guid.NewGuid();
        var branchId2 = Guid.NewGuid();

        modelBuilder.Entity<Branch>().HasData(
            new Branch
            {
                Id = branchId1,
                ExternalId = "OU-SP-001",
                Name = "Cervejaria São Paulo",
                Location = "Avenida Paulista, 1000 - São Paulo, SP"
            },
            new Branch
            {
                Id = branchId2,
                ExternalId = "OU-RJ-001",
                Name = "Cervejaria Rio de Janeiro",
                Location = "Rua da Lapa, 50 - Rio de Janeiro, RJ"
            }
        );

        // --- Product Seed Data ---
        var productId1 = Guid.NewGuid();
        var productId2 = Guid.NewGuid();
        var productId3 = Guid.NewGuid();
        var productId4 = Guid.NewGuid();
        var productId5 = Guid.NewGuid();

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = productId1,
                ExternalId = "P-BRAHMA-001",
                Name = "Brahma Chopp 600ml",
                Category = "Cerveja",
                StandardPrice = 5.50m
            },
            new Product
            {
                Id = productId2,
                ExternalId = "P-SKOL-002",
                Name = "Skol Pilsen 350ml",
                Category = "Cerveja",
                StandardPrice = 3.00m
            },
            new Product
            {
                Id = productId3,
                ExternalId = "P-GUARANA-003",
                Name = "Guaraná Antarctica 2L",
                Category = "Refrigerante",
                StandardPrice = 7.00m
            },
            new Product
            {
                Id = productId4,
                ExternalId = "P-H2OH-004",
                Name = "H2OH! Limão 500ml",
                Category = "Refrigerante",
                StandardPrice = 4.50m
            },
            new Product
            {
                Id = productId5,
                ExternalId = "P-AMA-005",
                Name = "Água AMA 1,5L",
                Category = "Água",
                StandardPrice = 2.00m
            }
        );
    }
}
