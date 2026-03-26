using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class ProductTests
{
    [Fact(DisplayName = "Product should be created with valid data")]
    public void Given_ValidData_When_Creating_Then_ProductShouldBeCreated()
    {
        var product = ProductTestData.GenerateValidProduct();

        product.Id.Should().NotBe(Guid.Empty);
        product.ExternalId.Should().NotBeEmpty();
        product.Name.Should().NotBeEmpty();
        product.Category.Should().NotBeEmpty();
        product.StandardPrice.Should().BeGreaterThan(0);
    }

    [Fact(DisplayName = "Validation should pass for valid product")]
    public void Given_ValidProduct_When_Validated_Then_ShouldReturnValid()
    {
        var product = ProductTestData.GenerateValidProduct();

        var result = product.Validate();

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Validation should fail for invalid product data")]
    public void Given_InvalidProduct_When_Validated_Then_ShouldReturnInvalid()
    {
        var product = new Product
        {
            ExternalId = "",
            Name = "",
            Category = "",
            StandardPrice = -10m
        };

        var result = product.Validate();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact(DisplayName = "Product should not allow negative price")]
    public void Given_ProductWithNegativePrice_When_Validated_Then_ShouldFail()
    {
        var product = new Product
        {
            ExternalId = "P-001",
            Name = "Test Product",
            Category = "Test",
            StandardPrice = -100m
        };

        var result = product.Validate();

        result.IsValid.Should().BeFalse();
    }
}