using System.Transactions;
using _011Global.Shared.JobsServiceDBContext;
using _011Global.Shared.TransactionDbContext.Interfaces;

namespace _011Global.Shared.TransactionDbContext.Repos
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly JobsServiceContext _context;
        public TransactionRepository(JobsServiceContext context)
        {
            _context = context;
        }

        public async Task SaveTransaction(Transaction result)
        {
            //_context.Transaction.Add(result);
            await _context.SaveChangesAsync();
        }
    }
}
