using _011Global.Shared.DbContexts.CreditCardsDbContext;
using _011Global.Shared.DbContexts.CustomerDbContext;
using _011Global.Shared.IUSAEpay;

namespace _011Global.JobsService.JobInterfaces
{
    public interface IUSAEpayService
    {
        Task<PaymentResult> Charge(Customer customer, CreditCard creditCard);
    }
}
