using Grpc.Core;
using ChapChap.Payments;

namespace ChapChap.gRPC.Services
{
    /// <summary>
    /// The MakePayment Service that processes a request from a gRPC client
    /// </summary>

    public class MakePaymentService : MakePayment.MakePaymentBase
    {
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