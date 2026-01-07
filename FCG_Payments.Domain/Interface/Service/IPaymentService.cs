using FCG_Payments.Domain.Model;

namespace FCG_Payments.Domain.Interface.Service;

public interface IPaymentService
{
    Task<PaymentDto> GetStatus();

    Task<ProcessPaymentDto> ProcessPayment();
}
