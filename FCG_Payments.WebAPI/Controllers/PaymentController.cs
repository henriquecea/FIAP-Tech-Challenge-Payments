using FCG_Payments.Domain.Interface.Service;
using FCG_Payments.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace FCG_Payments.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController(IPaymentService paymentService) : ControllerBase
{
    [HttpGet()]
    public async Task<PaymentDto> GetStatus() =>
        await paymentService.GetStatus();

    [HttpPost()]
    public async Task<ProcessPaymentDto> ProcessPayment() =>
        await paymentService.ProcessPayment();
}
