using _011Global.Shared.DbContexts.CreditCardsDbContext.Intefaces;
using _011Global.Shared.Exceptions;
using _011Global.Shared.JobsServiceDBContext;
using Microsoft.Extensions.Logging;

namespace _011Global.Shared.DbContexts.CreditCardsDbContext.Repos
{
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly JobsServiceContext _context;
        private readonly ILogger<CreditCardRepository> _logger;
        public CreditCardRepository(JobsServiceContext context, ILogger<CreditCardRepository> logger)
        {
            _context = context;
            _logger = logger;
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
                _logger.LogError(ex, "Error saving credit card {@Card}", card);
                throw new AddDBException("The credit card could not be saved", ex);
            }
        }

        public CreditCard getByCustomerId(int customerId)
        {
            return _context.Global_CreditCards.FirstOrDefault(cc => cc.CustomerId == customerId);
        }

        public async Task UpdateCreditCardToken(int creditCardId, string token)
        {
            try
            {
                var card = await _context.Global_CreditCards.FindAsync(creditCardId);

                if (card == null)
                {
                    throw new NotFoundException($"Credit card with ID {creditCardId} not found.");
                }

                card.CreditCardNumber = token;
                _context.Global_CreditCards.Update(card);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating credit card token for ID {Id}", creditCardId);
                throw new UpdateDBException("The credit card token could not be updated", ex);
            }
        }
    }
}
