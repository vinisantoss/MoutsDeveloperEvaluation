using Ambev.DeveloperEvaluation.Application.Sales.SaleModified;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Request model for updating an existing sale
/// </summary>
public class UpdateSaleRequest
{
    /// <summary>
    /// The unique identifier of the sale to update
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// List of item modifications (add, update, or remove)
    /// </summary>
    public List<UpdateSaleItemRequest> Items { get; set; } = new();
}

/// <summary>
/// Sale item modification request
/// </summary>
public class UpdateSaleItemRequest
{
    /// <summary>
    /// Item ID (if updating or removing existing item)
    /// </summary>
    public Guid? ItemId { get; set; }

    /// <summary>
    /// Product information (if adding new item)
    /// </summary>
    public ProductInfoRequest? Product { get; set; }

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
/// Product information for adding new items
/// </summary>
public class ProductInfoRequest
{
    /// <summary>
    /// External product identifier
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Product name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Product category
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Product standard price
    /// </summary>
    public decimal StandardPrice { get; set; }
}