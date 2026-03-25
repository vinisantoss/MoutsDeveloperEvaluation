using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.SaleModified;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Profile for mapping between API and Application UpdateSale
/// </summary>
public class UpdateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateSale feature
    /// </summary>
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleRequest, SaleModifiedCommand>();
        CreateMap<UpdateSaleItemRequest, SaleItem>();
        CreateMap<ProductInfoRequest, ProductInfo>();

        CreateMap<SaleModifiedResult, UpdateSaleResponse>();
        CreateMap<CustomerResult, CustomerResponse>();
        CreateMap<BranchResult, BranchResponse>();
        CreateMap<SaleItemResult, SaleItemResponse>();
        CreateMap<ProductResult, ProductResponse>();
    }
}