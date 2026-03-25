using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;


/// <summary>
/// Handler for processing GetSaleCommand requests
/// </summary>
public class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of GetSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger</param>
    public GetSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<GetSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the GetSaleCommand Request
    /// </summary>
    /// <param name="request">The GetSaleCommand</param>
    /// <param name="cancellationToken">The Cancellation Token</param>
    /// <returns>The sale result</returns>
    /// <exception cref="ValidationException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<GetSaleResult> Handle(GetSaleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving sale with ID {SaleId}", request.Id);

        var validator = new GetSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (sale is null)
        {
            throw new InvalidOperationException($"Sale with ID {request.Id} not found");
        }

        _logger.LogInformation("Sale retrieved successfully with ID {SaleId}", sale.Id);

        return _mapper.Map<GetSaleResult>(sale);
    }
}
