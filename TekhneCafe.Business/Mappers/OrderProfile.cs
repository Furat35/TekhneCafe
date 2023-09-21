using AutoMapper;
using TekhneCafe.Core.DTOs.Order;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDetailDto>();
            CreateMap<OrderAddDto, Order>();
            CreateMap<Order, OrderListDto>();
        }
    }
}
