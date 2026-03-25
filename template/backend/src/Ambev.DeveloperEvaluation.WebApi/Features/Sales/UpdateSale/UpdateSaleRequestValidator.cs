using FluentValidation;
using Ambev.DeveloperEvaluation.Application.Sales.SaleModified;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleRequest
/// </summary>
public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    public UpdateSaleRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required")
            .NotEqual(Guid.Empty)
            .WithMessage("Sale ID cannot be empty");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one item operation is required");

        RuleForEach(x => x.Items)
            .SetValidator(new UpdateSaleItemValidator());
    }
}

/// <summary>
/// Validator for UpdateSaleItemRequest
/// </summary>
public class UpdateSaleItemValidator : AbstractValidator<UpdateSaleItemRequest>
{
    public UpdateSaleItemValidator()
    {
        RuleFor(x => x.Operation)
            .IsInEnum()
            .WithMessage("Valid operation is required");

        // Para Remove/Update: precisa do ItemId
        When(x => x.Operation == UpdateOperation.Remove || x.Operation == UpdateOperation.Update, () => {
            RuleFor(x => x.ItemId)
                .NotEmpty()
                .WithMessage("Item ID is required for remove/update operations")
                .NotEqual(Guid.Empty)
                .WithMessage("Item ID cannot be empty");
        });

        // Para Add: precisa de Product info e Price
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

        // Para Update: precisa de Quantity
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
/// Validator for ProductInfoRequest
/// </summary>
public class ProductInfoValidator : AbstractValidator<ProductInfoRequest>
{
    public ProductInfoValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .WithMessage("Product external ID is required")
            .MaximumLength(50)
            .WithMessage("Product external ID must not exceed 50 characters");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required")
            .MaximumLength(100)
            .WithMessage("Product name must not exceed 100 characters");

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Product category is required")
            .MaximumLength(100)
            .WithMessage("Product category must not exceed 100 characters");

        RuleFor(x => x.StandardPrice)
            .GreaterThan(0)
            .WithMessage("Product standard price must be greater than zero");
    }
}