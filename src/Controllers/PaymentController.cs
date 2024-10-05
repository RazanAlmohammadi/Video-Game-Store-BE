using FusionTech.src.Services.Payment;
using FusionTech.src.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FusionTech.src.DTO.PaymentDTO;

namespace FusionTech.src.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class PaymentController : ControllerBase
    {
        protected readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        [Authorize(Roles = "EmployeeOrAdmin")]
        public async Task<ActionResult<PaymentReadDto>> CreateOne(
            [FromBody] PaymentCreateDto createDto
        )
        {
            var paymentCreated = await _paymentService.CreateOneAsync(createDto);
            return Created($"api/v1/payment/{paymentCreated.PaymentId}", paymentCreated);
            // return Ok(paymentCreated);
        }

        [HttpGet]
        public async Task<ActionResult<PaymentReadDto>> GetPayment(
            PaginationOptions paginationOptions
        )
        {
            var paymentList = await _paymentService.GetAllAsync(paginationOptions);
            return Ok(paymentList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentReadDto>> GetPaymentById([FromRoute] Guid id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            return Ok(payment);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "EmployeeOrAdmin")]
        public async Task<ActionResult<PaymentReadDto>> DeleteOne([FromRoute] Guid id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            await _paymentService.DeleteOneAsync(payment.PaymentId);
            return Ok(payment);
        }
    }
}
