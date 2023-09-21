using AutoMapper;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Core.DTOs.ProductAttribute;
using TekhneCafe.DataAccess.Abstract;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Concrete
{
    public class ProductAttributeManager : IProductAttributeService
    {
        private readonly IProductAttributeDal _productAttributeDal;
        private readonly IMapper _mapper;

        public ProductAttributeManager(IProductAttributeDal productAttributeDal, IMapper mapper)
        {
            _productAttributeDal = productAttributeDal;
            _mapper = mapper;
        }

        public async Task CreateProductAttributeAsync(ProductAttributeAddDto productAttributeAddDto)
        {
            ProductAttribute productAttribute = _mapper.Map<ProductAttribute>(productAttributeAddDto);
            await _productAttributeDal.AddAsync(productAttribute);
        }
    }
}
