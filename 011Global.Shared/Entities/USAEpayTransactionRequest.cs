using _011Global.Shared.Entities;

namespace _011Global.JobsService.Entities
{
    public class USAEpayTransactionRequest
    {
        public string command { get; set; } = "sale";
        public decimal amount { get; set; }
        public CreditCardDTO creditcard { get; set; } =  new CreditCardDTO();
        public bool save_card { get; set; }
    }
}
