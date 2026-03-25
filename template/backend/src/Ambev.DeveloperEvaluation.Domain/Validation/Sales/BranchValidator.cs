using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation.Sales;

/// <summary>
/// Validator for Branch entity
/// </summary>
public class BranchValidator : AbstractValidator<Branch>
{
    /// <summary>
    /// Initializes validation rules for Branch
    /// </summary>
    public BranchValidator()
    {
        RuleFor(x => x.ExternalId)
           .NotEmpty()
           .WithMessage("External ID is required")
           .MaximumLength(50)
           .WithMessage("External ID must not exceed 50 characters");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Location)
            .NotEmpty()
            .WithMessage("Location is required")
            .MaximumLength(200)
            .WithMessage("Location must not exceed 200 characters");
    }
}
