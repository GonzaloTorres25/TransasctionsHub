namespace _011Global.JobsService.Entities
{
    public class USAEpayRequest
    {
        public string Command { get; set; } = "sale";
        public decimal Amount { get; set; }
        public CreditCardDTO CreditCard { get; set; } = null!;
    }
}
