using ChapChap.Consumers.Messages;
using MassTransit;
using System.Threading.Tasks;
using ChapChap.Payments;
using System.Globalization;
using Microsoft.Extensions.Logging;
using ChapChap.Consumers.gRPC;
using ChapChap.Consumers.Data;
using System;

namespace ChapChap.Consumers
{
    /// <summary>
    /// 
    /// </summary>
    public class PaymentProcessingConsumer : IConsumer<PaymentMessage>
    {
        private readonly ILogger<PaymentProcessingConsumer> _logger;
        private readonly TransactionRepository _txnRepository;
        private readonly PaymentClient _paymentClient;

        public PaymentProcessingConsumer(
            ILogger<PaymentProcessingConsumer> logger,
            TransactionRepository txnRepository, PaymentClient paymentClient)
        {
            _logger = logger;
            _txnRepository = txnRepository;
            _paymentClient = paymentClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<PaymentMessage> context)
        {
            try
            {
                var resoponse = await _paymentClient.CreatePaymentTransaction(new TransactionRequest
                {
                    Amount = context.Message.Amount.ToString(CultureInfo.InvariantCulture),
                    ReferenceId = context.Message.ReferenceId.ToString(),
                    UserId = context.Message.UserId.ToString()
                });

                await _txnRepository.AddTransactionAsync(new Transaction
                {
                    Amount = context.Message.Amount,
                    Message = resoponse.Message,
                    ReferenceId = context.Message.ReferenceId,
                    UserId = context.Message.UserId,
                    Status = resoponse.Status.ToString(),
                    
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in processing transaction with {ReferenceId} for {UserId}",
                    context.Message.ReferenceId, context.Message.UserId);
            }

        }
    }
}
