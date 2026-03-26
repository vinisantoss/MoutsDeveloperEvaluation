using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.SaleCreated;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="productRepository">The product repository</param>
    /// <param name="customerRepository">The customer repository</param>
    /// <param name="branchRepository">The branch repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger</param>
    public CreateSaleHandler(
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        ICustomerRepository customerRepository,
        IBranchRepository branchRepository,
        IMapper mapper,
        ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _branchRepository = branchRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="request">The CreateCommercialTransaction command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created transaction details</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating sale with code {TransactionCode}", request.SaleNumber);

        var validator = new CreateSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingSale = await _saleRepository.GetBySaleNumberAsync(request.SaleNumber, cancellationToken);
        if (existingSale is not null)
        {
            throw new InvalidOperationException($"Sale with code {request.SaleNumber} already exists");
        }

        var customerInfo = await _customerRepository.GetByExternalIdAsync(request.Customer.ExternalId, cancellationToken);
        if (customerInfo is null)
        {
            throw new DomainException($"Customer with ExternalId '{request.Customer.ExternalId}' does not exists.");
        }

        var branchInfo = await _branchRepository.GetByExternalIdAsync(request.Branch.ExternalId, cancellationToken);
        if (branchInfo is null)
        {
            throw new DomainException($"Branch with ExternalId '{request.Branch.ExternalId}' does not exists.");
        }

        var sale = new Sale
        {
            SaleNumber = request.SaleNumber,
            CustomerId = customerInfo.Id,
            Customer = customerInfo,
            BranchId = branchInfo.Id,
            Branch = branchInfo,
            SaleDate = DateTime.UtcNow
        };

        foreach (var itemInfo in request.Items)
        {
            var product = await _productRepository.GetByExternalIdAsync(itemInfo.Product.ExternalId, cancellationToken);
            if (product is null)
            {
                throw new DomainException($"Product with ExternalId '{itemInfo.Product.ExternalId}' not found.");
            }

            sale.AddItem(product.Id, itemInfo.Quantity, itemInfo.ItemPrice);

            var lastItem = sale.Items.Last();
            lastItem.Product = product;
        }

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

        var saleCreatedEvent = new SaleCreatedEvent(createdSale);
        _logger.LogInformation("CreatedSale event: Sale {Id} with number {SaleNumber} created at {CreatedAt}",
            saleCreatedEvent.Sale.Id,
            saleCreatedEvent.Sale.SaleNumber,
            saleCreatedEvent.OccurredAt);

        _logger.LogInformation("Sale created successfully with ID {Id}", createdSale.Id);

        return _mapper.Map<CreateSaleResult>(createdSale);
    }
}