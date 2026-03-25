using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.ItemCancelled;

/// <summary>
/// Handler for processing ItemCancelledCommand requests
/// </summary>
public class ItemCancelledHandler : IRequestHandler<ItemCancelledCommand, ItemCancelledResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<ItemCancelledHandler> _logger;

    /// <summary>
    /// Initializes a new instance of ItemCancelledHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="logger">The logger</param>
    public ItemCancelledHandler(
        ISaleRepository saleRepository,
        ILogger<ItemCancelledHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }

    public async Task<ItemCancelledResult> Handle(ItemCancelledCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cancelling item {ItemId} from sale {SaleId}", request.ItemId, request.SaleId);

        var validator = new ItemCancelledValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var sale = await _saleRepository.GetByIdWithDetailsAsync(request.SaleId, cancellationToken);
        if (sale is null)
        {
            throw new InvalidOperationException($"Sale with ID {request.SaleId} not found");
        }

        var item = sale.Items.FirstOrDefault(i => i.Id == request.ItemId);
        if (item is null)
        {
            throw new InvalidOperationException($"Item with ID {request.ItemId} not found in sale {request.SaleId}");
        }

        sale.CancelItem(request.ItemId);

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        var itemCancelledEvent = new ItemCancelledEvent(item, updatedSale);
        _logger.LogInformation("ItemCancelledEvent: Item {ItemId} from sale {SaleId} cancelled at {CancelledAt}",
            itemCancelledEvent.SaleItem.Id,
            itemCancelledEvent.Sale.Id,
            itemCancelledEvent.OccurredAt);

        _logger.LogInformation("Sale item cancelled successfully - Sale: {SaleId}, Item: {ItemId}", request.SaleId, request.ItemId);

        return new ItemCancelledResult
        {
            SaleId = updatedSale.Id,
            ItemId = request.ItemId,
            SaleNumber = updatedSale.SaleNumber,
            UpdatedTotalAmount = updatedSale.SaleAmount,
            UpdatedAt = updatedSale.UpdatedAt,
            Message = $"Item {request.ItemId} has been successfully cancelled from sale {updatedSale.SaleNumber}"
        };
    }
}
