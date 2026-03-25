using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.Common;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Customer entity operations
/// </summary>
public interface ICustomerRepository : IRepository<Customer>
{
    /// <summary>
    /// Retrieves a Customer by its external identifier
    /// </summary>
    Task<Customer> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default);
}
