using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// AutoMapper profile for GetSale operation mappings
/// </summary>
public class GetSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSale
    /// </summary>
    public GetSaleProfile()
    {
        CreateMap<Sale, GetSaleResult>()
       .ForMember(dest => dest.SaleStatus, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<Customer, CustomerResult>();
        CreateMap<Branch, BranchResult>();
        CreateMap<SaleItem, SaleItemResult>();
        CreateMap<Product, ProductResult>();
    }
}
