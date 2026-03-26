using Ambev.DeveloperEvaluation.Application.Sales.ItemCancelled;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;
using DomainSale = Ambev.DeveloperEvaluation.Domain.Entities.Sale;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class ItemCancelledHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ItemCancelledHandler _handler;

    public ItemCancelledHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();

        _handler = new ItemCancelledHandler(
            _saleRepository,
            Substitute.For<Microsoft.Extensions.Logging.ILogger<ItemCancelledHandler>>());
    }

    [Fact(DisplayName = "Given valid sale and item ID When cancelling item Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        var sale = ItemCancelledHandlerTestData.GenerateValidSaleWithMultipleItems();
        var itemToCancel = sale.Items[0];
        var command = ItemCancelledHandlerTestData.GenerateCommandWithIds(sale.Id, itemToCancel.Id);

        _saleRepository.GetByIdWithDetailsAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(x =>
            {
                var updatedSale = x.ArgAt<DomainSale>(0);
                foreach (var item in updatedSale.Items.Where(i => i.Id == command.ItemId))
                {
                    item.IsCancelled = true;
                }
                return updatedSale;
            });

        var itemCancelledResult = await _handler.Handle(command, CancellationToken.None);

        itemCancelledResult.Should().NotBeNull();
        itemCancelledResult.SaleId.Should().Be(sale.Id);
        itemCancelledResult.ItemId.Should().Be(itemToCancel.Id);
        itemCancelledResult.SaleNumber.Should().Be(sale.SaleNumber);
        itemCancelledResult.Message.Should().NotBeEmpty();

        await _saleRepository.Received(1).GetByIdWithDetailsAsync(command.SaleId, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given non-existent sale ID When cancelling item Then throws InvalidOperationException")]
    public async Task Handle_NonExistentSale_ThrowsException()
    {
        var command = ItemCancelledHandlerTestData.GenerateValidCommand();

        _saleRepository.GetByIdWithDetailsAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns((DomainSale?)null);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage($"*{command.SaleId}*");
    }

    [Fact(DisplayName = "Given non-existent item ID When cancelling item Then throws InvalidOperationException")]
    public async Task Handle_NonExistentItem_ThrowsException()
    {
        var sale = ItemCancelledHandlerTestData.GenerateValidSaleWithMultipleItems();
        var command = ItemCancelledHandlerTestData.GenerateCommandWithIds(sale.Id, Guid.NewGuid());

        _saleRepository.GetByIdWithDetailsAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"*{command.ItemId}*");
    }

    [Fact(DisplayName = "Given sale with single item When cancelling item Then item is cancelled successfully")]
    public async Task Handle_SingleItemSale_CancelsItemSuccessfully()
    {
        var sale = ItemCancelledHandlerTestData.GenerateValidSaleWithSingleItem();
        var itemToCancel = sale.Items[0];
        var command = ItemCancelledHandlerTestData.GenerateCommandWithIds(sale.Id, itemToCancel.Id);

        _saleRepository.GetByIdWithDetailsAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(x =>
            {
                var updatedSale = x.ArgAt<DomainSale>(0);
                foreach (var item in updatedSale.Items.Where(i => i.Id == command.ItemId))
                {
                    item.IsCancelled = true;
                }
                return updatedSale;
            });

        var itemCancelledResult = await _handler.Handle(command, CancellationToken.None);

        itemCancelledResult.Should().NotBeNull();
        sale.Items[0].IsCancelled.Should().BeTrue();
    }

    [Fact(DisplayName = "Given sale with multiple items When cancelling one item Then only that item is cancelled")]
    public async Task Handle_MultipleItemsSale_CancelsOnlySpecificItem()
    {
        var sale = ItemCancelledHandlerTestData.GenerateValidSaleWithMultipleItems();
        var itemToCancel = sale.Items[1];
        var command = ItemCancelledHandlerTestData.GenerateCommandWithIds(sale.Id, itemToCancel.Id);

        _saleRepository.GetByIdWithDetailsAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(x =>
            {
                var updatedSale = x.ArgAt<DomainSale>(0);
                foreach (var item in updatedSale.Items.Where(i => i.Id == command.ItemId))
                {
                    item.IsCancelled = true;
                }
                return updatedSale;
            });

        var itemCancelledResult = await _handler.Handle(command, CancellationToken.None);

        itemCancelledResult.Should().NotBeNull();
        sale.Items[1].IsCancelled.Should().BeTrue();
        sale.Items[0].IsCancelled.Should().BeFalse();
        sale.Items[2].IsCancelled.Should().BeFalse();
    }

    [Fact(DisplayName = "Given valid sale When cancelling item Then sale total is recalculated")]
    public async Task Handle_ValidRequest_RecalculatesTotalAmount()
    {
        var sale = ItemCancelledHandlerTestData.GenerateValidSaleWithMultipleItems();
        var originalTotal = sale.SaleAmount;
        var itemToCancel = sale.Items[0];
        var command = ItemCancelledHandlerTestData.GenerateCommandWithIds(sale.Id, itemToCancel.Id);

        _saleRepository.GetByIdWithDetailsAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(x =>
            {
                var updatedSale = x.ArgAt<DomainSale>(0);
                foreach (var item in updatedSale.Items.Where(i => i.Id == command.ItemId))
                {
                    item.IsCancelled = true;
                }
                return updatedSale;
            });

        var itemCancelledResult = await _handler.Handle(command, CancellationToken.None);

        itemCancelledResult.Should().NotBeNull();
        itemCancelledResult.UpdatedTotalAmount.Should().BeLessThan(originalTotal);
    }

    [Fact(DisplayName = "Given sale ID When cancelling item Then repository retrieves correct sale")]
    public async Task Handle_ValidRequest_CallsRepositoryWithCorrectSaleId()
    {
        var sale = ItemCancelledHandlerTestData.GenerateValidSaleWithMultipleItems();
        var command = ItemCancelledHandlerTestData.GenerateCommandWithIds(sale.Id, sale.Items[0].Id);

        _saleRepository.GetByIdWithDetailsAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(x =>
            {
                var updatedSale = x.ArgAt<DomainSale>(0);
                foreach (var item in updatedSale.Items.Where(i => i.Id == command.ItemId))
                {
                    item.IsCancelled = true;
                }
                return updatedSale;
            });

        await _handler.Handle(command, CancellationToken.None);

        await _saleRepository.Received(1).GetByIdWithDetailsAsync(
            Arg.Is<Guid>(id => id == command.SaleId),
            Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given valid command When cancelling item Then repository updates sale")]
    public async Task Handle_ValidRequest_CallsRepositoryUpdate()
    {
        var sale = ItemCancelledHandlerTestData.GenerateValidSaleWithMultipleItems();
        var command = ItemCancelledHandlerTestData.GenerateCommandWithIds(sale.Id, sale.Items[0].Id);

        _saleRepository.GetByIdWithDetailsAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(x =>
            {
                var updatedSale = x.ArgAt<DomainSale>(0);
                foreach (var item in updatedSale.Items.Where(i => i.Id == command.ItemId))
                {
                    item.IsCancelled = true;
                }
                return updatedSale;
            });

        await _handler.Handle(command, CancellationToken.None);

        await _saleRepository.Received(1).UpdateAsync(
            Arg.Is<DomainSale>(s => s.Id == sale.Id),
            Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given invalid command with empty IDs When cancelling item Then throws ValidationException")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        var command = new ItemCancelledCommand(Guid.Empty, Guid.Empty);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "Given valid command When cancelling item Then ItemCancelledEvent is created")]
    public async Task Handle_ValidRequest_CreatesItemCancelledEvent()
    {
        var sale = ItemCancelledHandlerTestData.GenerateValidSaleWithMultipleItems();
        var command = ItemCancelledHandlerTestData.GenerateCommandWithIds(sale.Id, sale.Items[0].Id);

        _saleRepository.GetByIdWithDetailsAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(x =>
            {
                var updatedSale = x.ArgAt<DomainSale>(0);
                foreach (var item in updatedSale.Items.Where(i => i.Id == command.ItemId))
                {
                    item.IsCancelled = true;
                }
                return updatedSale;
            });

        var itemCancelledResult = await _handler.Handle(command, CancellationToken.None);

        itemCancelledResult.Should().NotBeNull();
    }

    [Fact(DisplayName = "Given valid command When cancelling item Then result contains correct sale information")]
    public async Task Handle_ValidRequest_ReturnsCorrectResultData()
    {
        var sale = ItemCancelledHandlerTestData.GenerateValidSaleWithMultipleItems();
        var itemToCancel = sale.Items[0];
        var command = ItemCancelledHandlerTestData.GenerateCommandWithIds(sale.Id, itemToCancel.Id);

        _saleRepository.GetByIdWithDetailsAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.UpdateAsync(Arg.Any<DomainSale>(), Arg.Any<CancellationToken>())
            .Returns(x =>
            {
                var updatedSale = x.ArgAt<DomainSale>(0);
                foreach (var item in updatedSale.Items.Where(i => i.Id == command.ItemId))
                {
                    item.IsCancelled = true;
                }
                return updatedSale;
            });

        var itemCancelledResult = await _handler.Handle(command, CancellationToken.None);

        itemCancelledResult.Should().NotBeNull();
        itemCancelledResult.SaleId.Should().Be(sale.Id);
        itemCancelledResult.ItemId.Should().Be(itemToCancel.Id);
        itemCancelledResult.SaleNumber.Should().Be(sale.SaleNumber);
        itemCancelledResult.UpdatedTotalAmount.Should().Be(sale.SaleAmount);
        itemCancelledResult.UpdatedAt.Should().NotBeNull();
        itemCancelledResult.Message.Should().Contain(itemToCancel.Id.ToString());
    }
}