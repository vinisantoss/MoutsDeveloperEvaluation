using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IProductRepository using Entity Framework Core
/// </summary>
public class ProductRepository : Repository<Product>, IProductRepository
{
    /// <summary>
    /// Initializes a new instance of ProductRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public ProductRepository(DefaultContext context) : base(context) { }

    /// <summary>
    /// Retrieves a product by its external identifier
    /// </summary>
    /// <param name="externalId">The external identifier of the product</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product if found, null otherwise</returns>
    public async Task<Product?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default)
      => await _dbSet.FirstOrDefaultAsync(p => p.ExternalId == externalId, cancellationToken);
}
