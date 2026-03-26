using Ambev.DeveloperEvaluation.Application.Sales.SaleModified;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using DomainSale = Ambev.DeveloperEvaluation.Domain.Entities.Sale;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="SaleModifiedHandler"/> class.
/// Tests cover add, update, remove operations and mixed operations.
/// </summary>
public class SaleModifiedHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly SaleModifiedHandler _handler;

    public SaleModifiedHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();

        _handler = new SaleModifiedHandler(
            _saleRepository,
            _mapper,
            Substitute.For<Microsoft.Extensions.Logging.ILogger<SaleModifiedHandler>>());
    }

    /// <summary>
    /// Tests adding a new item to an existing sale.
    /// </summary>
    [Fact(DisplayName = "Given valid add operation When modifying sale Then adds item successfully")]
    public async Task Handle_ValidAddOperation_AddsItemSuccessfully()
    {
        // Given
        var command = SaleModifiedHandlerTestData.GenerateValidAddCommand();
        var sale = SaleModifiedHandlerTestData.GenerateExistingSale();
        sale.Id = command.Id;

        var result = new SaleModifiedResult { SaleNumber = sale.SaleNumber };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<SaleModifiedResult>(sale).Returns(result);

        // When
        var saleModifiedResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        saleModifiedResult.Should().NotBeNull();
        saleModifiedResult.SaleNumber.Should().Be(sale.SaleNumber);
        await _saleRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests updating an existing item quantity.
    /// </summary>
    [Fact(DisplayName = "Given valid update operation When modifying sale Then updates item quantity successfully")]
    public async Task Handle_ValidUpdateOperation_UpdatesQuantitySuccessfully()
    {
        // Given
        var command = SaleModifiedHandlerTestData.GenerateValidUpdateCommand();
        var sale = SaleModifiedHandlerTestData.GenerateExistingSale();
        sale.Id = command.Id;
        command.Items[0].ItemId = sale.Items[0].Id;

        var result = new SaleModifiedResult { SaleNumber = sale.SaleNumber };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<SaleModifiedResult>(sale).Returns(result);

        // When
        var saleModifiedResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        saleModifiedResult.Should().NotBeNull();
        saleModifiedResult.SaleNumber.Should().Be(sale.SaleNumber);
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests removing an item from a sale.
    /// </summary>
    [Fact(DisplayName = "Given valid remove operation When modifying sale Then removes item successfully")]
    public async Task Handle_ValidRemoveOperation_RemovesItemSuccessfully()
    {
        // Given
        var command = SaleModifiedHandlerTestData.GenerateValidRemoveCommand();
        var sale = SaleModifiedHandlerTestData.GenerateExistingSale();
        sale.Id = command.Id;
        command.Items[0].ItemId = sale.Items[0].Id;

        var result = new SaleModifiedResult { SaleNumber = sale.SaleNumber };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<SaleModifiedResult>(sale).Returns(result);

        // When
        var saleModifiedResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        saleModifiedResult.Should().NotBeNull();
        saleModifiedResult.SaleNumber.Should().Be(sale.SaleNumber);
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests mixed operations (add, update, remove) on the same sale.
    /// </summary>
    [Fact(DisplayName = "Given mixed operations When modifying sale Then processes all operations successfully")]
    public async Task Handle_MixedOperations_ProcessesAllSuccessfully()
    {
        // Given
        var command = SaleModifiedHandlerTestData.GenerateValidMixedOperationsCommand();
        var sale = SaleModifiedHandlerTestData.GenerateExistingSale();
        sale.Id = command.Id;
        command.Items[1].ItemId = sale.Items[0].Id;
        command.Items[2].ItemId = sale.Items[0].Id;

        var result = new SaleModifiedResult { SaleNumber = sale.SaleNumber };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<SaleModifiedResult>(sale).Returns(result);

        // When
        var saleModifiedResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        saleModifiedResult.Should().NotBeNull();
        saleModifiedResult.SaleNumber.Should().Be(sale.SaleNumber);
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that non-existent sale throws exception.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale When modifying Then throws InvalidOperationException")]
    public async Task Handle_NonExistentSale_ThrowsException()
    {
        // Given
        var command = SaleModifiedHandlerTestData.GenerateValidAddCommand();

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((DomainSale?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage($"*{command.Id}*");
    }

    /// <summary>
    /// Tests that invalid item data throws exception.
    /// </summary>
    [Fact(DisplayName = "Given add operation with missing product When modifying sale Then throws InvalidOperationException")]
    public async Task Handle_InvalidProductInfo_ThrowsException()
    {
        // Given
        var command = SaleModifiedHandlerTestData.GenerateValidAddCommand();
        var sale = SaleModifiedHandlerTestData.GenerateExistingSale();
        sale.Id = command.Id;

        // Remove product info to simulate missing data
        command.Items[0].Product = null;

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    /// <summary>
    /// Tests that updating non-existent item throws exception.
    /// </summary>
    [Fact(DisplayName = "Given update operation with invalid item ID When modifying sale Then throws exception")]
    public async Task Handle_InvalidItemId_ThrowsException()
    {
        // Given
        var command = SaleModifiedHandlerTestData.GenerateValidUpdateCommand();
        var sale = SaleModifiedHandlerTestData.GenerateExistingSale();
        sale.Id = command.Id;
        command.Items[0].ItemId = Guid.NewGuid(); // Non-existent item

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<Exception>();
    }

    /// <summary>
    /// Tests that repository update is called.
    /// </summary>
    [Fact(DisplayName = "Given valid command When modifying sale Then repository persists changes")]
    public async Task Handle_ValidRequest_CallsRepositoryUpdate()
    {
        // Given
        var command = SaleModifiedHandlerTestData.GenerateValidUpdateCommand();
        var sale = SaleModifiedHandlerTestData.GenerateExistingSale();
        sale.Id = command.Id;
        command.Items[0].ItemId = sale.Items[0].Id;

        var result = new SaleModifiedResult { SaleNumber = sale.SaleNumber };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<SaleModifiedResult>(sale).Returns(result);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).UpdateAsync(
            Arg.Is<DomainSale>(s => s.Id == command.Id),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that mapper is called with updated sale.
    /// </summary>
    [Fact(DisplayName = "Given valid command When modifying sale Then mapper maps result")]
    public async Task Handle_ValidRequest_CallsMapper()
    {
        // Given
        var command = SaleModifiedHandlerTestData.GenerateValidUpdateCommand();
        var sale = SaleModifiedHandlerTestData.GenerateExistingSale();
        sale.Id = command.Id;
        command.Items[0].ItemId = sale.Items[0].Id;

        var result = new SaleModifiedResult { SaleNumber = sale.SaleNumber };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<SaleModifiedResult>(sale).Returns(result);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<SaleModifiedResult>(Arg.Is<DomainSale>(s => s.Id == sale.Id));
    }
}