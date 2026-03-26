using Ambev.DeveloperEvaluation.Application.Sales.SaleCreated;
using Bogus;
using AppCustomer = Ambev.DeveloperEvaluation.Application.Sales.SaleCreated.Customer;
using AppBranch = Ambev.DeveloperEvaluation.Application.Sales.SaleCreated.Branch;
using AppProduct = Ambev.DeveloperEvaluation.Application.Sales.SaleCreated.Product;
using AppSaleItem = Ambev.DeveloperEvaluation.Application.Sales.SaleCreated.SaleItem;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public static class CreateSaleHandlerTestData
{
    private static readonly Faker _faker = new();

    private static readonly Faker<AppCustomer> customerFaker = new Faker<AppCustomer>()
        .RuleFor(c => c.ExternalId, f => $"BP-{f.Random.Number(1, 999):000}")
        .RuleFor(c => c.Name, f => f.Company.CompanyName())
        .RuleFor(c => c.Email, f => f.Internet.Email())
        .RuleFor(c => c.Document, f => $"{f.Random.Number(10, 99)}.{f.Random.Number(100, 999)}.{f.Random.Number(100, 999)}/0001-{f.Random.Number(10, 99)}");

    private static readonly Faker<AppBranch> branchFaker = new Faker<AppBranch>()
        .RuleFor(b => b.ExternalId, f => $"OU-{f.Address.StateAbbr()}-{f.Random.Number(1, 999):000}")
        .RuleFor(b => b.Name, f => f.Company.CompanyName())
        .RuleFor(b => b.Location, f => f.Address.City());

    private static readonly Faker<AppProduct> productFaker = new Faker<AppProduct>()
        .RuleFor(p => p.ExternalId, f => $"P-{f.Random.AlphaNumeric(10).ToUpper()}")
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(p => p.StandardPrice, f => decimal.Parse(f.Commerce.Price(10, 1000)));

    private static AppSaleItem GenerateSaleItemWithUniqueProduct(int quantity)
    {
        return new AppSaleItem
        {
            Product = productFaker.Generate(),
            Quantity = quantity,
            ItemPrice = decimal.Parse(_faker.Commerce.Price(10, 500))
        };
    }

    private static readonly Faker<CreateSaleCommand> createSaleCommandFaker = new Faker<CreateSaleCommand>()
        .RuleFor(s => s.SaleNumber, f => $"SALE-{f.Date.Recent().Year}-{f.Random.Number(1000, 9999)}")
        .RuleFor(s => s.Customer, f => customerFaker.Generate())
        .RuleFor(s => s.Branch, f => branchFaker.Generate())
        .RuleFor(s => s.Items, f => new List<AppSaleItem>
        {
            GenerateSaleItemWithUniqueProduct(f.Random.Number(1, 10)),
            GenerateSaleItemWithUniqueProduct(f.Random.Number(1, 10))
        });

    public static CreateSaleCommand GenerateValidCommand()
    {
        return createSaleCommandFaker.Generate();
    }

    public static List<CreateSaleCommand> GenerateValidCommands(int count = 3)
    {
        return createSaleCommandFaker.Generate(count);
    }

    public static CreateSaleCommand GenerateCommandWithSaleNumber(string saleNumber)
    {
        return createSaleCommandFaker
            .RuleFor(s => s.SaleNumber, saleNumber)
            .Generate();
    }

    public static CreateSaleCommand GenerateCommandWithMinimumItems()
    {
        var command = createSaleCommandFaker.Generate();
        command.Items = new List<AppSaleItem>
        {
            GenerateSaleItemWithUniqueProduct(2)
        };
        return command;
    }

    public static CreateSaleCommand GenerateCommandWithMaximumQuantity()
    {
        var command = createSaleCommandFaker.Generate();
        command.Items = new List<AppSaleItem>
        {
            new AppSaleItem
            {
                Product = new AppProduct
                {
                    ExternalId = $"P-MAX-{_faker.Random.AlphaNumeric(5).ToUpper()}",
                    Name = "Test Product Max",
                    Category = "Test",
                    StandardPrice = 10.00m
                },
                Quantity = 20,
                ItemPrice = 10.00m
            }
        };
        return command;
    }

    public static CreateSaleCommand GenerateCommandWithInvalidQuantity()
    {
        var command = createSaleCommandFaker.Generate();
        command.Items = new List<AppSaleItem>
        {
            new AppSaleItem
            {
                Product = productFaker.Generate(),
                Quantity = 21,
                ItemPrice = 100m
            }
        };
        return command;
    }

    public static CreateSaleCommand GenerateCommandWithEmptyItems()
    {
        var command = createSaleCommandFaker.Generate();
        command.Items = new List<AppSaleItem>();
        return command;
    }

    public static CreateSaleCommand GenerateCommandWithInvalidCustomer()
    {
        var command = createSaleCommandFaker.Generate();
        command.Customer = new AppCustomer
        {
            ExternalId = string.Empty,
            Name = string.Empty,
            Email = "invalid-email",
            Document = string.Empty
        };
        return command;
    }

    public static CreateSaleCommand GenerateCommandWithInvalidBranch()
    {
        var command = createSaleCommandFaker.Generate();
        command.Branch = new AppBranch
        {
            ExternalId = string.Empty,
            Name = string.Empty,
            Location = string.Empty
        };
        return command;
    }
}