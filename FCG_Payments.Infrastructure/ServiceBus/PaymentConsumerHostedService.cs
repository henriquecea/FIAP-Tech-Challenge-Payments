using Azure.Messaging.ServiceBus;
using FCG_Payments.Domain.Interface.Service;
using FCG_Payments.Domain.Model.Response;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FCG_Payments.Infrastructure.ServiceBus;

public class PaymentConsumerHostedService : BackgroundService
{
    private readonly ServiceBusProcessor _processor;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PaymentConsumerHostedService> _logger;

    public PaymentConsumerHostedService(
        ServiceBusClient client,
        IServiceScopeFactory scopeFactory,
        ILogger<PaymentConsumerHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;

        _processor = client.CreateProcessor("games", new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 2
        });

        _processor.ProcessMessageAsync += OnMessageAsync;
        _processor.ProcessErrorAsync += OnErrorAsync;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        => await _processor.StartProcessingAsync(stoppingToken);

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _processor.StopProcessingAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }

    private async Task OnMessageAsync(ProcessMessageEventArgs args)
    {
        try
        {
            var json = args.Message.Body.ToString();
            var game = JsonSerializer.Deserialize<GameResponse>(json);

            if (game is null)
            {
                await args.DeadLetterMessageAsync(
                    args.Message,
                    "InvalidMessage",
                    "Could not deserialize GameResponse");

                return;
            }

            using var scope = _scopeFactory.CreateScope();
            var paymentService = scope.ServiceProvider
                .GetRequiredService<IPaymentService>();

            var result = await paymentService
                .ProcessPaymentAsync(game, args.CancellationToken);

            if (result.Success)
            {
                _logger.LogInformation(
                    "Mensagem processada com sucesso. MessageId={MessageId} | {Message}",
                    args.Message.MessageId,
                    result.Message);

                await args.CompleteMessageAsync(args.Message);
            }
            else
            {
                _logger.LogWarning(
                    "Falha no processamento da mensagem. MessageId={MessageId} | {Message}",
                    args.Message.MessageId,
                    result.Message);

                // Falha de negócio → DeadLetter
                await args.DeadLetterMessageAsync(
                    args.Message,
                    "BusinessFailure",
                    result.Message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Erro inesperado ao processar mensagem. MessageId={MessageId}",
                args.Message.MessageId);

            // Erro técnico → retry automático
            await args.AbandonMessageAsync(args.Message);
        }
    }

    private Task OnErrorAsync(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "ServiceBus error. Entity={EntityPath} Source={ErrorSource}",
            args.EntityPath, args.ErrorSource);

        return Task.CompletedTask;
    }
}