using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Validator for GetSaleCommand
/// </summary>
public class GetSaleValidator : AbstractValidator<GetSaleCommand>
{
    public GetSaleValidator()
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("ID is required")
        .NotEqual(Guid.Empty)
        .WithMessage("ID cannot be empty");
    }
}
