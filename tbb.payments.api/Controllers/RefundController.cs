using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using tbb.payments.api.Interfaces;
using tbb.payments.api.Models;

namespace tbb.payments.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundController : ControllerBase
    {
        private readonly IRefundProvider _refundProvider;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IEmailService _emailService;

        public RefundController(IRefundProvider refundProvider, IPaymentRepository paymentRepository, IEmailService emailService)
        {
            _refundProvider = refundProvider;
            _paymentRepository = paymentRepository;
            _emailService = emailService;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessRefund([FromBody] RefundDetails refundDetails)
        {
            if (refundDetails == null)
            {
                return BadRequest();
            }

            var result = await _refundProvider.ProcessRefundAsync(refundDetails);
            if (result)
            {
                await _paymentRepository.AddRefundRecordAsync(refundDetails);

                // Notify user
                var userEmail = await _paymentRepository.GetUserEmailByIdAsync(refundDetails.UserId);
                if (!string.IsNullOrEmpty(userEmail))
                {
                    await _emailService.SendEmailAsync(userEmail, "Refund Processed", "Your refund has been processed successfully.");
                }

                return Ok(new { message = "Refund processed successfully.", refundDetails });
            }

            return StatusCode(500, "An error occurred while processing the refund.");
        }
    }
}
