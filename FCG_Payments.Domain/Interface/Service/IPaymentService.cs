using FCG_Payments.Domain.Model;
using FCG_Payments.Domain.Model.Response;

namespace FCG_Payments.Domain.Interface.Service;

public interface IPaymentService
{
    Task<GameResponse?> GetByIdAsync(Guid id);

    Task<OperationResult> ProcessPaymentAsync(GameResponse game, CancellationToken ct);

    Task<GameResponse> CreateAsync(GameResponse game);
}

