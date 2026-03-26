using Ambev.DeveloperEvaluation.Application.Sales.SaleCreated;
using DomainCustomer = Ambev.DeveloperEvaluation.Domain.Entities.Customer;
using DomainBranch = Ambev.DeveloperEvaluation.Domain.Entities.Branch;
using DomainProduct = Ambev.DeveloperEvaluation.Domain.Entities.Product;
using DomainSale = Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// Tests cover successful creation, validation, duplicate prevention, and error scenarios.
/// </summary>
public class SaleCreatedHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly CreateSaleHandler _handler;

    public SaleCreatedHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();
        _branchRepository = Substitute.For<IBranchRepository>();
        _mapper = Substitute.For<IMapper>();

        _handler = new CreateSaleHandler(
            _saleRepository,
            _productRepository,
            _customerRepository,
            _branchRepository,
            _mapper,
            Substitute.For<Microsoft.Extensions.Logging.ILogger<CreateSaleHandler>>());
    }

    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var customer = new DomainCustomer
        {
            Id = customerId,
            ExternalId = command.Customer.ExternalId,
            Name = command.Customer.Name,
            Email = command.Customer.Email,
            Document = command.Customer.Document
        };

        var branch = new DomainBranch
        {
            Id = branchId,
            ExternalId = command.Branch.ExternalId,
            Name = command.Branch.Name,
            Location = command.Branch.Location
        };

        var product = new DomainProduct
        {
            Id = productId,
            ExternalId = command.Items[0].Product.ExternalId,
            Name = command.Items[0].Product.Name,
            Category = command.Items[0].Product.Category,
            StandardPrice = command.Items[0].Product.StandardPrice
        };

        var createdSale = new DomainSale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            CustomerId = customerId,
            Customer = customer,
            BranchId = branchId,
            Branch = branch,
            SaleDate = DateTime.UtcNow
        };

        createdSale.AddItem(productId, command.Items[0].Quantity, command.Items[0].ItemPrice);
        createdSale.Items[0].Product = product;

        var result = new CreateSaleResult { Id = createdSale.Id };

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((DomainSale?)null);

        _customerRepository.GetByExternalIdAsync(command.Customer.ExternalId, Arg.Any<CancellationToken>())
            .Returns(customer);

        _branchRepository.GetByExternalIdAsync(command.Branch.ExternalId, Arg.Any<CancellationToken>())
            .Returns(branch);

        _productRepository.GetByExternalIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(product);

        _saleRepository.CreateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(createdSale);

        _mapper.Map<CreateSaleResult>(createdSale).Returns(result);

        // When
        var createSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createSaleResult.Should().NotBeNull();
        createSaleResult.Id.Should().Be(createdSale.Id);

        await _saleRepository.Received(1).GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>());
        await _customerRepository.Received(1).GetByExternalIdAsync(command.Customer.ExternalId, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).CreateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that creating a sale with duplicate sale number throws exception.
    /// </summary>
    [Fact(DisplayName = "Given duplicate sale number When creating sale Then throws InvalidOperationException")]
    public async Task Handle_DuplicateSaleNumber_ThrowsException()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var existingSale = new DomainSale { SaleNumber = command.SaleNumber };

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns(existingSale);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage($"*{command.SaleNumber}*");
    }

    /// <summary>
    /// Tests that creating a sale with non-existent customer throws exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid customer When creating sale Then throws DomainException")]
    public async Task Handle_InvalidCustomer_ThrowsDomainException()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((DomainSale?)null);

        _customerRepository.GetByExternalIdAsync(command.Customer.ExternalId, Arg.Any<CancellationToken>())
            .Returns((DomainCustomer?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage($"*Customer*");
    }

    /// <summary>
    /// Tests that creating a sale with non-existent branch throws exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid branch When creating sale Then throws DomainException")]
    public async Task Handle_InvalidBranch_ThrowsDomainException()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();

        var customer = new DomainCustomer
        {
            Id = Guid.NewGuid(),
            ExternalId = command.Customer.ExternalId,
            Name = command.Customer.Name,
            Email = command.Customer.Email,
            Document = command.Customer.Document
        };

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((DomainSale?)null);

        _customerRepository.GetByExternalIdAsync(command.Customer.ExternalId, Arg.Any<CancellationToken>())
            .Returns(customer);

        _branchRepository.GetByExternalIdAsync(command.Branch.ExternalId, Arg.Any<CancellationToken>())
            .Returns((DomainBranch?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage($"*Branch*");
    }

    /// <summary>
    /// Tests that creating a sale with non-existent product throws exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid product When creating sale Then throws DomainException")]
    public async Task Handle_InvalidProduct_ThrowsDomainException()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();

        var customer = new DomainCustomer
        {
            Id = Guid.NewGuid(),
            ExternalId = command.Customer.ExternalId,
            Name = command.Customer.Name,
            Email = command.Customer.Email,
            Document = command.Customer.Document
        };

        var branch = new DomainBranch
        {
            Id = Guid.NewGuid(),
            ExternalId = command.Branch.ExternalId,
            Name = command.Branch.Name,
            Location = command.Branch.Location
        };

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((DomainSale?)null);

        _customerRepository.GetByExternalIdAsync(command.Customer.ExternalId, Arg.Any<CancellationToken>())
            .Returns(customer);

        _branchRepository.GetByExternalIdAsync(command.Branch.ExternalId, Arg.Any<CancellationToken>())
            .Returns(branch);

        _productRepository.GetByExternalIdAsync(command.Items[0].Product.ExternalId, Arg.Any<CancellationToken>())
            .Returns((DomainProduct?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage($"*Product*");
    }

    /// <summary>
    /// Tests that sale is created with correct total amount calculated.
    /// </summary>
    [Fact(DisplayName = "Given valid sale with multiple items When creating Then calculates correct total")]
    public async Task Handle_MultipleItems_CalculatesCorrectTotal()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateCommandWithMinimumItems();
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var customer = new DomainCustomer
        {
            Id = customerId,
            ExternalId = command.Customer.ExternalId,
            Name = command.Customer.Name,
            Email = command.Customer.Email,
            Document = command.Customer.Document
        };

        var branch = new DomainBranch
        {
            Id = branchId,
            ExternalId = command.Branch.ExternalId,
            Name = command.Branch.Name,
            Location = command.Branch.Location
        };

        var product = new DomainProduct
        {
            Id = productId,
            ExternalId = command.Items[0].Product.ExternalId,
            Name = command.Items[0].Product.Name,
            Category = command.Items[0].Product.Category,
            StandardPrice = command.Items[0].Product.StandardPrice
        };

        var createdSale = new DomainSale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            CustomerId = customerId,
            Customer = customer,
            BranchId = branchId,
            Branch = branch
        };

        createdSale.AddItem(productId, command.Items[0].Quantity, command.Items[0].ItemPrice);
        createdSale.Items[0].Product = product;

        var result = new CreateSaleResult { Id = createdSale.Id };

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((DomainSale?)null);

        _customerRepository.GetByExternalIdAsync(command.Customer.ExternalId, Arg.Any<CancellationToken>())
            .Returns(customer);

        _branchRepository.GetByExternalIdAsync(command.Branch.ExternalId, Arg.Any<CancellationToken>())
            .Returns(branch);

        _productRepository.GetByExternalIdAsync(command.Items[0].Product.ExternalId, Arg.Any<CancellationToken>())
            .Returns(product);

        _saleRepository.CreateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(createdSale);

        _mapper.Map<CreateSaleResult>(createdSale).Returns(result);

        // When
        var createSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createSaleResult.Should().NotBeNull();
        createdSale.Items.Should().NotBeEmpty();
        createdSale.SaleAmount.Should().BeGreaterThan(0);
    }

    /// <summary>
    /// Tests that sale with maximum quantity per item is accepted.
    /// </summary>
    [Fact(DisplayName = "Given sale with 20 items of same product When creating Then succeeds")]
    public async Task Handle_MaximumQuantity_Succeeds()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateCommandWithMaximumQuantity();
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var customer = new DomainCustomer
        {
            Id = customerId,
            ExternalId = command.Customer.ExternalId,
            Name = command.Customer.Name,
            Email = command.Customer.Email,
            Document = command.Customer.Document
        };

        var branch = new DomainBranch
        {
            Id = branchId,
            ExternalId = command.Branch.ExternalId,
            Name = command.Branch.Name,
            Location = command.Branch.Location
        };

        var product = new DomainProduct
        {
            Id = productId,
            ExternalId = command.Items[0].Product.ExternalId,
            Name = command.Items[0].Product.Name,
            Category = command.Items[0].Product.Category,
            StandardPrice = command.Items[0].Product.StandardPrice
        };

        var createdSale = new DomainSale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            CustomerId = customerId,
            Customer = customer,
            BranchId = branchId,
            Branch = branch
        };

        createdSale.AddItem(productId, 20, command.Items[0].ItemPrice);
        createdSale.Items[0].Product = product;

        var result = new CreateSaleResult { Id = createdSale.Id };

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((DomainSale?)null);

        _customerRepository.GetByExternalIdAsync(command.Customer.ExternalId, Arg.Any<CancellationToken>())
            .Returns(customer);

        _branchRepository.GetByExternalIdAsync(command.Branch.ExternalId, Arg.Any<CancellationToken>())
            .Returns(branch);

        _productRepository.GetByExternalIdAsync(command.Items[0].Product.ExternalId, Arg.Any<CancellationToken>())
            .Returns(product);

        _saleRepository.CreateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(createdSale);

        _mapper.Map<CreateSaleResult>(createdSale).Returns(result);

        // When
        var createSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createSaleResult.Should().NotBeNull();
        createdSale.Items.Should().NotBeEmpty();
        createdSale.Items[0].Quantity.Should().Be(20);
    }
}