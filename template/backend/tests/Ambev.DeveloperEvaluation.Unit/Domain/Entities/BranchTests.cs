using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class BranchTests
{
    [Fact(DisplayName = "Branch should be created with valid data")]
    public void Given_ValidData_When_Creating_Then_BranchShouldBeCreated()
    {
        var branch = BranchTestData.GenerateValidBranch();

        branch.Id.Should().NotBe(Guid.Empty);
        branch.ExternalId.Should().NotBeEmpty();
        branch.Name.Should().NotBeEmpty();
        branch.Location.Should().NotBeEmpty();
    }

    [Fact(DisplayName = "Validation should pass for valid branch")]
    public void Given_ValidBranch_When_Validated_Then_ShouldReturnValid()
    {
        var branch = BranchTestData.GenerateValidBranch();

        var result = branch.Validate();

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Validation should fail for invalid branch data")]
    public void Given_InvalidBranch_When_Validated_Then_ShouldReturnInvalid()
    {
        var branch = new Branch
        {
            ExternalId = "",
            Name = "",
            Location = ""
        };

        var result = branch.Validate();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
}