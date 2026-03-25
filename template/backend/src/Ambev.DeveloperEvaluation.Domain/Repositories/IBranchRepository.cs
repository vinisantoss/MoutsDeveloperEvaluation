using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.Common;


namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Branch entity operations
/// </summary>
public interface IBranchRepository : IRepository<Branch>
{
    /// <summary>
    /// Retrieves an Branch by its external identifier
    /// </summary>
    Task<Branch> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default);
}

