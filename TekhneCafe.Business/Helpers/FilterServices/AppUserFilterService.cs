using TekhneCafe.Core.Filters;
using TekhneCafe.Core.Filters.AppUser;
using TekhneCafe.Core.ResponseHeaders;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Helpers.FilterServices
{
    public class AppUserFilterService
    {
        public AppUserResponseFilter<List<AppUser>> FilterAppUsers(IQueryable<AppUser> appUsers, AppUserRequestFilter filters)
        {
            if (filters.Name != null)
                appUsers = appUsers.Where(_ => _.FullName.ToLower().Contains(filters.Name.ToLower()));
            var filteredUserHistory = appUsers.OrderBy(_ => _.FullName).Skip(filters.Page * filters.Size).Take(filters.Size).ToList();
            Metadata metadata = new(filters.Page, filters.Size, appUsers.Count(), appUsers.Count() / filters.Size + 1);
            var header = new CustomHeaders().AddPaginationHeader(metadata);

            return new()
            {
                ResponseValue = filteredUserHistory,
                Headers = header
            };
        }
    }
}
