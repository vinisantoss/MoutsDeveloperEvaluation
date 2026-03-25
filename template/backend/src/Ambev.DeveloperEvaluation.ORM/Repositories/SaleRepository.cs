using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleRepository using Entity Framework Core
/// </summary>
public class SaleRepository : Repository<Sale>, ISaleRepository
{
    /// <summary>
    /// Initializes a new instance of SaleRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public SaleRepository(DefaultContext context) : base(context) { }

    /// <summary>
    /// Retrieves a sale by its unique identifier, including related Customer, Branch, and Items with Products
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale with all related details if found, null otherwise</returns>
    public async Task<Sale?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
     => await _dbSet
                     .Include(ct => ct.Customer)
                     .Include(ct => ct.Branch)
                     .Include(ct => ct.Items)
                         .ThenInclude(item => item.Product)
                     .FirstOrDefaultAsync(ct => ct.Id == id, cancellationToken);

    /// <summary>
    /// Retrieves a sale by its sale number, including related Customer, Branch, and Items with Products
    /// </summary>
    /// <param name="saleCode">The sale number of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale with all related details if found, null otherwise</returns>
    public async Task<Sale?> GetBySaleNumberAsync(string saleCode, CancellationToken cancellationToken = default)
    {
        return await _dbSet
                      .Include(ct => ct.Customer)
                      .Include(ct => ct.Branch)
                      .Include(ct => ct.Items)
                          .ThenInclude(item => item.Product)
                      .FirstOrDefaultAsync(ct => ct.SaleNumber == saleCode, cancellationToken);
    }
}
