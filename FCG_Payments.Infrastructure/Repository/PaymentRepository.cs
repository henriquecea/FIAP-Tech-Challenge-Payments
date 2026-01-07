using FCG_Payments.Domain.Entity;
using FCG_Payments.Domain.Interface.Repository;
using FCG_Payments.Infrastructure.Data;

namespace FCG_Payments.Infrastructure.Repository;

public class PaymentRepository(AppDbContext context) : Repository<PurchaseEntity>(context), IPaymentRepository
{

}
