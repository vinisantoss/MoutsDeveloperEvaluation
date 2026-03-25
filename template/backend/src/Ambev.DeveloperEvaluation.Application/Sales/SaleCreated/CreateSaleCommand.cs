using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.SaleCreated;

/// <summary>
/// Command for creating a new commercial sale.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for creating a sale, 
/// including sale number, customer, branch, and items. 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="CreateSaleResult"/>.
/// 
/// The data provided in this command is validated using the 
/// <see cref="CreateSaleValidator"/> which extends 
/// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
/// populated and follow the required rules.
/// </remarks>
public class CreateSaleCommand : IRequest<CreateSaleResult>
{
    /// <summary>
    /// Gets or sets the sale number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer information
    /// </summary>
    public Customer Customer { get; set; } = new();

    /// <summary>
    /// Gets or sets the branch  information
    /// </summary>
    public Branch Branch { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of items in the sale
    /// </summary>
    public List<SaleItem> Items { get; set; } = new();

    /// <summary>
    /// Validates the command using the validator
    /// </summary>
    /// <returns>Validation result</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new CreateSaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}

/// <summary>
/// Customer information for External Identity pattern
/// </summary>
public class Customer
{
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
}

/// <summary>
/// Branch information for External Identity pattern
/// </summary>
public class Branch
{
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}

/// <summary>
/// Sale item information
/// </summary>
public class SaleItem
{
    public Product Product { get; set; } = new();
    public int Quantity { get; set; }
    public decimal ItemPrice { get; set; }
}

/// <summary>
/// Product information for External Identity pattern
/// </summary>
public class Product
{
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal StandardPrice { get; set; }
}