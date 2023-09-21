using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TekhneCafe.Api.ActionFilters;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.Consts;
using TekhneCafe.Core.Consts;
using TekhneCafe.Core.Filters.AppUser;

namespace TekhneCafe.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IAppUserService _userService;

        public UsersController(IAppUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Payment for selected user.
        /// </summary>
        /// <param name="payment">Payment details</param>
        /// <returns>Users list</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Server error</response>
        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.CafeAdmin}, {RoleConsts.CafeService}")]
        public IActionResult GetUsers([FromQuery] AppUserRequestFilter filters)
        {
            var users = _userService.GetUserList(filters);
            return Ok(users);
        }

        /// <summary>
        /// Returns user with given user id.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User with given id</returns>
        /// <response code="200">Success</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Server error</response>
        [HttpGet("{id}")]
        [Authorize(Roles = $"{RoleConsts.CafeAdmin}, {RoleConsts.CafeService}")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = new object[] { "id" })]
        public async Task<IActionResult> GetUsers([FromRoute] string id)
        {
            var users = await _userService.GetUserByIdAsync(id);
            return Ok(users);
        }

        /// <summary>
        /// Update user phone.
        /// </summary>
        /// <param name="phone">New user phone number</param>
        /// <returns>OK result</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Server error</response>
        [HttpPost("updatephone")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = new object[] { "phone" })]
        public async Task<IActionResult> UpdatePhone([FromBody] string phone)
        {
            await _userService.UpdateUserPhoneAsync(phone);
            return Ok();
        }

        /// <summary>
        /// Update user image.
        /// </summary>
        /// <returns>image path</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Server error</response>
        [HttpPost("updateimage")]
        public async Task<IActionResult> UpdateImage()
        {
            string imagePath = await _userService.UpdateUserImageAsync(MinioBuckets.UserImage);
            return Ok(imagePath);
        }
    }
}
