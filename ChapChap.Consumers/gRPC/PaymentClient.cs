using ChapChap.Payments;
using Grpc.Net.Client;
using System.Threading.Tasks;

namespace ChapChap.Consumers.gRPC
{
    /// <summary>
    /// The client that handles the communication with the gRPC service.
    /// </summary>
    public class PaymentClient
    {   
        /// <summary>
        /// Generated client from the MakePayment service .proto file.
        /// </summary>
        private readonly MakePayment.MakePaymentClient _makePaymentClient;
  
        public PaymentClient(GrpcChannel grpcChannel)
        {
            _makePaymentClient = new MakePayment.MakePaymentClient(grpcChannel);
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
