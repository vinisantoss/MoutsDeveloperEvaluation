using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.ItemCancelled;

/// <summary>
/// Validator for ItemCancelledCommand
/// </summary>
public class ItemCancelledValidator : AbstractValidator<ItemCancelledCommand>
{
    public ItemCancelledValidator()
    {
        RuleFor(x => x.SaleId)
           .NotEmpty()
           .WithMessage("sale ID is required")
           .NotEqual(Guid.Empty)
           .WithMessage("sale ID cannot be empty");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item ID is required")
            .NotEqual(Guid.Empty)
            .WithMessage("Item ID cannot be empty");
    }
}

