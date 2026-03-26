using Ambev.DeveloperEvaluation.Application.Sales.SaleCancelled;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;
using DomainSale = Ambev.DeveloperEvaluation.Domain.Entities.Sale;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="SaleCancelledHandler"/> class.
/// Tests cover successful cancellation, validation, already cancelled scenarios, and error handling.
/// </summary>
public class SaleCancelledHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly SaleCancelledHandler _handler;

    public SaleCancelledHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();

        _handler = new SaleCancelledHandler(
            _saleRepository,
            _mapper,
            Substitute.For<Microsoft.Extensions.Logging.ILogger<SaleCancelledHandler>>());
    }

    [Fact(DisplayName = "Given valid sale ID When cancelling sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        var sale = SaleCancelledHandlerTestData.GenerateValidSaleForCancellation();
        var command = SaleCancelledHandlerTestData.GenerateValidCommand(sale.Id);

        var result = new SaleCancelledResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<SaleCancelledResult>(sale).Returns(result);

        var saleCancelledResult = await _handler.Handle(command, CancellationToken.None);

        saleCancelledResult.Should().NotBeNull();
        saleCancelledResult.Id.Should().Be(sale.Id);
        saleCancelledResult.SaleNumber.Should().Be(sale.SaleNumber);

        await _saleRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given non-existent sale ID When cancelling sale Then throws InvalidOperationException")]
    public async Task Handle_NonExistentSale_ThrowsException()
    {
        var command = SaleCancelledHandlerTestData.GenerateValidCommand();

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((DomainSale?)null);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage($"*{command.Id}*");
    }

    [Fact(DisplayName = "Given sale with multiple items When cancelling sale Then all items are cancelled")]
    public async Task Handle_SaleWithMultipleItems_CancelsAllItems()
    {
        var sale = SaleCancelledHandlerTestData.GenerateSaleWithMultipleItemsForCancellation();
        var command = SaleCancelledHandlerTestData.GenerateValidCommand(sale.Id);

        var result = new SaleCancelledResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(x =>
            {
                var updatedSale = x.ArgAt<DomainSale>(0);
                foreach (var item in updatedSale.Items)
                {
                    item.IsCancelled = true;
                }
                return updatedSale;
            });

        _mapper.Map<SaleCancelledResult>(sale).Returns(result);

        var saleCancelledResult = await _handler.Handle(command, CancellationToken.None);

        saleCancelledResult.Should().NotBeNull();
        sale.Items.Should().AllSatisfy(item => item.IsCancelled.Should().BeTrue());
    }

    [Fact(DisplayName = "Given sale with partially cancelled items When cancelling sale Then remaining items are cancelled")]
    public async Task Handle_SaleWithPartiallyCancelledItems_CancelsRemainingItems()
    {
        var sale = SaleCancelledHandlerTestData.GenerateSaleWithPartiallyCancelledItems();
        var command = SaleCancelledHandlerTestData.GenerateValidCommand(sale.Id);

        var result = new SaleCancelledResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(x =>
            {
                var updatedSale = x.ArgAt<DomainSale>(0);
                foreach (var item in updatedSale.Items)
                {
                    item.IsCancelled = true;
                }
                return updatedSale;
            });

        _mapper.Map<SaleCancelledResult>(sale).Returns(result);

        var saleCancelledResult = await _handler.Handle(command, CancellationToken.None);

        saleCancelledResult.Should().NotBeNull();
        sale.Items.Should().AllSatisfy(item => item.IsCancelled.Should().BeTrue());
    }

    [Fact(DisplayName = "Given already cancelled sale When cancelling again Then returns success (idempotent)")]
    public async Task Handle_AlreadyCancelledSale_SucceedsIdempotently()
    {
        var sale = SaleCancelledHandlerTestData.GenerateAlreadyCancelledSale();
        var command = SaleCancelledHandlerTestData.GenerateValidCommand(sale.Id);

        var result = new SaleCancelledResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<SaleCancelledResult>(sale).Returns(result);

        var saleCancelledResult = await _handler.Handle(command, CancellationToken.None);

        saleCancelledResult.Should().NotBeNull();
        saleCancelledResult.Id.Should().Be(sale.Id);
    }

    [Fact(DisplayName = "Given valid sale When cancelling Then sale status is set to Cancelled")]
    public async Task Handle_ValidRequest_UpdatesSaleStatus()
    {
        var sale = SaleCancelledHandlerTestData.GenerateValidSaleForCancellation();
        var command = SaleCancelledHandlerTestData.GenerateValidCommand(sale.Id);

        var result = new SaleCancelledResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<SaleCancelledResult>(sale).Returns(result);

        await _handler.Handle(command, CancellationToken.None);

        await _saleRepository.Received(1).UpdateAsync(
            Arg.Is<DomainSale>(s => s.Status.ToString().Contains("Cancelled")),
            Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given sale ID When cancelling sale Then repository retrieves correct sale")]
    public async Task Handle_ValidRequest_CallsRepositoryWithCorrectId()
    {
        var sale = SaleCancelledHandlerTestData.GenerateValidSaleForCancellation();
        var command = SaleCancelledHandlerTestData.GenerateValidCommand(sale.Id);

        var result = new SaleCancelledResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<SaleCancelledResult>(sale).Returns(result);

        await _handler.Handle(command, CancellationToken.None);

        await _saleRepository.Received(1).GetByIdAsync(
            Arg.Is<Guid>(id => id == command.Id),
            Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given valid command When cancelling sale Then mapper is called with updated sale")]
    public async Task Handle_ValidRequest_CallsMapperWithUpdatedSale()
    {
        var sale = SaleCancelledHandlerTestData.GenerateValidSaleForCancellation();
        var command = SaleCancelledHandlerTestData.GenerateValidCommand(sale.Id);

        var result = new SaleCancelledResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<SaleCancelledResult>(sale).Returns(result);

        await _handler.Handle(command, CancellationToken.None);

        _mapper.Received(1).Map<SaleCancelledResult>(Arg.Is<DomainSale>(s => s.Id == sale.Id));
    }

    [Fact(DisplayName = "Given invalid command with empty ID When cancelling sale Then throws ValidationException")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        var command = new SaleCancelledCommand(Guid.Empty);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "Given valid sale When cancelling Then SaleCancelledEvent is created")]
    public async Task Handle_ValidRequest_CreatesSaleCancelledEvent()
    {
        var sale = SaleCancelledHandlerTestData.GenerateValidSaleForCancellation();
        var command = SaleCancelledHandlerTestData.GenerateValidCommand(sale.Id);

        var result = new SaleCancelledResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<SaleCancelledResult>(sale).Returns(result);

        var saleCancelledResult = await _handler.Handle(command, CancellationToken.None);

        saleCancelledResult.Should().NotBeNull();
    }
}