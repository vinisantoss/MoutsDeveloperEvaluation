using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class BranchTestData
{
    public static Branch GenerateValidBranch()
    {
        var faker = new Faker();

        return new Branch
        {
            Id = Guid.NewGuid(),
            ExternalId = $"BRANCH-{Guid.NewGuid().ToString().Substring(0, 8)}",
            Name = faker.Company.CompanyName(),
            Location = faker.Address.City()
        };
    }

    public static Branch GenerateBranchWithEmptyName()
    {
        return new Branch
        {
            Id = Guid.NewGuid(),
            ExternalId = "BRANCH-001",
            Name = string.Empty,
            Location = "Test Location"
        };
    }

    public static Branch GenerateBranchWithEmptyLocation()
    {
        return new Branch
        {
            Id = Guid.NewGuid(),
            ExternalId = "BRANCH-002",
            Name = "Test Branch",
            Location = string.Empty
        };
    }

    public static Branch GenerateBranchWithEmptyExternalId()
    {
        return new Branch
        {
            Id = Guid.NewGuid(),
            ExternalId = string.Empty,
            Name = "Test Branch",
            Location = "Test Location"
        };
    }

    public static List<Branch> GenerateValidBranches(int count = 3)
    {
        return Enumerable.Range(0, count)
            .Select(_ => GenerateValidBranch())
            .ToList();
    }
}