using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IBranchRepository using Entity Framework Core
/// </summary>
public class BranchRepository : Repository<Branch>, IBranchRepository
{
    /// <summary>
    /// Initializes a new instance of ProductRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public BranchRepository(DefaultContext context) : base(context) { }

    /// <summary>
    /// Retrieves a branch by its external identifier
    /// </summary>
    /// <param name="externalId">The external identifier of the branch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product if found, null otherwise</returns>
    public async Task<Branch?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(ou => ou.ExternalId == externalId, cancellationToken);
    }
}
