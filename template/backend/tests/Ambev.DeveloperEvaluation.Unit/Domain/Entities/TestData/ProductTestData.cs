using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class ProductTestData
{
    public static Product GenerateValidProduct()
    {
        var faker = new Faker();

        return new Product
        {
            Id = Guid.NewGuid(),
            ExternalId = $"PROD-{Guid.NewGuid().ToString().Substring(0, 8)}",
            Name = faker.Commerce.ProductName(),
            Category = faker.Commerce.Categories(1)[0],
            StandardPrice = decimal.Parse(faker.Commerce.Price(10, 1000))
        };
    }

    public static Product GenerateProductWithNegativePrice()
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            ExternalId = "PROD-001",
            Name = "Test Product",
            Category = "Test",
            StandardPrice = -100m
        };
    }

    public static Product GenerateProductWithZeroPrice()
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            ExternalId = "PROD-002",
            Name = "Test Product",
            Category = "Test",
            StandardPrice = 0m
        };
    }

    public static Product GenerateProductWithEmptyName()
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            ExternalId = "PROD-003",
            Name = string.Empty,
            Category = "Test",
            StandardPrice = 100m
        };
    }

    public static Product GenerateProductWithEmptyCategory()
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            ExternalId = "PROD-004",
            Name = "Test Product",
            Category = string.Empty,
            StandardPrice = 100m
        };
    }

    public static Product GenerateProductWithEmptyExternalId()
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            ExternalId = string.Empty,
            Name = "Test Product",
            Category = "Test",
            StandardPrice = 100m
        };
    }

    public static List<Product> GenerateValidProducts(int count = 3)
    {
        return Enumerable.Range(0, count)
            .Select(_ => GenerateValidProduct())
            .ToList();
    }
}