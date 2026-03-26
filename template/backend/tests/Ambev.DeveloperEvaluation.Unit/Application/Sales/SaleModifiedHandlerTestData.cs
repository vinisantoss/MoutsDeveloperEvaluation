using Ambev.DeveloperEvaluation.Application.Sales.SaleModified;
using Bogus;
using AppProductInfo = Ambev.DeveloperEvaluation.Application.Sales.SaleModified.ProductInfo;
using AppSaleItem = Ambev.DeveloperEvaluation.Application.Sales.SaleModified.SaleItem;
using DomainSale = Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using DomainCustomer = Ambev.DeveloperEvaluation.Domain.Entities.Customer;
using DomainBranch = Ambev.DeveloperEvaluation.Domain.Entities.Branch;
using DomainProduct = Ambev.DeveloperEvaluation.Domain.Entities.Product;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Provides methods for generating test data for SaleModifiedHandler tests.
/// </summary>
public static class SaleModifiedHandlerTestData
{
    /// <summary>
    /// Generates a valid SaleModifiedCommand for adding a new item.
    /// </summary>
    public static SaleModifiedCommand GenerateValidAddCommand()
    {
        var faker = new Faker();
        return new SaleModifiedCommand
        {
            Id = Guid.NewGuid(),
            Items = new List<AppSaleItem>
            {
                new AppSaleItem
                {
                    Operation = UpdateOperation.Add,
                    Product = new AppProductInfo
                    {
                        ExternalId = $"P-{faker.Random.AlphaNumeric(10).ToUpper()}",
                        Name = faker.Commerce.ProductName(),
                        Category = faker.Commerce.Categories(1)[0],
                        StandardPrice = decimal.Parse(faker.Commerce.Price(2, 50))
                    },
                    Quantity = faker.Random.Number(1, 20),
                    ItemPrice = decimal.Parse(faker.Commerce.Price(2, 50))
                }
            }
        };
    }

    /// <summary>
    /// Generates a valid SaleModifiedCommand for updating an item quantity.
    /// </summary>
    public static SaleModifiedCommand GenerateValidUpdateCommand()
    {
        var faker = new Faker();
        return new SaleModifiedCommand
        {
            Id = Guid.NewGuid(),
            Items = new List<AppSaleItem>
            {
                new AppSaleItem
                {
                    ItemId = Guid.NewGuid(),
                    Operation = UpdateOperation.Update,
                    Quantity = faker.Random.Number(1, 20)
                }
            }
        };
    }

    /// <summary>
    /// Generates a valid SaleModifiedCommand for removing an item.
    /// </summary>
    public static SaleModifiedCommand GenerateValidRemoveCommand()
    {
        return new SaleModifiedCommand
        {
            Id = Guid.NewGuid(),
            Items = new List<AppSaleItem>
            {
                new AppSaleItem
                {
                    ItemId = Guid.NewGuid(),
                    Operation = UpdateOperation.Remove
                }
            }
        };
    }

    /// <summary>
    /// Generates a SaleModifiedCommand with mixed operations (add, update, remove).
    /// </summary>
    public static SaleModifiedCommand GenerateValidMixedOperationsCommand()
    {
        var faker = new Faker();
        return new SaleModifiedCommand
        {
            Id = Guid.NewGuid(),
            Items = new List<AppSaleItem>
            {
                new AppSaleItem
                {
                    Operation = UpdateOperation.Add,
                    Product = new AppProductInfo
                    {
                        ExternalId = $"P-{faker.Random.AlphaNumeric(10).ToUpper()}",
                        Name = faker.Commerce.ProductName(),
                        Category = faker.Commerce.Categories(1)[0],
                        StandardPrice = decimal.Parse(faker.Commerce.Price(2, 50))
                    },
                    Quantity = faker.Random.Number(1, 20),
                    ItemPrice = decimal.Parse(faker.Commerce.Price(2, 50))
                },
                new AppSaleItem
                {
                    ItemId = Guid.NewGuid(),
                    Operation = UpdateOperation.Update,
                    Quantity = faker.Random.Number(1, 20)
                },
                new AppSaleItem
                {
                    ItemId = Guid.NewGuid(),
                    Operation = UpdateOperation.Remove
                }
            }
        };
    }

    /// <summary>
    /// Generates an existing Sale for testing updates.
    /// </summary>
    public static DomainSale GenerateExistingSale()
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
            SaleNumber = "SALE-TEST-001",
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
}