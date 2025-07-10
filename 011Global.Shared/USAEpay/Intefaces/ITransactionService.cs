using _011Global.Shared.DbContexts.CreditCardsDbContext;
using _011Global.Shared.DbContexts.CustomerDbContext;
using _011Global.Shared.Entities;

namespace _011Global.JobsService.JobInterfaces
{
    public interface ITransactionService
    {
        Task<PaymentResult> Charge(Customer customer, CreditCard creditCard);
    }
}
