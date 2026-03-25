using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.Common;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleRepository : IRepository<Sale>
{
    /// <summary>
    /// Retrieves a sale by its unique identifier, eagerly loading Customer, Branch, and Items with Products.
    /// </summary>
    Task<Sale> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale by its sale number
    /// </summary>
    /// <param name="transactionCode">The sale number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    Task<Sale> GetBySaleNumberAsync(string transactionCode, CancellationToken cancellationToken = default);
}
