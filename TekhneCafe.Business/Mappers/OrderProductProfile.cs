using AutoMapper;
using TekhneCafe.Core.DTOs.OrderProduct;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Mappers
{
    public class OrderProductProfile : Profile
    {
        public OrderProductProfile()
        {
            CreateMap<OrderProductAddDto, OrderProduct>();
            CreateMap<OrderProduct, OrderProductListDto>();
            CreateMap<Product, OrderProduct>();
        }
    }
}
