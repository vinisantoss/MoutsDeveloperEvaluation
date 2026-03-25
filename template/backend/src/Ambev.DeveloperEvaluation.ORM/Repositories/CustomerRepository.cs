using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ICustomerRepository using Entity Framework Core
/// </summary>
public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    /// <summary>
    /// Initializes a new instance of CustomerRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public CustomerRepository(DefaultContext context) : base(context) { }

    /// <summary>
    /// Retrieves a customer by their external identifier
    /// </summary>
    /// <param name="externalId">The external identifier of the customer</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The customer if found, null otherwise</returns>
    public async Task<Customer?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default)
     => await _dbSet.FirstOrDefaultAsync(ou => ou.ExternalId == externalId, cancellationToken);
   
}
