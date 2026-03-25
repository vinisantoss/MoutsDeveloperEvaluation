
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.SaleCancelled;

/// <summary>
/// Command for cancelling a sale
/// </summary>
public class SaleCancelledCommand : IRequest<SaleCancelledResult>
{
    /// <summary>
    /// Gets or sets the transaction's unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new SaleCancelledCommand
    /// </summary>
    /// <param name="id">The sale ID</param>
    public SaleCancelledCommand(Guid id)
    {
        Id = id;
    }
}
