using Ambev.DeveloperEvaluation.Application.Sales.SaleCancelled;
using Bogus;
using DomainBranch = Ambev.DeveloperEvaluation.Domain.Entities.Branch;
using DomainCustomer = Ambev.DeveloperEvaluation.Domain.Entities.Customer;
using DomainProduct = Ambev.DeveloperEvaluation.Domain.Entities.Product;
using DomainSale = Ambev.DeveloperEvaluation.Domain.Entities.Sale;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Provides methods for generating test data for SaleCancelledHandler tests.
/// </summary>
public static class SaleCancelledHandlerTestData
{
    /// <summary>
    /// Generates a valid Sale entity ready to be cancelled.
    /// </summary>
    public static DomainSale GenerateValidSaleForCancellation()
    {
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var productId = Guid.NewGuid();

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

        var product = new DomainProduct
        {
            Id = productId,
            ExternalId = "P-TEST-001",
            Name = "Test Product",
            Category = "Test",
            StandardPrice = 10.00m
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

        sale.AddItem(productId, 5, 10.00m);
        sale.Items[0].Product = product;

        return sale;
    }

    /// <summary>
    /// Generates a Sale with multiple items ready for cancellation.
    /// </summary>
    public static DomainSale GenerateSaleWithMultipleItemsForCancellation()
    {
        var sale = GenerateValidSaleForCancellation();
        var faker = new Faker();

        var product2 = new DomainProduct
        {
            Id = Guid.NewGuid(),
            ExternalId = $"P-{faker.Random.AlphaNumeric(10).ToUpper()}",
            Name = faker.Commerce.ProductName(),
            Category = faker.Commerce.Categories(1)[0],
            StandardPrice = decimal.Parse(faker.Commerce.Price(2, 50))
        };

        sale.AddItem(product2.Id, 10, 15.00m);
        sale.Items[1].Product = product2;

        return sale;
    }

    /// <summary>
    /// Generates a Sale that is already cancelled.
    /// </summary>
    public static DomainSale GenerateAlreadyCancelledSale()
    {
        var sale = GenerateValidSaleForCancellation();
        sale.Cancel();
        return sale;
    }

    /// <summary>
    /// Generates a Sale with partially cancelled items.
    /// </summary>
    public static DomainSale GenerateSaleWithPartiallyCancelledItems()
    {
        var sale = GenerateSaleWithMultipleItemsForCancellation();
        sale.CancelItem(sale.Items[0].Id);
        return sale;
    }

    /// <summary>
    /// Generates a valid SaleCancelledCommand.
    /// </summary>
    public static SaleCancelledCommand GenerateValidCommand(Guid? saleId = null)
    {
        return new SaleCancelledCommand(saleId ?? Guid.NewGuid());
    }

    /// <summary>
    /// Generates multiple valid SaleCancelledCommand entities.
    /// </summary>
    public static List<SaleCancelledCommand> GenerateValidCommands(int count = 3)
    {
        return Enumerable.Range(0, count)
            .Select(_ => GenerateValidCommand())
            .ToList();
    }
}