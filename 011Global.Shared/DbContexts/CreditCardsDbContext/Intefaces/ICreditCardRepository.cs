using _011Global.Shared.DbContexts.CreditCardsDbContext;

namespace _011Global.Shared.DbContexts.CreditCardsDbContext.Intefaces
{
    public interface ICreditCardRepository
    {
        Task Add(CreditCard creditCard);
        CreditCard getByCustomerId(int customerId);
    }
}
