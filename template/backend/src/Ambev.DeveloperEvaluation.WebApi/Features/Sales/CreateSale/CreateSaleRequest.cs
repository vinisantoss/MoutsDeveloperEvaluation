namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Request model for creating a new sale
/// </summary>
public class CreateSaleRequest
{
    /// <summary>
    /// The sale number (unique identifier for the sale)
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Customer information
    /// </summary>
    public CustomerRequest Customer { get; set; } = new();

    /// <summary>
    /// Branch information
    /// </summary>
    public BranchRequest Branch { get; set; } = new();

    /// <summary>
    /// List of items in the sale
    /// </summary>
    public List<CreateSaleItemRequest> Items { get; set; } = new();
}

/// <summary>
/// Customer information for creating a sale
/// </summary>
public class CustomerRequest
{
    /// <summary>
    /// External customer identifier
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Customer name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Customer email
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Customer document (CPF/CNPJ)
    /// </summary>
    public string Document { get; set; } = string.Empty;
}

/// <summary>
/// Branch information for creating a sale
/// </summary>
public class BranchRequest
{
    /// <summary>
    /// External branch identifier
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Branch name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Branch location
    /// </summary>
    public string Location { get; set; } = string.Empty;
}

/// <summary>
/// Sale item information for creating a sale
/// </summary>
public class CreateSaleItemRequest
{
    /// <summary>
    /// Product information
    /// </summary>
    public ProductRequest Product { get; set; } = new();

    /// <summary>
    /// Quantity of the product
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Item price at the time of sale
    /// </summary>
    public decimal ItemPrice { get; set; }
}

/// <summary>
/// Product information for creating a sale item
/// </summary>
public class ProductRequest
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