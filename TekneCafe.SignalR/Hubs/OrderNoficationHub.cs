using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TekhneCafe.Core.Consts;

namespace TekneCafe.SignalR.Hubs
{
    [Authorize(Roles = $"{RoleConsts.CafeAdmin}, {RoleConsts.CafeService}")]
    public class OrderNoficationHub : Hub
    {
        public OrderNoficationHub()
        {
        }
    }
}
