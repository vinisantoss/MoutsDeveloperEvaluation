using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Command for retrieving a sale by ID
/// </summary>
public class GetSaleCommand : IRequest<GetSaleResult>
{
    /// <summary>
    /// Gets or sets the sale's unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Validates the command using the validator
    /// </summary>
    /// <returns>Validation result</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new GetSaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}