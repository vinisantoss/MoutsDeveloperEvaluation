using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation.Sales;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation.Sales;

public class ProductValidatorTests
{
    private readonly ProductValidator _validator;

    public ProductValidatorTests()
    {
        _validator = new ProductValidator();
    }

    [Fact(DisplayName = "Valid product should pass validation")]
    public void Given_ValidProduct_When_Validated_Then_ShouldNotHaveErrors()
    {
        var product = ProductTestData.GenerateValidProduct();

        var result = _validator.TestValidate(product);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Product without external ID should fail validation")]
    public void Given_ProductWithoutExternalId_When_Validated_Then_ShouldHaveError()
    {
        var product = ProductTestData.GenerateValidProduct();
        product.ExternalId = string.Empty;

        var result = _validator.TestValidate(product);

        result.ShouldHaveValidationErrorFor(x => x.ExternalId)
            .WithErrorMessage("External ID is required");
    }

    [Fact(DisplayName = "Product with external ID exceeding 50 characters should fail validation")]
    public void Given_ProductWithExternalIdExceeding50Characters_When_Validated_Then_ShouldHaveError()
    {
        var product = ProductTestData.GenerateValidProduct();
        product.ExternalId = new string('A', 51);

        var result = _validator.TestValidate(product);

        result.ShouldHaveValidationErrorFor(x => x.ExternalId)
            .WithErrorMessage("External ID must not exceed 50 characters");
    }

    [Fact(DisplayName = "Product without name should fail validation")]
    public void Given_ProductWithoutName_When_Validated_Then_ShouldHaveError()
    {
        var product = ProductTestData.GenerateValidProduct();
        product.Name = string.Empty;

        var result = _validator.TestValidate(product);

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name is required");
    }

    [Fact(DisplayName = "Product with name exceeding 100 characters should fail validation")]
    public void Given_ProductWithNameExceeding100Characters_When_Validated_Then_ShouldHaveError()
    {
        var product = ProductTestData.GenerateValidProduct();
        product.Name = new string('A', 101);

        var result = _validator.TestValidate(product);

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name must not exceed 100 characters");
    }

    [Fact(DisplayName = "Product without category should fail validation")]
    public void Given_ProductWithoutCategory_When_Validated_Then_ShouldHaveError()
    {
        var product = ProductTestData.GenerateValidProduct();
        product.Category = string.Empty;

        var result = _validator.TestValidate(product);

        result.ShouldHaveValidationErrorFor(x => x.Category)
            .WithErrorMessage("Category is required");
    }

    [Fact(DisplayName = "Product with category exceeding 50 characters should fail validation")]
    public void Given_ProductWithCategoryExceeding50Characters_When_Validated_Then_ShouldHaveError()
    {
        var product = ProductTestData.GenerateValidProduct();
        product.Category = new string('A', 51);

        var result = _validator.TestValidate(product);

        result.ShouldHaveValidationErrorFor(x => x.Category)
            .WithErrorMessage("Category must not exceed 50 characters");
    }

    [Fact(DisplayName = "Product with zero or negative price should fail validation")]
    public void Given_ProductWithZeroPrice_When_Validated_Then_ShouldHaveError()
    {
        var product = ProductTestData.GenerateValidProduct();
        product.StandardPrice = 0;

        var result = _validator.TestValidate(product);

        result.ShouldHaveValidationErrorFor(x => x.StandardPrice)
            .WithErrorMessage("Standard price must be greater than zero");
    }

    [Fact(DisplayName = "Product with price exceeding maximum should fail validation")]
    public void Given_ProductWithPriceExceedingMaximum_When_Validated_Then_ShouldHaveError()
    {
        var product = ProductTestData.GenerateValidProduct();
        product.StandardPrice = 1000000m;

        var result = _validator.TestValidate(product);

        result.ShouldHaveValidationErrorFor(x => x.StandardPrice)
            .WithErrorMessage("Standard price must not exceed 999,999.99");
    }
}