namespace _011Global.Shared.CreditCardsDbContext.Intefaces
{
    public interface ICreditCardRepository
    {
        Task Add(CreditCard creditCard);
    }
}
