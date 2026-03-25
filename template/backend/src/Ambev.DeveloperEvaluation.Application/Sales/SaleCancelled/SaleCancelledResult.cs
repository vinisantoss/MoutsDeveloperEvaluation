
namespace Ambev.DeveloperEvaluation.Application.Sales.SaleCancelled;

/// <summary>
/// Represents the response after cancelling a sale
/// </summary>
public class SaleCancelledResult
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
    /// Gets or sets the sale status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets when the sale was cancelled
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets a success message
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
