using FCG_Payments.Domain.Interface.Service;
using FCG_Payments.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace FCG_Payments.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController(IPaymentService paymentService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var game = await paymentService.GetByIdAsync(id);
        if (game is null) return NotFound();
        return Ok(game);
    }
}

