using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation.Sales;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale with business rules and validations
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Unique sale code for identification
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Date when the sale was made
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Customer identification
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Customer reference
    /// </summary>
    public Customer Customer { get; set; } = null!;

    /// <summary>
    /// Branch identification
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Branch reference 
    /// </summary>
    public Branch Branch { get; set; } = null!;

    /// <summary>
    /// Grand total amount of the sale
    /// </summary>
    public decimal SaleAmount { get; set; }

    /// <summary>
    /// Current status of the sale
    /// </summary>
    public SaleStatus Status { get; set; }

    /// <summary>
    /// Collection of items in this sale
    /// </summary>
    public List<SaleItem> Items { get; set; } = new();

    /// <summary>
    /// Date when the sale was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date when the sale was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the Sale class
    /// </summary>
    public Sale()
    {
        CreatedAt = DateTime.UtcNow;
        Status = SaleStatus.Active;
        SaleDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds an item to the sale
    /// </summary>
    /// <param name="productId">Product identifier</param>
    /// <param name="quantity">Quantity to add</param>
    /// <param name="itemPrice">Item price</param>
    public void AddItem(Guid productId, int quantity, decimal itemPrice)
    {
        if (Status is SaleStatus.Cancelled)
            throw new DomainException("Cannot add items to a cancelled sale");

        if (quantity > 20)
            throw new DomainException("Cannot sell more than 20 identical items");

        var existingItem = Items.FirstOrDefault(i => i.ProductId == productId && !i.IsCancelled);

        if (existingItem is not null)
        {
            var newQuantity = existingItem.Quantity + quantity;
            if (newQuantity > 20)
                throw new DomainException("Cannot sell more than 20 identical items");

            existingItem.UpdateQuantity(newQuantity);
        }
        else
        {
            var newItem = SaleItem.Create(productId, quantity, itemPrice);
            newItem.SaleId = Id;
            Items.Add(newItem);
        }

        RecalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the quantity of an existing item
    /// </summary>
    /// <param name="itemId">Item identifier</param>
    /// <param name="newQuantity">New quantity</param>
    public void UpdateItemQuantity(Guid itemId, int newQuantity)
    {
        if (Status is SaleStatus.Cancelled)
            throw new DomainException("Cannot update items in a cancelled sale");

        var item = Items.FirstOrDefault(i => i.Id == itemId && !i.IsCancelled);
        if (item is null)
            throw new DomainException("Item not found or already cancelled");

        item.UpdateQuantity(newQuantity);
        RecalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels a specific item in the sale
    /// </summary>
    /// <param name="itemId">Item identifier</param>
    public void CancelItem(Guid itemId)
    {
        if (Status is SaleStatus.Cancelled)
            throw new DomainException("Cannot cancel items in a cancelled sale");

        var item = Items.FirstOrDefault(i => i.Id == itemId && !i.IsCancelled);
        if (item is null)
            throw new DomainException("Item not found or already cancelled");

        item.Cancel();
        RecalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels the entire sale
    /// </summary>
    public void Cancel()
    {
        Status = SaleStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Recalculates the total amount based on active items
    /// </summary>
    private void RecalculateTotalAmount()
    {
        SaleAmount = Items.Where(i => !i.IsCancelled).Sum(i => i.ItemTotal);
    }

    /// <summary>
    /// Validates the sale using business rules
    /// </summary>
    /// <returns>Validation result</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}