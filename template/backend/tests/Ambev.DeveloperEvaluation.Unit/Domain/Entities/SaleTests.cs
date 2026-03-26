using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    [Fact(DisplayName = "Sale should be created with Active status")]
    public void Given_NewSale_When_Created_Then_StatusShouldBeActive()
    {
        var sale = SaleTestData.GenerateValidSale();

        sale.Status.Should().Be(SaleStatus.Active);
    }

    [Fact(DisplayName = "Sale should add item successfully")]
    public void Given_ValidSale_When_AddingItem_Then_ItemShouldBeAdded()
    {
        var sale = SaleTestData.GenerateValidSale();
        var productId = Guid.NewGuid();
        var initialCount = sale.Items.Count;

        sale.AddItem(productId, 5, 100m);

        sale.Items.Count.Should().Be(initialCount + 1);
        sale.Items.Last().ProductId.Should().Be(productId);
        sale.Items.Last().Quantity.Should().Be(5);
    }

    [Fact(DisplayName = "Sale should apply 10% discount for 4+ items")]
    public void Given_ValidSale_When_AddingFourItems_Then_DiscountShouldBeTenPercent()
    {
        var sale = SaleTestData.GenerateValidSale();
        var productId = Guid.NewGuid();

        sale.AddItem(productId, 4, 100m);

        var item = sale.Items.First();
        item.DiscountPercentage.Should().Be(10m);
    }

    [Fact(DisplayName = "Sale should apply 20% discount for 10+ items")]
    public void Given_ValidSale_When_AddingTwentyItems_Then_DiscountShouldBeTwentyPercent()
    {
        var sale = SaleTestData.GenerateValidSale();
        var productId = Guid.NewGuid();

        sale.AddItem(productId, 10, 100m);

        var item = sale.Items.First();
        item.DiscountPercentage.Should().Be(20m);
    }

    [Fact(DisplayName = "Sale should not allow adding more than 20 items")]
    public void Given_ValidSale_When_AddingMoreThanTwentyItems_Then_ShouldThrowException()
    {
        var sale = SaleTestData.GenerateValidSale();
        var productId = Guid.NewGuid();

        var act = () => sale.AddItem(productId, 21, 100m);

        act.Should().Throw<DomainException>()
            .WithMessage("*20*");
    }

    [Fact(DisplayName = "Sale should not allow adding items to cancelled sale")]
    public void Given_CancelledSale_When_AddingItem_Then_ShouldThrowException()
    {
        var sale = SaleTestData.GenerateValidSale();
        sale.Cancel();

        var act = () => sale.AddItem(Guid.NewGuid(), 5, 100m);

        act.Should().Throw<DomainException>();
    }

    [Fact(DisplayName = "Sale should update item quantity successfully")]
    public void Given_SaleWithItem_When_UpdatingQuantity_Then_QuantityShouldBeUpdated()
    {
        var sale = SaleTestData.GenerateValidSale();
        sale.AddItem(Guid.NewGuid(), 5, 100m);
        var itemId = sale.Items[0].Id;

        sale.UpdateItemQuantity(itemId, 10);

        sale.Items[0].Quantity.Should().Be(10);
    }

    [Fact(DisplayName = "Sale should cancel item successfully")]
    public void Given_SaleWithItem_When_CancellingItem_Then_ItemShouldBeCancelled()
    {
        var sale = SaleTestData.GenerateValidSale();
        sale.AddItem(Guid.NewGuid(), 5, 100m);
        var itemId = sale.Items[0].Id;

        sale.CancelItem(itemId);

        sale.Items[0].IsCancelled.Should().BeTrue();
    }

    [Fact(DisplayName = "Sale should cancel all items when cancelled")]
    public void Given_SaleWithItems_When_Cancelled_Then_StatusShouldBeCancelled()
    {
        var sale = SaleTestData.GenerateValidSale();
        sale.AddItem(Guid.NewGuid(), 5, 100m);
        sale.AddItem(Guid.NewGuid(), 3, 50m);

        sale.Cancel();

        sale.Status.Should().Be(SaleStatus.Cancelled);
    }

    [Fact(DisplayName = "Sale should recalculate total after item cancellation")]
    public void Given_SaleWithMultipleItems_When_CancellingItem_Then_TotalShouldBeRecalculated()
    {
        var sale = SaleTestData.GenerateValidSale();
        sale.AddItem(Guid.NewGuid(), 5, 100m);
        sale.AddItem(Guid.NewGuid(), 5, 100m);
        var originalTotal = sale.SaleAmount;
        var itemIdToCancel = sale.Items[0].Id;

        sale.CancelItem(itemIdToCancel);

        sale.SaleAmount.Should().BeLessThan(originalTotal);
    }

    [Fact(DisplayName = "Validation should pass for valid sale")]
    public void Given_ValidSale_When_Validated_Then_ShouldReturnValid()
    {
        var sale = SaleTestData.GenerateValidSaleWithItems();
        var result = sale.Validate();

        if (!result.IsValid)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Detail));
            throw new Xunit.Sdk.XunitException($"Validation failed: {errors}");
        }

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Sale should not allow updating item in cancelled sale")]
    public void Given_CancelledSale_When_UpdatingItem_Then_ShouldThrowException()
    {
        var sale = SaleTestData.GenerateValidSale();
        sale.AddItem(Guid.NewGuid(), 5, 100m);
        var itemId = sale.Items[0].Id;
        sale.Cancel();

        var act = () => sale.UpdateItemQuantity(itemId, 10);

        act.Should().Throw<DomainException>();
    }
}