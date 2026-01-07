using FCG_Payments.Domain.Entity;

namespace FCG_Payments.Domain.Interface.Repository;

public interface IPaymentRepository : IRepository<PurchaseEntity>
{
}
