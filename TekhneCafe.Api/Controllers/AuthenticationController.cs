using Microsoft.AspNetCore.Mvc;
using TekhneCafe.Api.ActionFilters;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.ValidationRules.FluentValidations.Authentication;
using TekhneCafe.Core.DTOs.Authentication;

namespace TekhneCafe.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Log into account
        /// </summary>
        /// <param name="user">Enter user email and password</param>
        /// <returns>jwt token information</returns>
        /// <response code="200">User successful login</response>
        /// <response code="400">Invalid credentials</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("[action]")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<UserLoginDtoValidator, UserLoginDto>), Arguments = new object[] { "user" })]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            var token = await _authService.Login(user);
            return Ok(token);
        }
    }
}
