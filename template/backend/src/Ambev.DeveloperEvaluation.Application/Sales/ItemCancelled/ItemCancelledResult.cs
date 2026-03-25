namespace Ambev.DeveloperEvaluation.Application.Sales.ItemCancelled;

/// <summary>
///  Represents the response after cancelling a sale item
/// </summary>
public class ItemCancelledResult
{
    /// <summary>
    /// Gets or sets the sale's unique identifier
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the item's unique identifier
    /// </summary>
    public Guid ItemId { get; set; }

    /// <summary>
    /// Gets or sets the sale number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated total amount of the sale
    /// </summary>
    public decimal UpdatedTotalAmount { get; set; }

    /// <summary>
    /// Gets or sets when the item was cancelled
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets a success message
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
