using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation.Sales;

/// <summary>
/// Validator for SaleItem entity with business rules
/// </summary>
public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
    {
        RuleFor(item => item.ProductId)
       .NotEmpty()
       .WithMessage("Product is required");

        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero")
            .LessThanOrEqualTo(20)
            .WithMessage("Cannot sell more than 20 identical items");

        RuleFor(item => item.ItemPrice)
            .GreaterThan(0)
            .WithMessage("Item price must be greater than zero");

        RuleFor(item => item.DiscountPercentage)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Discount cannot be negative")
            .LessThanOrEqualTo(100)
            .WithMessage("Discount cannot exceed 100%");

        // Business rule validation for discount
        RuleFor(item => item)
            .Must(ValidateDiscountRules)
            .WithMessage("Discount rules violation: purchases below 4 items cannot have discount, 4+ items get 10%, 10-20 items get 20%");
    }

    private bool ValidateDiscountRules(SaleItem saleItem)
    {
        return saleItem.Quantity switch
        {
            < 4 => saleItem.DiscountPercentage == 0,
            >= 4 and < 10 => saleItem.DiscountPercentage == 10,
            >= 10 and <= 20 => saleItem.DiscountPercentage == 20,
            _ => false
        };
    }
}
