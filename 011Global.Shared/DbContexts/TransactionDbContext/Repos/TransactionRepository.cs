using _011Global.Shared.DbContexts.TransactionDbContext.Interfaces;
using _011Global.Shared.Exceptions;
using _011Global.Shared.JobsServiceDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace _011Global.Shared.DbContexts.TransactionDbContext.Repos
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly JobsServiceContext _context;
        private readonly ILogger<TransactionRepository> _logger;
        public TransactionRepository(JobsServiceContext context, ILogger<TransactionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Transaction?> GetLastVerificationRequiredTransactionByCustomerId(int customerId)
        {
            return await _context.Global_Transactions.Where(t => t.CustomerID == customerId).OrderByDescending(t => t.CreationDate).FirstOrDefaultAsync();
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return _context.Database.BeginTransactionAsync();
        }

        public async Task SaveTransaction(Transaction transaction)
        {
            try
            {
                _context.Global_Transactions.Add(transaction);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving transaction {@Transaction}", transaction);
                throw new AddDBException("The transaction could not be saved", ex);
            }
        }
    }
}
