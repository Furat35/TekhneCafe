using Microsoft.AspNetCore.Mvc;
using Moq;
using TekhneCafe.Api.Controllers;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Core.DTOs.Payment;
using TekhneCafe.Core.Exceptions.AppUser;

namespace TekhneCafe.Test.ControllerTests
{
    public class PaymentControllerApiTest
    {
        Mock<IPaymentService> _paymentServiceMock = new();

        [Fact]
        public async Task Pay_WithValidArguments_ReturnsOk()
        {
            // Arrange
            PaymentController controller = new(_paymentServiceMock.Object);

            // Act 
            var result = await controller.Pay(null);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Pay_WithInvalidUserId_ThrowsAppUserNotFoundException()
        {
            // Arrange
            _paymentServiceMock.Setup(_ => _.PayAsync(It.IsAny<PaymentDto>())).ThrowsAsync(new AppUserNotFoundException());
            PaymentController controller = new(_paymentServiceMock.Object);

            // Act 
            var result = async () => await controller.Pay(null);

            // Assert
            await Assert.ThrowsAsync(typeof(AppUserNotFoundException), result);
        }
    }
}
