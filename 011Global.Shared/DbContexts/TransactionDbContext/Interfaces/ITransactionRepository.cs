using Microsoft.EntityFrameworkCore.Storage;

namespace _011Global.Shared.DbContexts.TransactionDbContext.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetLastVerificationRequiredTransactionByCustomerId(int customerId);
        Task SaveTransaction(Transaction transaction);
    }
}
