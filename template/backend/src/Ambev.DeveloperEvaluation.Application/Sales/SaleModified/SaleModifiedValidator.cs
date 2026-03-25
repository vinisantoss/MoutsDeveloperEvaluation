using Ambev.DeveloperEvaluation.Application.Sales.ItemCancelled;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.SaleModified;

/// <summary>
/// Validator for SaleModifiedCommand
/// </summary>
public class SaleModifiedValidator : AbstractValidator<SaleModifiedCommand>
{
    public SaleModifiedValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage("Sale ID is required");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one item operation is required");

        RuleForEach(x => x.Items)
            .SetValidator(new SaleItemModificationValidator());
    }
}

/// <summary>
/// Validator for SaleItemModification in SaleModifiedCommand
/// </summary>
public class SaleItemModificationValidator : AbstractValidator<SaleItem>
{
    public SaleItemModificationValidator()
    {
        RuleFor(x => x.Operation)
            .IsInEnum()
            .WithMessage("Valid operation is required");

        When(x => x.Operation == UpdateOperation.Remove || x.Operation == UpdateOperation.Update, () => {
            RuleFor(x => x.ItemId)
                .NotEmpty()
                .WithMessage("Item ID is required for remove/update operations")
                .NotEqual(Guid.Empty)
                .WithMessage("Item ID cannot be empty");
        });

        When(x => x.Operation == UpdateOperation.Add, () => {
            RuleFor(x => x.Product)
                .NotNull()
                .WithMessage("Product information is required for add operations")
                .SetValidator(new ProductInfoValidator()!);

            RuleFor(x => x.ItemPrice)
                .NotNull()
                .WithMessage("Item price is required for add operations")
                .GreaterThan(0)
                .WithMessage("Item price must be greater than zero");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero")
                .LessThanOrEqualTo(20)
                .WithMessage("Cannot sell more than 20 identical items");
        });

        When(x => x.Operation == UpdateOperation.Update, () => {
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero")
                .LessThanOrEqualTo(20)
                .WithMessage("Cannot sell more than 20 identical items");
        });
    }
}

/// <summary>
/// Validator for ProductInfo
/// </summary>
public class ProductInfoValidator : AbstractValidator<ProductInfo>
{
    public ProductInfoValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .WithMessage("Product external ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required")
            .MaximumLength(100)
            .WithMessage("Product name cannot exceed 100 characters");

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Product category is required");

        RuleFor(x => x.StandardPrice)
            .GreaterThan(0)
            .WithMessage("Product standard price must be greater than zero");
    }
}