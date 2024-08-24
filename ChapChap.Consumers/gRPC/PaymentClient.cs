using ChapChap.Payments;
using System.Threading.Tasks;
using ChapChap.Consumers.Services;
using Grpc.Net.Client;

namespace ChapChap.Consumers.gRPC
{
    /// <summary>
    /// The client that handles the communication with the gRPC service.
    /// </summary>
    public class PaymentClient : IPaymentClient
    {   
        /// <summary>
        /// Generated client from the MakePayment service .proto file.
        /// </summary>
        private readonly MakePayment.MakePaymentClient _makePaymentClient;

        public PaymentClient(GrpcChannel channel)
        {
            _makePaymentClient = new MakePayment.MakePaymentClient(channel);
        }

        /// <summary>
        /// Calls the generated gRPC client with the transactionRequest
        /// </summary>
        /// <param name="transactionRequest"> <see cref="TransactionRequest"/></param>
        /// <returns></returns>
        public async Task<TransactionResponse> CreatePaymentTransaction(TransactionRequest transactionRequest)
            => await _makePaymentClient.CreateTransactionAsync(transactionRequest);

    }
}
