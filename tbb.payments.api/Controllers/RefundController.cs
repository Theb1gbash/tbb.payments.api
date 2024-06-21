using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using tbb.payments.api.Interfaces;
using tbb.payments.api.Models;

namespace tbb.payments.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefundController : ControllerBase
    {
        private readonly IRefundProvider _refundProvider;

        public RefundController(IRefundProvider refundProvider)
        {
            _refundProvider = refundProvider;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessRefund([FromBody] RefundDetails refundDetails)
        {
            try
            {
                var result = await _refundProvider.ProcessRefundAsync(refundDetails);
                if (result)
                {
                    return Ok(new { Message = "Refund processed successfully." });
                }
                return BadRequest(new { Message = "Refund processing failed." });
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
