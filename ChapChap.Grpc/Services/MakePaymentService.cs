using Grpc.Core;

namespace ChapChap.Grpc.Services
{
    /// <summary>
    /// The MakePayment Service that processes a request from a gRPC client
    /// </summary>
    /// 
    /// <param name="logger"></param>
    public class MakePaymentService(
        ILogger<MakePaymentService> logger) : MakePayment.MakePaymentBase
    {
        private readonly ILogger<MakePaymentService> _logger = logger;
           

        /// <summary>
        ///  Always returns a success status and message for every request
        /// </summary>
        /// 
        /// <param name="txnRequest"> The transaction request to be fulfilled</param>
        /// <param name="callContext"></param>
        /// 
        /// <returns> <see cref="TransactionResponse"/> </returns>
        public override Task<TransactionResponse> CreateTransaction(
            TransactionRequest txnRequest, ServerCallContext callContext)
            => Task.FromResult(new TransactionResponse
            {
                Message = $"Transaction for {txnRequest.ReferenceId} request processed successfully",
                Status = TransactionResponse.Types.Status.Success
            });
    }
}
