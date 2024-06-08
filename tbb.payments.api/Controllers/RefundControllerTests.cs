using Xunit;
using Moq;
using tbb.payments.api.Interfaces;
using tbb.payments.api.Controllers;
using tbb.payments.api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace tbb.payments.api.Tests
{
    public class RefundControllerTests
    {
        private readonly Mock<IRefundProvider> _mockRefundProvider;
        private readonly Mock<IPaymentRepository> _mockPaymentRepository;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly RefundController _controller;

        public RefundControllerTests()
        {
            _mockRefundProvider = new Mock<IRefundProvider>();
            _mockPaymentRepository = new Mock<IPaymentRepository>();
            _mockEmailService = new Mock<IEmailService>();
            _controller = new RefundController(_mockRefundProvider.Object, _mockPaymentRepository.Object, _mockEmailService.Object);
        }

        [Fact]
        public async Task ProcessRefund_ReturnsOkResult_WhenRefundIsSuccessful()
        {
            // Arrange
            var refundDetails = new RefundDetails { TransactionId = "test_id", Amount = 100, UserId = "user1" };
            _mockRefundProvider.Setup(r => r.ProcessRefundAsync(refundDetails)).ReturnsAsync(true);
            _mockPaymentRepository.Setup(r => r.AddRefundRecordAsync(refundDetails)).ReturnsAsync(true);
            _mockPaymentRepository.Setup(r => r.GetUserEmailByIdAsync(refundDetails.UserId)).ReturnsAsync("test@example.com");
            _mockEmailService.Setup(e => e.SendEmailAsync("test@example.com", "Refund Processed", It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ProcessRefund(refundDetails);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        // Additional tests for other scenarios
    }
}
