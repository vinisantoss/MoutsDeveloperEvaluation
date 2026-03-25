using FluentValidation;


namespace Ambev.DeveloperEvaluation.Application.Sales.SaleCreated;

/// <summary>
/// Validator for CreateSaleCommand
/// </summary>
public class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleValidator()
    {
        RuleFor(x => x.SaleNumber)
            .NotEmpty()
            .WithMessage("Sale code is required")
            .MaximumLength(50)
            .WithMessage("Sale code cannot exceed 50 characters");

        RuleFor(x => x.Customer)
            .NotNull()
            .WithMessage("Customer information is required")
            .SetValidator(new CustomerValidator());

        RuleFor(x => x.Branch)
            .NotNull()
            .WithMessage("Branch information is required")
            .SetValidator(new BranchValidator());

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Sale must have at least one item");

        RuleForEach(x => x.Items)
            .SetValidator(new SaleItemValidator());
    }
}

/// <summary>
/// Validator for Customer
/// </summary>
public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .WithMessage("Customer external ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Customer name is required")
            .MaximumLength(100)
            .WithMessage("Customer name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Customer email is required")
            .EmailAddress()
            .WithMessage("Customer email must be valid");

        RuleFor(x => x.Document)
            .NotEmpty()
            .WithMessage("Customer document is required");
    }
}

/// <summary>
/// Validator for Branch
/// </summary>
public class BranchValidator : AbstractValidator<Branch>
{
    public BranchValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .WithMessage("Branch external ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Branch name is required")
            .MaximumLength(100)
            .WithMessage("Branch name cannot exceed 100 characters");

        RuleFor(x => x.Location)
            .NotEmpty()
            .WithMessage("Branch location is required");
    }
}

/// <summary>
/// Validator for SaleItem
/// </summary>
public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero")
            .LessThanOrEqualTo(20)
            .WithMessage("Cannot sell more than 20 identical items");

        RuleFor(x => x.ItemPrice)
            .GreaterThan(0)
            .WithMessage("Item price must be greater than zero");

        RuleFor(x => x.Product)
            .NotNull()
            .WithMessage("Product information is required")
            .SetValidator(new ProductValidator());
    }
}

/// <summary>
/// Validator for Product
/// </summary>
public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
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