using FCG_Payments.Domain.Entity;
using FCG_Payments.Domain.Interface.Repository;
using FCG_Payments.Domain.Interface.Service;
using FCG_Payments.Domain.Model;
using FCG_Payments.Domain.Model.Response;
using Microsoft.Extensions.Logging;

namespace FCG_Payments.Application.Service;

public class PaymentService : IPaymentService
{
    private readonly ILogger<PaymentService> _logger;
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(ILogger<PaymentService> logger, IPaymentRepository paymentRepository)
    {
        _logger = logger;
        _paymentRepository = paymentRepository;
    }

    public async Task<GameResponse?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Iniciando GetByIdAsync para pagamento {PaymentId}", id);

        try
        {
            var payment = await _paymentRepository.GetByIdAsync(id);

            if (payment is null)
            {
                _logger.LogWarning("GetByIdAsync: Pagamento {PaymentId} não encontrado", id);
                return null;
            }

            _logger.LogInformation("GetByIdAsync: Pagamento {PaymentId} encontrado", id);

            return new GameResponse
            {
                Id = payment.Id,
                // Adicione outros campos se necessário
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar pagamento {PaymentId}", id);
            throw;
        }
    }

    public async Task<OperationResult> ProcessPaymentAsync(GameResponse game, CancellationToken ct)
    {
        _logger.LogInformation("Iniciando ProcessPaymentAsync para game {GameId}", game.Id);

        try
        {
            await CreateAsync(game);

            _logger.LogInformation("ProcessPaymentAsync: Pagamento processado com sucesso para game {GameId}", game.Id);

            return OperationResult.Ok($"Pagamento processado com sucesso para o game {game.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar pagamento do game {GameId}", game.Id);

            return OperationResult.Fail($"Erro ao processar pagamento do game {game.Id}");
        }
    }

    public async Task<GameResponse> CreateAsync(GameResponse game)
    {
        _logger.LogInformation("Iniciando CreateAsync para game {GameId}", game.Id);

        try
        {
            var purchase = new PurchaseEntity(Guid.NewGuid(), Guid.NewGuid(), game.Value)
            {
                Id = Guid.NewGuid()
            };

            _logger.LogInformation("CreateAsync: Criando purchase {PurchaseId} para game {GameId}", purchase.Id, game.Id);

            await _paymentRepository.AddAsync(purchase);
            await _paymentRepository.SaveChangesAsync();

            _logger.LogInformation("CreateAsync: Purchase {PurchaseId} salvo com sucesso", purchase.Id);

            return new GameResponse
            {
                Id = game.Id,
                Name = game.Name,
                Gender = game.Gender,
                Value = game.Value
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pagamento para game {GameId}", game.Id);
            throw;
        }
    }
}
