
using System.Transactions;

namespace _011Global.Shared.TransactionDbContext.Interfaces
{
    public interface ITransactionRepository
    {
        Task SaveTransaction(Transaction result);
    }
}
