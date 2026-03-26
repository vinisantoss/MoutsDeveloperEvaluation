using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation.Sales;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation.Sales;

public class SaleValidatorTests
{
    private readonly SaleValidator _validator;

    public SaleValidatorTests()
    {
        _validator = new SaleValidator();
    }

    [Fact(DisplayName = "Valid sale should pass validation")]
    public void Given_ValidSale_When_Validated_Then_ShouldNotHaveErrors()
    {
        var sale = SaleTestData.GenerateValidSale();
        sale.SaleDate = DateTime.Now;
        sale.Items = new List<SaleItem>
        {
            new SaleItem
            {
               Id = Guid.NewGuid(),
               Quantity = 1,
               ItemPrice = 1,
               ProductId = Guid.NewGuid(),
               Product = new Product
               {
                   Id = Guid.NewGuid(),
               }
            }
        };

        var result = _validator.TestValidate(sale);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Sale without sale number should fail validation")]
    public void Given_SaleWithoutSaleNumber_When_Validated_Then_ShouldHaveError()
    {
        var sale = SaleTestData.GenerateValidSale();
        sale.SaleNumber = string.Empty;

        var result = _validator.TestValidate(sale);

        result.ShouldHaveValidationErrorFor(x => x.SaleNumber)
            .WithErrorMessage("Sale number is required");
    }

    [Fact(DisplayName = "Sale with sale number exceeding 50 characters should fail validation")]
    public void Given_SaleNumberExceeding50Characters_When_Validated_Then_ShouldHaveError()
    {
        var sale = SaleTestData.GenerateValidSale();
        sale.SaleNumber = new string('A', 51);

        var result = _validator.TestValidate(sale);

        result.ShouldHaveValidationErrorFor(x => x.SaleNumber)
            .WithErrorMessage("Sale number cannot exceed 50 characters");
    }

    [Fact(DisplayName = "Sale without branch ID should fail validation")]
    public void Given_SaleWithoutBranchId_When_Validated_Then_ShouldHaveError()
    {
        var sale = SaleTestData.GenerateValidSale();
        sale.BranchId = Guid.Empty;

        var result = _validator.TestValidate(sale);

        result.ShouldHaveValidationErrorFor(x => x.BranchId)
            .WithErrorMessage("Branch is required");
    }

    [Fact(DisplayName = "Sale without customer ID should fail validation")]
    public void Given_SaleWithoutCustomerId_When_Validated_Then_ShouldHaveError()
    {
        var sale = SaleTestData.GenerateValidSale();
        sale.CustomerId = Guid.Empty;

        var result = _validator.TestValidate(sale);

        result.ShouldHaveValidationErrorFor(x => x.CustomerId)
            .WithErrorMessage("Customer is required");
    }

    [Fact(DisplayName = "Sale with future sale date should fail validation")]
    public void Given_SaleWithFutureSaleDate_When_Validated_Then_ShouldHaveError()
    {
        var sale = SaleTestData.GenerateValidSale();
        sale.SaleDate = DateTime.UtcNow.AddDays(1);

        var result = _validator.TestValidate(sale);

        result.ShouldHaveValidationErrorFor(x => x.SaleDate)
            .WithErrorMessage("Sale date cannot be in the future");
    }
}