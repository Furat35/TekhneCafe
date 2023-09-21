using AutoMapper;
using TekhneCafe.Core.DTOs.ProductAttribute;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Mappers
{
    public class ProductAttributeProfile : Profile
    {
        public ProductAttributeProfile()
        {
            CreateMap<ProductAttribute, OrderProductAttribute>();
            CreateMap<ProductAttributeAddDto, ProductAttribute>();
            CreateMap<ProductAttribute, ProductAttributeListDto>();
            CreateMap<ProductAttribute, ProductAttributeDetailDto>();
            CreateMap<ProductAttributeUpdateDto, ProductAttribute>();
        }
    }
}
