using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleItemTests
{
    [Fact(DisplayName = "SaleItem should be created with valid data")]
    public void Given_ValidData_When_Creating_Then_SaleItemShouldBeCreated()
    {
        var productId = Guid.NewGuid();
        var quantity = 5;
        var itemPrice = 100m;

        var item = SaleItem.Create(productId, quantity, itemPrice);

        item.ProductId.Should().Be(productId);
        item.Quantity.Should().Be(quantity);
        item.ItemPrice.Should().Be(itemPrice);
        item.IsCancelled.Should().BeFalse();
        item.Id.Should().NotBe(Guid.Empty);
    }

    [Fact(DisplayName = "SaleItem should calculate 10% discount for 4+ items")]
    public void Given_FourItems_When_CalculatingDiscount_Then_DiscountShouldBeTenPercent()
    {
        var item = SaleItem.Create(Guid.NewGuid(), 4, 100m);

        item.DiscountPercentage.Should().Be(10m);
    }

    [Fact(DisplayName = "SaleItem should calculate 20% discount for 10+ items")]
    public void Given_TwentyItems_When_CalculatingDiscount_Then_DiscountShouldBeTwentyPercent()
    {
        var item = SaleItem.Create(Guid.NewGuid(), 10, 100m);

        item.DiscountPercentage.Should().Be(20m);
    }

    [Fact(DisplayName = "SaleItem should calculate no discount for less than 4 items")]
    public void Given_ThreeItems_When_CalculatingDiscount_Then_DiscountShouldBeZero()
    {
        var item = SaleItem.Create(Guid.NewGuid(), 3, 100m);

        item.DiscountPercentage.Should().Be(0m);
    }

    [Fact(DisplayName = "SaleItem should calculate correct total with discount")]
    public void Given_SaleItemWithDiscount_When_CalculatingTotal_Then_TotalShouldIncludeDiscount()
    {
        var item = SaleItem.Create(Guid.NewGuid(), 10, 100m);

        var expectedTotal = 10 * 100m * (1 - 20m / 100);
        item.ItemTotal.Should().Be(expectedTotal);
    }

    [Fact(DisplayName = "SaleItem should update quantity successfully")]
    public void Given_SaleItem_When_UpdatingQuantity_Then_QuantityShouldBeUpdated()
    {
        var item = SaleItem.Create(Guid.NewGuid(), 5, 100m);

        item.UpdateQuantity(10);

        item.Quantity.Should().Be(10);
    }

    [Fact(DisplayName = "SaleItem should not allow quantity greater than 20")]
    public void Given_SaleItem_When_UpdatingQuantityToMoreThanTwenty_Then_ShouldThrowException()
    {
        var item = SaleItem.Create(Guid.NewGuid(), 5, 100m);

        var act = () => item.UpdateQuantity(21);

        act.Should().Throw<DomainException>();
    }

    [Fact(DisplayName = "SaleItem should not allow quantity less than or equal to zero")]
    public void Given_SaleItem_When_UpdatingQuantityToZero_Then_ShouldThrowException()
    {
        var item = SaleItem.Create(Guid.NewGuid(), 5, 100m);

        var act = () => item.UpdateQuantity(0);

        act.Should().Throw<DomainException>();
    }

    [Fact(DisplayName = "SaleItem should be cancelled successfully")]
    public void Given_SaleItem_When_Cancelled_Then_IsCancelledShouldBeTrue()
    {
        var item = SaleItem.Create(Guid.NewGuid(), 5, 100m);

        item.Cancel();

        item.IsCancelled.Should().BeTrue();
    }

    [Fact(DisplayName = "SaleItem should validate successfully with valid data")]
    public void Given_ValidSaleItem_When_Validated_Then_ShouldReturnValid()
    {
        var item = SaleItem.Create(Guid.NewGuid(), 5, 100m);

        var result = item.Validate();

        result.IsValid.Should().BeTrue();
    }
}