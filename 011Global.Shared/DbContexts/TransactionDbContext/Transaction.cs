namespace _011Global.Shared.DbContexts.TransactionDbContext
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int CustomerID { get; set; }
        public float Amount { get; set; }
        public int TransactionStatus { get; set; }
        public string? PaymentGWTransID { get; set; }
        public string? ResponseCode { get; set; }
        public string? SubErrorDesc1 { get; set; }
        public string? SubErrorDesc2 { get; set; }
        public string? SubErrorDesc3 { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreditCardId { get; set; }
    }
}
