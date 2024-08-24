using ChapChap.Api.Models;
using ChapChap.Consumers.Messages;
using MassTransit;

namespace ChapChap.Api.Services
{
    public class TransactionRequestService(IBus massTransitBus, MassTransitOptions massTransitOptions,
        ILogger<TransactionRequestService> logger)
    {
        private readonly IBus _massTransitBus = massTransitBus;
        private readonly MassTransitOptions _massTransitOptions = massTransitOptions;
        private readonly ILogger<TransactionRequestService> _logger = logger;

        public async Task<IResult> ProcessTransactionRequestAsync(TransactionRequest request)
        {
            _logger.LogInformation("Received {@TransactionRequest} with {ReferenceId} and {UserId}",
                request, request.ReferenceId, request.UserId);

            if (request.Amount <= 0)
                return Results.BadRequest($"{nameof(request.Amount)} should be greater than 0");

            var rabbitMqOptions = _massTransitOptions.RabbitMQ ??
                throw new ArgumentNullException(nameof(_massTransitOptions.RabbitMQ));

            var address = $"{rabbitMqOptions.Host}/{rabbitMqOptions.TransactionQueue}";
            var endpoint = await _massTransitBus.GetSendEndpoint(new Uri(address));

            await endpoint.Send(new PaymentMessage
            {
                ReferenceId = request.ReferenceId,
                UserId = request.UserId,
                Amount = request.Amount
            });

            return Results.Ok("Transaction request queued for processing");
        }
    }

}
