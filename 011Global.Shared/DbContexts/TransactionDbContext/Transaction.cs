namespace _011Global.Shared.DbContexts.TransactionDbContext
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int CustomerID { get; set; }
        public double? Amount { get; set; }
        public byte TransactionStatusID { get; set; }
        public string? PaymentGWTransID { get; set; }
        public string? AuthCode { get; set; }
        public string? ResponseCode { get; set; }
        public string? SubErrorDesc { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreditCardID { get; set; }
    }
}

                    
                            
