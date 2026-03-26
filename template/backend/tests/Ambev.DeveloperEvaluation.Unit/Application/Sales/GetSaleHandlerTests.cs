using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using DomainSale = Ambev.DeveloperEvaluation.Domain.Entities.Sale;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="GetSaleHandler"/> class.
/// Tests cover successful retrieval, not found scenarios, and invalid input handling.
/// </summary>
public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSaleHandler _handler;

    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();

        _handler = new GetSaleHandler(
            _saleRepository,
            _mapper,
            Substitute.For<Microsoft.Extensions.Logging.ILogger<GetSaleHandler>>());
    }

    /// <summary>
    /// Tests that a valid sale retrieval request returns the sale successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale ID When getting sale Then returns success response")]
    public async Task Handle_ValidSaleId_ReturnsSuccessResponse()
    {
        // Given
        var saleId = Guid.NewGuid();
        var sale = GetSaleHandlerTestData.GenerateValidSale();
        sale.Id = saleId;

        var result = new GetSaleResult { Id = sale.Id };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(result);

        var command = new GetSaleCommand { Id = saleId };

        // When
        var getSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        getSaleResult.Should().NotBeNull();
        getSaleResult.Id.Should().Be(saleId);

        await _saleRepository.Received(1).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that retrieving a non-existent sale throws exception.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale ID When getting sale Then throws InvalidOperationException")]
    public async Task Handle_NonExistentSale_ThrowsException()
    {
        // Given
        var saleId = Guid.NewGuid();

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns((DomainSale?)null);

        var command = new GetSaleCommand { Id = saleId };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage($"*{saleId}*");
    }

    /// <summary>
    /// Tests that retrieving a sale with multiple items returns all items.
    /// </summary>
    [Fact(DisplayName = "Given sale with multiple items When getting sale Then returns all items")]
    public async Task Handle_SaleWithMultipleItems_ReturnsAllItems()
    {
        // Given
        var saleId = Guid.NewGuid();
        var sale = GetSaleHandlerTestData.GenerateSaleWithMultipleItems();
        sale.Id = saleId;

        var result = new GetSaleResult
        {
            Id = saleId
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(result);

        var command = new GetSaleCommand { Id = saleId };

        // When
        var getSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        getSaleResult.Should().NotBeNull();
        sale.Items.Should().HaveCount(2); 
    }

    /// <summary>
    /// Tests that retrieving a cancelled sale returns correct status.
    /// </summary>
    [Fact(DisplayName = "Given cancelled sale When getting sale Then returns cancelled status")]
    public async Task Handle_CancelledSale_ReturnsCancelledStatus()
    {
        // Given
        var saleId = Guid.NewGuid();
        var sale = GetSaleHandlerTestData.GenerateCancelledSale();
        sale.Id = saleId;

        var result = new GetSaleResult { Id = saleId };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(result);

        var command = new GetSaleCommand { Id = saleId };

        // When
        var getSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        getSaleResult.Should().NotBeNull();
        sale.Status.ToString().Should().Contain("Cancelled");
    }

    /// <summary>
    /// Tests that retrieving a sale with cancelled items still returns the sale.
    /// </summary>
    [Fact(DisplayName = "Given sale with cancelled items When getting sale Then returns sale with all items")]
    public async Task Handle_SaleWithCancelledItems_ReturnsSaleWithAllItems()
    {
        // Given
        var saleId = Guid.NewGuid();
        var sale = GetSaleHandlerTestData.GenerateSaleWithCancelledItems();
        sale.Id = saleId;

        var result = new GetSaleResult { Id = saleId };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(result);

        var command = new GetSaleCommand { Id = saleId };

        // When
        var getSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        getSaleResult.Should().NotBeNull();
        sale.Items.Should().NotBeEmpty();
        sale.Items.Should().Contain(i => i.IsCancelled);
    }

    /// <summary>
    /// Tests that mapper is called with correct sale entity.
    /// </summary>
    [Fact(DisplayName = "Given valid sale ID When getting sale Then calls mapper with correct entity")]
    public async Task Handle_ValidRequest_CallsMapperWithCorrectEntity()
    {
        // Given
        var saleId = Guid.NewGuid();
        var sale = GetSaleHandlerTestData.GenerateValidSale();
        sale.Id = saleId;

        var result = new GetSaleResult { Id = saleId };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(result);

        var command = new GetSaleCommand { Id = saleId };

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<GetSaleResult>(Arg.Is<DomainSale>(s => s.Id == saleId));
    }

    /// <summary>
    /// Tests that repository is called with correct sale ID.
    /// </summary>
    [Fact(DisplayName = "Given sale ID When getting sale Then calls repository with correct ID")]
    public async Task Handle_ValidRequest_CallsRepositoryWithCorrectId()
    {
        // Given
        var saleId = Guid.NewGuid();
        var sale = GetSaleHandlerTestData.GenerateValidSale();
        sale.Id = saleId;

        var result = new GetSaleResult { Id = saleId };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(result);

        var command = new GetSaleCommand { Id = saleId };

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).GetByIdAsync(
            Arg.Is<Guid>(id => id == saleId),
            Arg.Any<CancellationToken>());
    }
}