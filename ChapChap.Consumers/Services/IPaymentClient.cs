using ChapChap.Payments;
using System.Threading.Tasks;

namespace ChapChap.Consumers.Services
{
    public interface IPaymentClient
    {
        Task<TransactionResponse> CreatePaymentTransaction(TransactionRequest transactionRequest);
    }
}
