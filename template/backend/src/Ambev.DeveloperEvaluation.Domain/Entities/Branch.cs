using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation.Sales;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an Branch using External Identity pattern
/// </summary>
public class Branch : BaseEntity
{
    /// <summary>
    /// External identifier from the Branch
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Branch name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Branch location
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Performs validation of the Branch entity using the BranchValidator rules.
    /// </summary>
    public ValidationResultDetail Validate()
    {
        var validator = new BranchValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
