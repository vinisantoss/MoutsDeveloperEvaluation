using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class CustomerTestData
{
    public static Customer GenerateValidCustomer()
    {
        var faker = new Faker();

        return new Customer
        {
            Id = Guid.NewGuid(),
            ExternalId = $"CUST-{Guid.NewGuid().ToString().Substring(0, 8)}",
            Name = faker.Person.FullName,
            Email = faker.Internet.Email(),
            Document = GenerateValidDocument()
        };
    }

    public static Customer GenerateCustomerWithInvalidEmail()
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            ExternalId = "CUST-001",
            Name = "Test Customer",
            Email = "invalid-email",
            Document = GenerateValidDocument()
        };
    }

    public static Customer GenerateCustomerWithEmptyName()
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            ExternalId = "CUST-002",
            Name = string.Empty,
            Email = "test@test.com",
            Document = GenerateValidDocument()
        };
    }

    public static Customer GenerateCustomerWithInvalidDocument()
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            ExternalId = "CUST-003",
            Name = "Test Customer",
            Email = "test@test.com",
            Document = string.Empty
        };
    }

    public static List<Customer> GenerateValidCustomers(int count = 3)
    {
        return Enumerable.Range(0, count)
            .Select(_ => GenerateValidCustomer())
            .ToList();
    }

    private static string GenerateValidDocument()
    {
        var faker = new Faker();
        return faker.Random.ReplaceNumbers("##.###.###/####-##");
    }
}