using ChapChap.Consumers.Data;
using System.Threading.Tasks;

namespace ChapChap.Consumers.Services
{
    public interface ITransactionRepository
    {
        Task AddTransactionAsync(Transaction transaction);
    }
}
