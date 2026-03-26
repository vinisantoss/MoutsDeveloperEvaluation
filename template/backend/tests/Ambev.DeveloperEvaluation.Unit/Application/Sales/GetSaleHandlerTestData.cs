using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Bogus;
using DomainSale = Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using DomainCustomer = Ambev.DeveloperEvaluation.Domain.Entities.Customer;
using DomainBranch = Ambev.DeveloperEvaluation.Domain.Entities.Branch;
using DomainProduct = Ambev.DeveloperEvaluation.Domain.Entities.Product;
using DomainSaleItem = Ambev.DeveloperEvaluation.Domain.Entities.SaleItem;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Provides methods for generating test data for GetSaleHandler tests.
/// </summary>
public static class GetSaleHandlerTestData
{
    /// <summary>
    /// Generates a valid Sale entity with all related data.
    /// </summary>
    public static DomainSale GenerateValidSale()
    {
        var faker = new Faker();
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var customer = new DomainCustomer
        {
            Id = customerId,
            ExternalId = $"BP-{faker.Random.Number(1, 999):000}",
            Name = faker.Company.CompanyName(),
            Email = faker.Internet.Email(),
            Document = $"{faker.Random.Number(10, 99)}.{faker.Random.Number(100, 999)}.{faker.Random.Number(100, 999)}/0001-{faker.Random.Number(10, 99)}"
        };

        var branch = new DomainBranch
        {
            Id = branchId,
            ExternalId = $"OU-{faker.Address.StateAbbr()}-{faker.Random.Number(1, 999):000}",
            Name = faker.Company.CompanyName(),
            Location = faker.Address.FullAddress()
        };

        var product = new DomainProduct
        {
            Id = productId,
            ExternalId = $"P-{faker.Random.AlphaNumeric(10).ToUpper()}",
            Name = faker.Commerce.ProductName(),
            Category = faker.Commerce.Categories(1)[0],
            StandardPrice = decimal.Parse(faker.Commerce.Price(2, 50))
        };

        var sale = new DomainSale
        {
            Id = Guid.NewGuid(),
            SaleNumber = $"SALE-{faker.Date.Recent().Year}-{faker.Random.Number(1000, 9999)}",
            SaleDate = DateTime.UtcNow,
            CustomerId = customerId,
            Customer = customer,
            BranchId = branchId,
            Branch = branch,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        sale.AddItem(productId, 10, 5.50m);
        sale.Items[0].Product = product;

        return sale;
    }

    /// <summary>
    /// Generates a Sale with multiple items.
    /// </summary>
    public static DomainSale GenerateSaleWithMultipleItems()
    {
        var sale = GenerateValidSale();
        var faker = new Faker();

        var product2 = new DomainProduct
        {
            Id = Guid.NewGuid(),
            ExternalId = $"P-{faker.Random.AlphaNumeric(10).ToUpper()}",
            Name = faker.Commerce.ProductName(),
            Category = faker.Commerce.Categories(1)[0],
            StandardPrice = decimal.Parse(faker.Commerce.Price(2, 50))
        };

        sale.AddItem(product2.Id, 5, 7.00m);
        sale.Items[1].Product = product2;

        return sale;
    }

    /// <summary>
    /// Generates a cancelled Sale.
    /// </summary>
    public static DomainSale GenerateCancelledSale()
    {
        var sale = GenerateValidSale();
        sale.Cancel();
        return sale;
    }

    /// <summary>
    /// Generates a Sale with some cancelled items.
    /// </summary>
    public static DomainSale GenerateSaleWithCancelledItems()
    {
        var sale = GenerateSaleWithMultipleItems();
        sale.CancelItem(sale.Items[0].Id);
        return sale;
    }
}