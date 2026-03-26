using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation.Sales;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation.Sales;

public class SaleItemValidatorTests
{
    private readonly SaleItemValidator _validator;

    public SaleItemValidatorTests()
    {
        _validator = new SaleItemValidator();
    }

    [Fact(DisplayName = "Valid sale item should pass validation")]
    public void Given_ValidSaleItem_When_Validated_Then_ShouldNotHaveErrors()
    {
        var item = SaleItemTestData.GenerateValidSaleItem();

        var result = _validator.TestValidate(item);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Sale item without product ID should fail validation")]
    public void Given_SaleItemWithoutProductId_When_Validated_Then_ShouldHaveError()
    {
        var item = SaleItemTestData.GenerateValidSaleItem();
        item.ProductId = Guid.Empty;

        var result = _validator.TestValidate(item);

        result.ShouldHaveValidationErrorFor(x => x.ProductId)
            .WithErrorMessage("Product is required");
    }

    [Fact(DisplayName = "Sale item with zero quantity should fail validation")]
    public void Given_SaleItemWithZeroQuantity_When_Validated_Then_ShouldHaveError()
    {
        var item = SaleItemTestData.GenerateValidSaleItem();
        item.Quantity = 0;

        var result = _validator.TestValidate(item);

        result.ShouldHaveValidationErrorFor(x => x.Quantity)
            .WithErrorMessage("Quantity must be greater than zero");
    }

    [Fact(DisplayName = "Sale item with quantity exceeding 20 should fail validation")]
    public void Given_SaleItemWithQuantityExceeding20_When_Validated_Then_ShouldHaveError()
    {
        var item = SaleItemTestData.GenerateValidSaleItem();
        item.Quantity = 21;

        var result = _validator.TestValidate(item);

        result.ShouldHaveValidationErrorFor(x => x.Quantity)
            .WithErrorMessage("Cannot sell more than 20 identical items");
    }

    [Fact(DisplayName = "Sale item with zero or negative price should fail validation")]
    public void Given_SaleItemWithZeroPrice_When_Validated_Then_ShouldHaveError()
    {
        var item = SaleItemTestData.GenerateValidSaleItem();
        item.ItemPrice = 0;

        var result = _validator.TestValidate(item);

        result.ShouldHaveValidationErrorFor(x => x.ItemPrice)
            .WithErrorMessage("Item price must be greater than zero");
    }

    [Fact(DisplayName = "Sale item with negative discount should fail validation")]
    public void Given_SaleItemWithNegativeDiscount_When_Validated_Then_ShouldHaveError()
    {
        var item = SaleItemTestData.GenerateValidSaleItem();
        item.DiscountPercentage = -10;

        var result = _validator.TestValidate(item);

        result.ShouldHaveValidationErrorFor(x => x.DiscountPercentage)
            .WithErrorMessage("Discount cannot be negative");
    }

    [Fact(DisplayName = "Sale item with discount exceeding 100% should fail validation")]
    public void Given_SaleItemWithDiscountExceeding100_When_Validated_Then_ShouldHaveError()
    {
        var item = SaleItemTestData.GenerateValidSaleItem();
        item.DiscountPercentage = 101;

        var result = _validator.TestValidate(item);

        result.ShouldHaveValidationErrorFor(x => x.DiscountPercentage)
            .WithErrorMessage("Discount cannot exceed 100%");
    }

    [Fact(DisplayName = "Sale item with 3 items and 0% discount should pass validation")]
    public void Given_SaleItemWith3ItemsAnd0PercentDiscount_When_Validated_Then_ShouldNotHaveErrors()
    {
        var item = SaleItem.Create(Guid.NewGuid(), 3, 100m);

        var result = _validator.TestValidate(item);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Sale item with 4 items and 10% discount should pass validation")]
    public void Given_SaleItemWith4ItemsAnd10PercentDiscount_When_Validated_Then_ShouldNotHaveErrors()
    {
        var item = SaleItem.Create(Guid.NewGuid(), 4, 100m);

        var result = _validator.TestValidate(item);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Sale item with 10 items and 20% discount should pass validation")]
    public void Given_SaleItemWith10ItemsAnd20PercentDiscount_When_Validated_Then_ShouldNotHaveErrors()
    {
        var item = SaleItem.Create(Guid.NewGuid(), 10, 100m);

        var result = _validator.TestValidate(item);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Sale item with 3 items and 10% discount should fail discount rules")]
    public void Given_SaleItemWith3ItemsAnd10PercentDiscount_When_Validated_Then_ShouldHaveError()
    {
        var item = SaleItemTestData.GenerateValidSaleItem();
        item.Quantity = 3;
        item.DiscountPercentage = 10;

        var result = _validator.TestValidate(item);

        result.ShouldHaveValidationErrorFor(x => x);
    }
}