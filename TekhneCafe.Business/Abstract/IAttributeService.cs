using TekhneCafe.Core.DTOs.Attribute;
using TekhneCafe.Core.Filters.Attribute;

namespace TekhneCafe.Business.Abstract
{
    public interface IAttributeService
    {
        Task CreateAttributeAsync(AttributeAddDto attributeAddDto);
        Task DeleteAttributeAsync(string id);
        List<AttributeDetailDto> GetAllAttributes(AttributeRequestFilter filters);
        Task<AttributeDetailDto> GetAttributeByIdAsync(string id);
        Task UpdateAttributeAsync(AttributeUpdateDto attributeUpdateDto);
    }
}
