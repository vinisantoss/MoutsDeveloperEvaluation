using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a sale item is cancelled
/// </summary>
public class ItemCancelledEvent
{
    public SaleItem SaleItem { get; }
    public Sale Sale { get; }
    public DateTime OccurredAt { get; }


    public ItemCancelledEvent(SaleItem saleItem, Sale sale)
    {
        SaleItem = saleItem;
        Sale = sale;
        OccurredAt = DateTime.UtcNow;
    }
}
