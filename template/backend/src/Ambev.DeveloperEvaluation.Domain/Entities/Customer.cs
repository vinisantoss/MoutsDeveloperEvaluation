using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Validation.Sales;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a customer using External Identity pattern.
/// </summary>
public class Customer : BaseEntity
{
    /// <summary>
    /// External identifier from the Customer
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Customer name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Customer email.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Customer document.
    /// </summary>
    public string Document { get; set; } = string.Empty;

    /// <summary>
    /// Performs validation of the customer entity using the CustomerValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    public ValidationResultDetail Validate()
    {
        var validator = new CustomerValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
