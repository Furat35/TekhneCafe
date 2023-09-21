using AutoMapper;
using TekhneCafe.Core.DTOs.OrderProductAttribute;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Mappers
{
    public class OrderProductAttributeProfile : Profile
    {
        public OrderProductAttributeProfile()
        {
            CreateMap<OrderProductAttributeAddDto, OrderProductAttribute>();
            CreateMap<OrderProductAttribute, OrderProductAttributeListDto>();
        }
    }
}
