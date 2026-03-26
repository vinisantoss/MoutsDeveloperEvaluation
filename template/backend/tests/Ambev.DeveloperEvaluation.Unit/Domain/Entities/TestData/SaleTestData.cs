using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class SaleTestData
{
    public static Sale GenerateValidSale()
    {
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();

        var customer = new Customer
        {
            Id = customerId,
            ExternalId = $"CUST-{Guid.NewGuid().ToString().Substring(0, 8)}",
            Name = "Test Customer",
            Email = "customer@test.com",
            Document = "12.345.678/0001-90"
        };

        var branch = new Branch
        {
            Id = branchId,
            ExternalId = $"BRANCH-{Guid.NewGuid().ToString().Substring(0, 8)}",
            Name = "Test Branch",
            Location = "Test Location"
        };

        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = $"SALE-{Guid.NewGuid().ToString().Substring(0, 8)}",
            SaleDate = DateTime.UtcNow,
            CustomerId = customerId,
            Customer = customer,
            BranchId = branchId,
            Branch = branch,
            Status = SaleStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        return sale;
    }

    public static Sale GenerateValidSaleWithItems()
    {
        var sale = GenerateValidSale();

        var product1 = new Product
        {
            Id = Guid.NewGuid(),
            ExternalId = "P-001",
            Name = "Product 1",
            Category = "Test",
            StandardPrice = 100m
        };

        var product2 = new Product
        {
            Id = Guid.NewGuid(),
            ExternalId = "P-002",
            Name = "Product 2",
            Category = "Test",
            StandardPrice = 50m
        };

        sale.AddItem(product1.Id, 5, 100m);
        sale.Items[0].Product = product1;

        sale.AddItem(product2.Id, 3, 50m);
        sale.Items[1].Product = product2;

        return sale;
    }

    public static Sale GenerateSaleWithMultipleItems()
    {
        var sale = GenerateValidSale();

        var product1 = new Product
        {
            Id = Guid.NewGuid(),
            ExternalId = "P-001",
            Name = "Product 1",
            Category = "Test",
            StandardPrice = 100m
        };

        var product2 = new Product
        {
            Id = Guid.NewGuid(),
            ExternalId = "P-002",
            Name = "Product 2",
            Category = "Test",
            StandardPrice = 200m
        };

        var product3 = new Product
        {
            Id = Guid.NewGuid(),
            ExternalId = "P-003",
            Name = "Product 3",
            Category = "Test",
            StandardPrice = 75m
        };

        sale.AddItem(product1.Id, 4, 100m);
        sale.Items[0].Product = product1;

        sale.AddItem(product2.Id, 10, 200m);
        sale.Items[1].Product = product2;

        sale.AddItem(product3.Id, 15, 75m);
        sale.Items[2].Product = product3;

        return sale;
    }

    public static Sale GenerateCancelledSale()
    {
        var sale = GenerateValidSale();
        sale.Cancel();

        return sale;
    }
}