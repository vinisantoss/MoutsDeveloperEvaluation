using Ambev.DeveloperEvaluation.Application.Sales.Common;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Represents the response after retrieving a sale
/// </summary>
public class GetSaleResult
{
    /// <summary>
    /// Gets or sets the sale's unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the sale number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sale date
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the customer information
    /// </summary>
    public CustomerResult Customer { get; set; } = new();

    /// <summary>
    /// Gets or sets the branch information
    /// </summary>
    public BranchResult Branch { get; set; } = new();

    /// <summary>
    /// Gets or sets the grand total amount of the sale
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the sale status
    /// </summary>
    public string SaleStatus { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of items in the sale
    /// </summary>
    public List<SaleItemResult> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets when the sale was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets when the sale was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}