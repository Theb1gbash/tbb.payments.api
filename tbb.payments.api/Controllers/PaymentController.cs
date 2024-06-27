using Microsoft.AspNetCore.Mvc;
using tbb.payments.api.Interfaces;
using tbb.payments.api.Models; // Add this line to import the Models namespace
using System.Threading.Tasks;

namespace tbb.payments.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentProvider _paymentProvider;

        public PaymentController(IPaymentProvider paymentProvider)
        {
            _paymentProvider = paymentProvider;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDetails paymentDetails)
        {
            var result = await _paymentProvider.ProcessPaymentAsync(paymentDetails);
            if (result)
            {
                return Ok(new { Message = "Payment processed successfully." });
            }
            return BadRequest(new { Message = "Payment processing failed." });
        }

    }
}
