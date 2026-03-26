using Ambev.DeveloperEvaluation.Application.Sales.ItemCancelled;
using DomainSale = Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using DomainCustomer = Ambev.DeveloperEvaluation.Domain.Entities.Customer;
using DomainBranch = Ambev.DeveloperEvaluation.Domain.Entities.Branch;
using DomainProduct = Ambev.DeveloperEvaluation.Domain.Entities.Product;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public static class ItemCancelledHandlerTestData
{
    public static DomainSale GenerateValidSaleWithMultipleItems()
    {
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();

        var customer = new DomainCustomer
        {
            Id = customerId,
            ExternalId = "BP-001",
            Name = "Test Customer",
            Email = "test@test.com",
            Document = "12.345.678/0001-90"
        };

        var branch = new DomainBranch
        {
            Id = branchId,
            ExternalId = "OU-SP-001",
            Name = "Test Branch",
            Location = "Test Location"
        };

        var product1 = new DomainProduct
        {
            Id = Guid.NewGuid(),
            ExternalId = "P-001",
            Name = "Product 1",
            Category = "Test",
            StandardPrice = 10.00m
        };

        var product2 = new DomainProduct
        {
            Id = Guid.NewGuid(),
            ExternalId = "P-002",
            Name = "Product 2",
            Category = "Test",
            StandardPrice = 20.00m
        };

        var product3 = new DomainProduct
        {
            Id = Guid.NewGuid(),
            ExternalId = "P-003",
            Name = "Product 3",
            Category = "Test",
            StandardPrice = 15.00m
        };

        var sale = new DomainSale
        {
            Id = Guid.NewGuid(),
            SaleNumber = $"SALE-{Guid.NewGuid().ToString().Substring(0, 8)}",
            CustomerId = customerId,
            Customer = customer,
            BranchId = branchId,
            Branch = branch,
            CreatedAt = DateTime.UtcNow
        };

        sale.AddItem(product1.Id, 5, 10.00m);
        sale.Items[0].Product = product1;

        sale.AddItem(product2.Id, 10, 20.00m);
        sale.Items[1].Product = product2;

        sale.AddItem(product3.Id, 3, 15.00m);
        sale.Items[2].Product = product3;

        return sale;
    }

    public static DomainSale GenerateValidSaleWithSingleItem()
    {
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();

        var customer = new DomainCustomer
        {
            Id = customerId,
            ExternalId = "BP-002",
            Name = "Single Item Customer",
            Email = "single@test.com",
            Document = "98.765.432/0001-21"
        };

        var branch = new DomainBranch
        {
            Id = branchId,
            ExternalId = "OU-RJ-001",
            Name = "Rio Branch",
            Location = "Rio Location"
        };

        var product = new DomainProduct
        {
            Id = Guid.NewGuid(),
            ExternalId = "P-SINGLE",
            Name = "Single Product",
            Category = "Test",
            StandardPrice = 50.00m
        };

        var sale = new DomainSale
        {
            Id = Guid.NewGuid(),
            SaleNumber = $"SALE-SINGLE-{Guid.NewGuid().ToString().Substring(0, 8)}",
            CustomerId = customerId,
            Customer = customer,
            BranchId = branchId,
            Branch = branch,
            CreatedAt = DateTime.UtcNow
        };

        sale.AddItem(product.Id, 1, 50.00m);
        sale.Items[0].Product = product;

        return sale;
    }

    public static DomainSale GenerateSaleWithSomeCancelledItems()
    {
        var sale = GenerateValidSaleWithMultipleItems();
        if (sale.Items.Count > 0)
        {
            sale.CancelItem(sale.Items[0].Id);
        }
        return sale;
    }

    public static ItemCancelledCommand GenerateValidCommand(Guid? saleId = null, Guid? itemId = null)
    {
        return new ItemCancelledCommand(
            saleId ?? Guid.NewGuid(),
            itemId ?? Guid.NewGuid());
    }

    public static ItemCancelledCommand GenerateCommandWithIds(Guid saleId, Guid itemId)
    {
        return new ItemCancelledCommand(saleId, itemId);
    }

    public static List<ItemCancelledCommand> GenerateValidCommands(int count = 3)
    {
        return Enumerable.Range(0, count)
            .Select(_ => GenerateValidCommand())
            .ToList();
    }
}