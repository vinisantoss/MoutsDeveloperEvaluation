using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Application.Sales.Common;

/// <summary>
/// Shared customer result information across all sales operations
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
/// Shared branch result information across all sales operations
/// </summary>
public class BranchResult
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}

/// <summary>
/// Shared product result information across all sales operations
/// </summary>
public class ProductResult
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal StandardPrice { get; set; }
}

/// <summary>
/// Shared sale item result information across all sales operations
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