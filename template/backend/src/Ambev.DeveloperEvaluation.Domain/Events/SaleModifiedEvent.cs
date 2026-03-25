using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a sale is modified
/// </summary>
public class SaleModifiedEvent
{
    public Sale Sale { get; }
    public string ModificationType { get; }
    public DateTime OccurredAt { get; }

    public SaleModifiedEvent(Sale sale, string modificationType)
    {
        Sale = sale;
        ModificationType = modificationType;
        OccurredAt = DateTime.UtcNow;
    }

}
