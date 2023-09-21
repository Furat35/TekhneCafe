using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TekhneCafe.Api.ActionFilters;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.ValidationRules.FluentValidations.Order;
using TekhneCafe.Core.Consts;
using TekhneCafe.Core.DTOs.Order;
using TekhneCafe.Core.Filters.Order;

namespace TekhneCafe.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Create new order
        /// </summary>
        /// <param name="orderDto">Order parameters</param>
        /// <returns></returns>
        /// <response code="201">Order created</response>
        /// <response code="400">Invalid order</response>
        /// <response code="500">Server error</response>
        [HttpPost("create")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<OrderAddDtoValidator, OrderAddDto>), Arguments = new object[] { "order" })]
        public async Task<IActionResult> CreateOrder([FromBody] OrderAddDto order)
        {
            await _orderService.CreateOrderAsync(order);
            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Confirm Order
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Order not found</response>
        /// <response code="500">Server error</response>
        [HttpPost("confirm")]
        [Authorize(Roles = $"{RoleConsts.CafeService}, {RoleConsts.CafeAdmin}")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = new object[] { "id" })]
        public async Task<IActionResult> ConfirmOrder([FromQuery] string id)
        {
            await _orderService.ConfirmOrderAsync(id);
            return Ok();
        }

        /// <summary>
        /// Get Order By Id
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns>Order with given id</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Order not found</response>
        /// <response code="500">Server error</response>
        [HttpGet("{id}")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = new object[] { "id" })]
        public async Task<IActionResult> Orders([FromRoute] string id)
        {
            var order = await _orderService.GetOrderDetailByIdAsync(id);
            return Ok(order);
        }

        /// <summary>
        /// Get All Orders
        /// </summary>
        /// <returns>Order with given id</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Order not found</response>
        /// <response code="500">Server error</response>
        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.CafeService}, {RoleConsts.CafeAdmin}")]
        public IActionResult Orders([FromQuery] OrderRequestFilter filters)
        {
            var orders = _orderService.GetOrders(filters);
            return Ok(orders);
        }
    }
}
