using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class SaleItemTestData
{
    public static SaleItem GenerateValidSaleItem()
    {
        return SaleItem.Create(Guid.NewGuid(), 5, 100m);
    }

    public static SaleItem GenerateSaleItemWithTenPercentDiscount()
    {
        return SaleItem.Create(Guid.NewGuid(), 4, 100m);
    }

    public static SaleItem GenerateSaleItemWithTwentyPercentDiscount()
    {
        return SaleItem.Create(Guid.NewGuid(), 10, 100m);
    }

    public static SaleItem GenerateSaleItemWithMaxQuantity()
    {
        return SaleItem.Create(Guid.NewGuid(), 20, 100m);
    }

    public static SaleItem GenerateCancelledSaleItem()
    {
        var item = GenerateValidSaleItem();
        item.Cancel();

        return item;
    }

    public static List<SaleItem> GenerateValidSaleItems(int count = 3)
    {
        return Enumerable.Range(0, count)
            .Select(_ => GenerateValidSaleItem())
            .ToList();
    }
}