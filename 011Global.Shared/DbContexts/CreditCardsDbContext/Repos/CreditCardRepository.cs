using _011Global.Shared.DbContexts.CreditCardsDbContext.Intefaces;
using _011Global.Shared.Exceptions;
using _011Global.Shared.JobsServiceDBContext;

namespace _011Global.Shared.DbContexts.CreditCardsDbContext.Repos
{
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly JobsServiceContext _context;
        public CreditCardRepository(JobsServiceContext context)
        {
            _context = context;
        }
        public async Task Add(CreditCard card)
        {
            try
            {
                await _context.Global_CreditCards.AddAsync(card);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new AddDBException("The credit card could not be saved", ex);
            }
        }

        public CreditCard getByCustomerId(int customerId)
        {
            return _context.Global_CreditCards.FirstOrDefault(cc => cc.CustomerId == customerId);
        }
    }
}
