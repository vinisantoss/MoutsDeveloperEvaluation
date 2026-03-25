using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleRequest
/// </summary>
public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    public CreateSaleRequestValidator()
    {
        RuleFor(x => x.SaleNumber)
            .NotEmpty()
            .WithMessage("Sale number is required")
            .MaximumLength(50)
            .WithMessage("Sale number must not exceed 50 characters");

        RuleFor(x => x.Customer)
            .NotNull()
            .WithMessage("Customer information is required")
            .SetValidator(new CustomerValidator()!);

        RuleFor(x => x.Branch)
            .NotNull()
            .WithMessage("Branch information is required")
            .SetValidator(new BranchValidator()!);

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one item is required");

        RuleForEach(x => x.Items)
            .SetValidator(new CreateSaleItemValidator());
    }
}

/// <summary>
/// Validator for CustomerRequest
/// </summary>
public class CustomerValidator : AbstractValidator<CustomerRequest>
{
    public CustomerValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .WithMessage("Customer external ID is required")
            .MaximumLength(50)
            .WithMessage("Customer external ID must not exceed 50 characters");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Customer name is required")
            .MaximumLength(200)
            .WithMessage("Customer name must not exceed 200 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Customer email is required")
            .EmailAddress()
            .WithMessage("Customer email must be valid");

        RuleFor(x => x.Document)
            .NotEmpty()
            .WithMessage("Customer document is required")
            .MaximumLength(20)
            .WithMessage("Customer document must not exceed 20 characters");
    }
}

/// <summary>
/// Validator for BranchRequest
/// </summary>
public class BranchValidator : AbstractValidator<BranchRequest>
{
    public BranchValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .WithMessage("Branch external ID is required")
            .MaximumLength(50)
            .WithMessage("Branch external ID must not exceed 50 characters");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Branch name is required")
            .MaximumLength(200)
            .WithMessage("Branch name must not exceed 200 characters");

        RuleFor(x => x.Location)
            .NotEmpty()
            .WithMessage("Branch location is required")
            .MaximumLength(200)
            .WithMessage("Branch location must not exceed 200 characters");
    }
}

/// <summary>
/// Validator for CreateSaleItemRequest
/// </summary>
public class CreateSaleItemValidator : AbstractValidator<CreateSaleItemRequest>
{
    public CreateSaleItemValidator()
    {
        RuleFor(x => x.Product)
            .NotNull()
            .WithMessage("Product information is required")
            .SetValidator(new ProductValidator()!);

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero")
            .LessThanOrEqualTo(20)
            .WithMessage("Cannot sell more than 20 identical items");

        RuleFor(x => x.ItemPrice)
            .GreaterThan(0)
            .WithMessage("Item price must be greater than zero");
    }
}

/// <summary>
/// Validator for ProductRequest
/// </summary>
public class ProductValidator : AbstractValidator<ProductRequest>
{
    public ProductValidator()
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