namespace _011Global.Shared.DbContexts.CreditCardsDbContext.Intefaces
{
    public interface ICreditCardRepository
    {
        Task Add(CreditCard creditCard);
        CreditCard getByCustomerId(int customerId);
        Task<List<CreditCard>> getAllCreditCards();
        Task UpdateCreditCardToken(int creditCardId, string token);
    }
}
