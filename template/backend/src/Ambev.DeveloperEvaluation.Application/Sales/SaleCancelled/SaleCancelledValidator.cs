using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.SaleCancelled;

/// <summary>
/// Validator for SaleCancelledValidator
/// </summary>
public class SaleCancelledValidator : AbstractValidator<SaleCancelledCommand>
{
    /// <summary>
    /// Initializes a new SaleCancelledValidator
    /// </summary>
    public SaleCancelledValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage("Sale ID is required")
           .NotEqual(Guid.Empty)
           .WithMessage("Sale ID cannot be empty");
    }
}
