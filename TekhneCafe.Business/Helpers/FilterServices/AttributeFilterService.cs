using TekhneCafe.Core.Filters;
using TekhneCafe.Core.Filters.Attribute;
using TekhneCafe.Core.ResponseHeaders;
using ProductAttributes = TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Helpers.FilterServices
{
    public class AttributeFilterService
    {
        public AttributeResponseFilter<List<ProductAttributes.Attribute>> FilterAttribute(IQueryable<ProductAttributes.Attribute> attributes, AttributeRequestFilter filters)
        {
            var filteredAttributes = attributes.OrderBy(_ => _.Name).Skip(filters.Page * filters.Size).Take(filters.Size).ToList();
            Metadata metadata = new(filters.Page, filters.Size, attributes.Count(), attributes.Count() / filters.Size + 1);
            var header = new CustomHeaders().AddPaginationHeader(metadata);
            return new()
            {
                ResponseValue = filteredAttributes,
                Headers = header
            };
        }
    }
}
