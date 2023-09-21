using System.Text.Json;
using TekhneCafe.Core.Filters;

namespace TekhneCafe.Core.ResponseHeaders
{
    public class CustomHeaders
    {
        public Dictionary<string, string> AddPaginationHeader(Metadata metadata)
            => metadata != null
            ? new Dictionary<string, string>() { { "X-Pagination", JsonSerializer.Serialize(metadata) } }
            : throw new Exception();
    }
}
