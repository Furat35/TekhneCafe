using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.Helpers.FilterServices;
using TekhneCafe.Business.Helpers.HeaderServices;
using TekhneCafe.Core.DTOs.Attribute;
using TekhneCafe.Core.Exceptions.Attribute;
using TekhneCafe.Core.Filters.Attribute;
using TekhneCafe.DataAccess.Abstract;
using ProductAttributes = TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Concrete
{
    public class AttributeManager : IAttributeService
    {
        private readonly IAttributeDal _attributeDal;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public AttributeManager(IAttributeDal attributeDal, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _attributeDal = attributeDal;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public async Task CreateAttributeAsync(AttributeAddDto attributeAddDto)
        {
            ProductAttributes.Attribute attribute = _mapper.Map<ProductAttributes.Attribute>(attributeAddDto);
            await _attributeDal.AddAsync(attribute);

        }
        public List<AttributeDetailDto> GetAllAttributes(AttributeRequestFilter filters)
        {
            var filteredResult = FilterAttributes(filters);
            return _mapper.Map<List<AttributeDetailDto>>(filteredResult.ResponseValue);
        }

        public async Task<AttributeDetailDto> GetAttributeByIdAsync(string id)
        {
            ProductAttributes.Attribute attribute = await _attributeDal.GetByIdAsync(Guid.Parse(id));
            ThrowErrorIfAttributeNotFound(attribute);
            return _mapper.Map<AttributeDetailDto>(attribute);

        }

        public async Task DeleteAttributeAsync(string id)
        {
            ProductAttributes.Attribute attribute = await _attributeDal.GetByIdAsync(Guid.Parse(id));
            ThrowErrorIfAttributeNotFound(attribute);
            attribute.IsDeleted = true;
            await _attributeDal.SafeDeleteAsync(attribute);
        }

        public async Task UpdateAttributeAsync(AttributeUpdateDto attributeUpdateDto)
        {
            ProductAttributes.Attribute attribute = await _attributeDal.GetByIdAsync(Guid.Parse(attributeUpdateDto.Id));
            ThrowErrorIfAttributeNotFound(attribute);
            _mapper.Map(attributeUpdateDto, attribute);
            await _attributeDal.UpdateAsync(attribute);
        }

        private static void ThrowErrorIfAttributeNotFound(ProductAttributes.Attribute attribute)
        {
            if (attribute is null)
                throw new AttributeNotFoundException();
        }

        private AttributeResponseFilter<List<ProductAttributes.Attribute>> FilterAttributes(AttributeRequestFilter filters)
        {
            var query = GetNotDeletedAttributes();
            var filteredResult = new AttributeFilterService().FilterAttribute(query, filters);
            new HeaderService(_httpContext).AddToHeaders(filteredResult.Headers);
            return filteredResult;
        }

        private IQueryable<ProductAttributes.Attribute> GetNotDeletedAttributes()
          => _attributeDal.GetAll(_ => !_.IsDeleted).AsNoTracking().AsSplitQuery();
    }
}
