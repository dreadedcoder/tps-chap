using MongoDB.Driver;
using System.Threading.Tasks;

namespace ChapChap.Consumers.Data
{
    /// <summary>
    /// Abstraction over data access
    /// </summary>
    public class TransactionRepository
    {
        private readonly IMongoCollection<Transaction> _transactions;

        public TransactionRepository(IMongoCollection<Transaction> txnsCollection)
        {
            _transactions = txnsCollection;
        }

        public async Task AddTransactionAsync(Transaction transaction)
            => await _transactions.InsertOneAsync(transaction);
    }
}
