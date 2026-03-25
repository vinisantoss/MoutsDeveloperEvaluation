using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ItemCancelled;

/// <summary>
/// Command for cancelling a specific item in a sale
/// </summary>
public class ItemCancelledCommand : IRequest<ItemCancelledResult>
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
    /// Initializes a new ItemCancelledCommand
    /// </summary>
    /// <param name="saleId">The sale ID</param>
    /// <param name="itemId">The item ID</param>
    public ItemCancelledCommand(Guid saleId, Guid itemId)
    {
        SaleId = saleId;
        ItemId = itemId;
    }

}
