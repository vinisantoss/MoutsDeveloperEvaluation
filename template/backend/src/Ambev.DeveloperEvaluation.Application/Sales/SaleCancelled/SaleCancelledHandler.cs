
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.SaleCancelled;

/// <summary>
/// Handler for processing SaleCancelledCommand requests
/// </summary>
public class SaleCancelledHandler : IRequestHandler<SaleCancelledCommand, SaleCancelledResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<SaleCancelledHandler> _logger;

    public SaleCancelledHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<SaleCancelledHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    ///  Handles the SaleCancelledCommand Request
    /// </summary>
    /// <param name="request">The SaleCancelled command</param>
    /// <param name="cancellationToken">The Cancelation Token</param>
    /// <returns>The Cancellation result</returns>
    public async Task<SaleCancelledResult> Handle(SaleCancelledCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cancelling sale with ID {SaleId}", request.Id);

        var validator = new SaleCancelledValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (sale is null)
        {
            throw new InvalidOperationException($"Sale with ID {request.Id} not found");
        }

        sale.Cancel();

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        var transactionCancelledEvent = new SaleCancelledEvent(updatedSale);
        _logger.LogInformation("SaleCancelled event: Sale {SaleId} with code {SaleNumber} cancelled at {CancelledAt}",
            transactionCancelledEvent.Sale.Id,
            transactionCancelledEvent.Sale.SaleNumber,
            transactionCancelledEvent.OccurredAt);

        _logger.LogInformation("Sale cancelled successfully with ID {SaleId}", updatedSale.Id);

        return _mapper.Map<SaleCancelledResult>(updatedSale);
    }
}
