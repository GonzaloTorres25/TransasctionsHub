namespace _011Global.Shared.DbContexts.CreditCardsDbContext
{
    public class CreditCard
    {
        public int CreditCardId { get; set; }
        public int CustomerId { get; set; }
        public string CreditCardNumber { get; set; }
        public string LastFourNumbers { get; set; }
        public string CardHolder { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
