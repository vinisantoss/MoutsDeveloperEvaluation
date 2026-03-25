using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a sale is cancelled
/// </summary>
public class SaleCancelledEvent
{
    public Sale Sale { get; }
    public DateTime OccurredAt { get; }

    public SaleCancelledEvent(Sale sale)
    {
        Sale = sale;
        OccurredAt = DateTime.Now;
    }
}
