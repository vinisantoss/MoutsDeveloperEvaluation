using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.SaleCreated;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Profile for mapping between API and Application CreateSale
/// </summary>
public class CreateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateSale feature
    /// </summary>
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleRequest, CreateSaleCommand>();
        CreateMap<CustomerRequest, Customer>();
        CreateMap<BranchRequest, Branch>();
        CreateMap<CreateSaleItemRequest, SaleItem>();
        CreateMap<ProductRequest, Product>();

        CreateMap<CreateSaleResult, CreateSaleResponse>();
        CreateMap<CustomerResult, CustomerResponse>();
        CreateMap<BranchResult, BranchResponse>();
        CreateMap<SaleItemResult, SaleItemResponse>();
        CreateMap<ProductResult, ProductResponse>();
    }
}