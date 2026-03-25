using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.SaleCancelled;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// Profile for mapping between API and Application CancelSale
/// </summary>
public class CancelSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CancelSale feature
    /// </summary>
    public CancelSaleProfile()
    {
        CreateMap<Guid, SaleCancelledCommand>()
            .ConstructUsing(id => new SaleCancelledCommand(id));

        CreateMap<SaleCancelledResult, CancelSaleResponse>();
        CreateMap<CustomerResult, CustomerResponse>();
        CreateMap<BranchResult, BranchResponse>();
        CreateMap<SaleItemResult, SaleItemResponse>();
        CreateMap<ProductResult, ProductResponse>();
    }
}