namespace _011Global.JobsService.Entities
{
    public class USAEpayRequest
    {
        public string command { get; set; } = "sale";
        public decimal amount { get; set; }
        public CreditCardDTO creditcard { get; set; } =  new CreditCardDTO();
    }
}
