using FCG_Payments.Domain.Interface.Repository;
using FCG_Payments.Domain.Interface.Service;
using FCG_Payments.Domain.Model;

namespace FCG_Payments.Application.Service;

public class PaymentService(IPaymentRepository paymentRepository) : IPaymentService
{
    public Task<PaymentDto> GetStatus()
    {
        throw new NotImplementedException();
    }

    public Task<ProcessPaymentDto> ProcessPayment()
    {
        throw new NotImplementedException();
    }
}
