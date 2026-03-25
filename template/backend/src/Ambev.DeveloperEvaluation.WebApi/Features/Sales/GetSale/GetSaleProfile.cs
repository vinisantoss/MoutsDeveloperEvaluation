using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Profile for mapping between Application and API GetSale responses
/// </summary>
public class GetSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSale feature
    /// </summary>
    public GetSaleProfile()
    {
        CreateMap<Guid, GetSaleCommand>()
            .ConstructUsing(id => new GetSaleCommand { Id = id });

        CreateMap<GetSaleResult, GetSaleResponse>();
        CreateMap<CustomerResult, CustomerResponse>();
        CreateMap<BranchResult, BranchResponse>();
        CreateMap<ProductResult, ProductResponse>();
        CreateMap<SaleItemResult, SaleItemResponse>();
    }
}