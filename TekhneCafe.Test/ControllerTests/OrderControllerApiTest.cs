using Microsoft.AspNetCore.Mvc;
using Moq;
using TekhneCafe.Api.Controllers;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Core.DTOs.Order;
using TekhneCafe.Core.Exceptions.Order;

namespace TekhneCafe.Test.ControllerTests
{
    public class OrderControllerApiTest
    {
        Mock<IOrderService> _orderService = new();
        public OrderControllerApiTest()
        {

        }

        [Fact]
        public async Task CreateOrder_WithValidParameters_Returns201StatusCode()
        {
            // Arrange
            OrdersController controller = new(_orderService.Object);

            // Act
            var result = await controller.CreateOrder(new());

            // Assert
            Assert.IsType(typeof(StatusCodeResult), result);
        }

        [Fact]
        public async Task CreateOrder_WithNoProducts_ThrowsOrderBadRequestException()
        {
            // Arrange
            _orderService.Setup(_ => _.CreateOrderAsync(It.IsAny<OrderAddDto>())).ThrowsAsync(new OrderBadRequestException());
            OrdersController controller = new(_orderService.Object);

            // Act
            var result = async () => await controller.CreateOrder(new());

            // Assert
            await Assert.ThrowsAsync(typeof(OrderBadRequestException), result);
        }

        [Fact]
        public async Task ConfirmOrder_WithValidOrderId_ReturnsOkObjectResult()
        {
            // Arrange
            OrdersController controller = new(_orderService.Object);

            // Act
            var result = await controller.ConfirmOrder("234");

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task ConfirmOrder_WithInvalidOrderId_ThrowsOrderNotFoundException()
        {
            // Arrange
            _orderService.Setup(_ => _.ConfirmOrderAsync(It.IsAny<string>())).ThrowsAsync(new OrderNotFoundException());
            OrdersController controller = new(_orderService.Object);

            // Act
            var result = async () => await controller.ConfirmOrder("234");

            // Assert
            await Assert.ThrowsAsync(typeof(OrderNotFoundException), result);
        }

        [Fact]
        public async Task Orders_WithOrderId_ReturnsOrderWithGivenId()
        {
            // Arrange
            OrdersController controller = new(_orderService.Object);

            // Act
            var result = await controller.Orders("");

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Orders_WithInvalidOrderId_ThrowsOrderNotFoundException()
        {
            // Arrange
            _orderService.Setup(_ => _.GetOrderDetailByIdAsync(It.IsAny<string>())).ThrowsAsync(new OrderNotFoundException());
            OrdersController controller = new(_orderService.Object);

            // Act
            var result = async () => await controller.Orders("");

            // Assert
            await Assert.ThrowsAsync(typeof(OrderNotFoundException), result);
        }
    }
}
