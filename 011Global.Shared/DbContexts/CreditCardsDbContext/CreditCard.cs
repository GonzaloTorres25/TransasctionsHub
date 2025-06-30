namespace _011Global.Shared.DbContexts.CreditCardsDbContext
{
    public class CreditCard
    {
        public int CreditCardId { get; set; }
        public int CustomerId { get; set; }
        public string CreditCardNumber { get; set; }
        public int LastFourNumbers { get; set; }
        public string CardHolder { get; set; }
        public string SecurityCode { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
