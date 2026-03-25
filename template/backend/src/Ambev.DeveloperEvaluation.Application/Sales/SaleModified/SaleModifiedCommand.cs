using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.SaleModified;

/// <summary>
/// Command for modified an existing sale
/// </summary>
public class SaleModifiedCommand : IRequest<SaleModifiedResult>
{
    /// <summary>
    /// Gets or sets the sale's unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the list of items to update in the sale
    /// </summary>
    public List<SaleItem> Items { get; set; } = new();

    /// <summary>
    /// Validates the command using the validator
    /// </summary>
    /// <returns>Validation result</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleModifiedValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}

/// <summary>
/// Information for updating a sale item
/// </summary>
public class SaleItem
{
    /// <summary>
    /// Item ID (if updating existing item)
    /// </summary>
    public Guid? ItemId { get; set; }

    /// <summary>
    /// Product information (if adding new item)
    /// </summary>
    public ProductInfo? Product { get; set; }

    /// <summary>
    /// New quantity for the item
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Item price (for new items)
    /// </summary>
    public decimal? ItemPrice { get; set; }

    /// <summary>
    /// Operation type: Add, Update, or Remove
    /// </summary>
    public UpdateOperation Operation { get; set; }
}

/// <summary>
/// Product information for new items
/// </summary>
public class ProductInfo
{
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal StandardPrice { get; set; }
}

/// <summary>
/// Types of update operations
/// </summary>
public enum UpdateOperation
{
    Add,
    Update,
    Remove
}
