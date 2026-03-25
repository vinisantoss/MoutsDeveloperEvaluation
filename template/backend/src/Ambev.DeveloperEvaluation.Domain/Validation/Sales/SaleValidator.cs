using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation.Sales;

/// <summary>
/// Validator for Sale entity with business rules
/// </summary>
public class SaleValidator : AbstractValidator<Sale>
{
    /// <summary>
    /// Initializes validation rules for Sale
    /// </summary>
    public SaleValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .NotEmpty()
            .WithMessage("Sale number is required")
            .MaximumLength(50)
            .WithMessage("Sale number cannot exceed 50 characters");

        RuleFor(sale => sale.BranchId)
            .NotEmpty()
            .WithMessage("Branch is required");

        RuleFor(sale => sale.CustomerId)
            .NotEmpty()
            .WithMessage("Customer is required");

        RuleFor(sale => sale.SaleDate)
            .NotEmpty()
            .WithMessage("Sale date is required")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Sale date cannot be in the future");

        RuleFor(sale => sale.Items)
            .NotEmpty()
            .WithMessage("Sale must have at least one item");

        RuleForEach(sale => sale.Items)
            .SetValidator(new SaleItemValidator());
    }
}
