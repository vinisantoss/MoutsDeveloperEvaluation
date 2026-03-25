namespace Ambev.DeveloperEvaluation.Application.Sales.SaleCreated;

/// <summary>
/// Represents the response after creating a commercial sale
/// </summary>
public class CreateSaleResult
{
    /// <summary>
    /// Gets or sets the transaction's unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the transaction code
    /// </summary>
    public string TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the transaction date
    /// </summary>
    public DateTime TransactionDate { get; set; }

    /// <summary>
    /// Gets or sets the Customer information
    /// </summary>
    public CustomerResult Customer { get; set; } = new();

    /// <summary>
    /// Gets or sets the branch information
    /// </summary>
    public BranchResult Branch { get; set; } = new();

    /// <summary>
    /// Gets or sets the grand total amount of the transaction
    /// </summary>
    public decimal GrandTotal { get; set; }

    /// <summary>
    /// Gets or sets the transaction status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of items in the transaction
    /// </summary>
    public List<SaleItemResult> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets when the transaction was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Customer result information
/// </summary>
public class CustomerResult
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
}

/// <summary>
/// Branch result information
/// </summary>
public class BranchResult
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}

/// <summary>
/// Sale item result information
/// </summary>
public class SaleItemResult
{
    public Guid Id { get; set; }
    public ProductResult Product { get; set; } = new();
    public int Quantity { get; set; }
    public decimal ItemPrice { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal ItemTotal { get; set; }
    public bool IsCancelled { get; set; }
}

/// <summary>
/// Commercial product result information
/// </summary>
public class ProductResult
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal StandardPrice { get; set; }
}
