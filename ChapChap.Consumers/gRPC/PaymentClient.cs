using ChapChap.Payments;
using Grpc.Net.Client;
using System.Threading.Tasks;

namespace ChapChap.Consumers.gRPC
{
    /// <summary>
    /// 
    /// </summary>
    public class PaymentClient
    {    
        private readonly MakePayment.MakePaymentClient _makePaymentClient;
  
        public PaymentClient(GrpcChannel grpcChannel)
        {
            _makePaymentClient = new MakePayment.MakePaymentClient(grpcChannel);
        }

        public async Task<TransactionResponse> CreatePaymentTransaction(TransactionRequest transactionRequest)
        {
            return await _makePaymentClient.CreateTransactionAsync(transactionRequest);
        }
    }
}
