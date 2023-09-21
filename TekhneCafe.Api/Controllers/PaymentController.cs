using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TekhneCafe.Api.ActionFilters;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.ValidationRules.FluentValidations.Payment;
using TekhneCafe.Core.Consts;
using TekhneCafe.Core.DTOs.Payment;

namespace TekhneCafe.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Payment for selected user.
        /// </summary>
        /// <param name="payment">Payment details</param>
        /// <returns>OK result</returns>
        /// <response code="200">Success</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Server error</response>
        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.CafeAdmin}, {RoleConsts.CafeService}")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<PaymentDtoValidator, PaymentDto>), Arguments = new object[] { "payment" })]
        public async Task<IActionResult> Pay([FromBody] PaymentDto payment)
        {
            await _paymentService.PayAsync(payment);
            return Ok();
        }
    }
}
