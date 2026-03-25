using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.SaleCreated;

/// <summary>
/// AutoMapper profile for sale operation mappings
/// </summary>
public class CreateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateSale
    /// </summary>
    public CreateSaleProfile()
    {
        CreateMap<Sale, CreateSaleResult>()
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<Customer, CustomerResult>();
        CreateMap<Branch, BranchResult>();
        CreateMap<SaleItem, SaleItemResult>();
        CreateMap<Product, ProductResult>();
    }
   
}
