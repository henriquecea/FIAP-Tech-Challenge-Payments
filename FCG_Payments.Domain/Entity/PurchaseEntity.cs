namespace FCG_Payments.Domain.Entity;

public class PurchaseEntity(Guid userId, Guid gameId, decimal value) : BaseEntity
{
    public Guid UserId { get; set; } = userId;

    public Guid GameId { get; set; } = gameId;

    public decimal Value { get; set; } = value;
}
