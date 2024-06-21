using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using tbb.payments.api.Controllers;
using tbb.payments.api.Interfaces;
using tbb.payments.api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace tbb.payments.api.Tests
{
    [TestClass]
    public class RefundControllerTests
    {
        private Mock<IRefundProvider> _refundProviderMock;
        private RefundController _refundController;

        [TestInitialize]
        public void Initialize()
        {
            _refundProviderMock = new Mock<IRefundProvider>();
            _refundController = new RefundController(_refundProviderMock.Object);
        }

        [TestMethod]
        public async Task ProcessRefund_ReturnsSuccess_WhenRefundIsProcessed()
        {
            // Arrange
            var refundDetails = new RefundDetails ();
            _refundProviderMock.Setup(x => x.ProcessRefundAsync(refundDetails)).ReturnsAsync(true);

            // Act
            var result = await _refundController.ProcessRefund(refundDetails);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
