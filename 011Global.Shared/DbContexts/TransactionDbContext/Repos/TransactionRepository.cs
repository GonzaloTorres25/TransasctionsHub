using _011Global.Shared.DbContexts.TransactionDbContext.Interfaces;
using _011Global.Shared.IUSAEpay;
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
            return await _context.Global_Transactions.Where(t => t.CustomerID == customerId && t.TransactionStatus == 5).OrderByDescending(t => t.CreationDate).FirstOrDefaultAsync();
        }

        public Task SaveTransaction(PaymentResult paymentResult)
        {
            throw new NotImplementedException();
        }
    }
}
