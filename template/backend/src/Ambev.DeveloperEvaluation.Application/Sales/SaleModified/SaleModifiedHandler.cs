using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.SaleModified;

/// <summary>
/// Handler for processing SaleModifiedCommand requests
/// </summary>
public class SaleModifiedHandler : IRequestHandler<SaleModifiedCommand, SaleModifiedResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<SaleModifiedHandler> _logger;

    /// <summary>
    ///  Initializes a new instance of SaleModifiedHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger</param>
    public SaleModifiedHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<SaleModifiedHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SaleModifiedResult> Handle(SaleModifiedCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating sale with ID {SaleId}", request.Id);

        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (sale is null)
        {
            throw new InvalidOperationException($"Sale with ID {request.Id} not found");
        }

        foreach (var itemInfo in request.Items)
        {
            switch (itemInfo.Operation)
            {
                case UpdateOperation.Add:
                    await AddNewItem(sale, itemInfo);
                    break;

                case UpdateOperation.Update:
                    UpdateExistingItem(sale, itemInfo);
                    break;

                case UpdateOperation.Remove:
                    RemoveItem(sale, itemInfo);
                    break;
            }
        }

        var validationResult = sale.Validate();
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.Detail));
            throw new InvalidOperationException($"Sale validation failed: {errors}");
        }

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        var saleModifiedEvent = new SaleModifiedEvent(updatedSale, "ItemsUpdated");
        _logger.LogInformation("SaleModified event: Sale {SaleId} modified at {ModifiedAt} - {ModificationType}",
            saleModifiedEvent.Sale.Id,
            saleModifiedEvent.OccurredAt,
            saleModifiedEvent.ModificationType);

        _logger.LogInformation("Sale updated successfully with ID {SaleId}", updatedSale.Id);

        return _mapper.Map<SaleModifiedResult>(updatedSale);
    }

    private async Task AddNewItem(Sale sale, SaleItem saleItem)
    {
        if (saleItem.Product is null || saleItem.ItemPrice is null)
        {
            throw new InvalidOperationException("Product information and item price are required for adding new items");
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            ExternalId = saleItem.Product.ExternalId,
            Name = saleItem.Product.Name,
            Category = saleItem.Product.Category,
            StandardPrice = saleItem.Product.StandardPrice
        };

        sale.AddItem(product.Id, saleItem.Quantity, saleItem.ItemPrice.Value);

        var lastItem = sale.Items.Last();
        lastItem.Product = product;
    }

    private void UpdateExistingItem(Sale sale, SaleItem saleItem)
    {
        if (saleItem.ItemId is null)
        {
            throw new InvalidOperationException("Item ID is required for updating existing items");
        }

        sale.UpdateItemQuantity(saleItem.ItemId.Value, saleItem.Quantity);
    }

    private void RemoveItem(Sale sale, SaleItem saleItem)
    {
        if (saleItem.ItemId is null)
        {
            throw new InvalidOperationException("Item ID is required for removing items");
        }

        sale.CancelItem(saleItem.ItemId.Value);
    }
}
