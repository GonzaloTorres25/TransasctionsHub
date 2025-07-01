using _011Global.Shared.DbContexts.TransactionDbContext.Interfaces;
using _011Global.Shared.JobsServiceDBContext;
using Microsoft.EntityFrameworkCore;

namespace _011Global.Shared.DbContexts.TransactionDbContext.Repos
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly JobsServiceContext _context;
        public TransactionRepository(JobsServiceContext context)
        {
            _context = context;
        }

        public async Task<Transaction?> GetLastVerificationRequiredTransactionByCustomerId(int customerId)
        {
            return await _context.Global_Transactions.Where(t => t.CustomerID == customerId).FirstOrDefaultAsync();
        }

        public async Task SaveTransaction(Transaction transaction)
        {
            _context.Global_Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
