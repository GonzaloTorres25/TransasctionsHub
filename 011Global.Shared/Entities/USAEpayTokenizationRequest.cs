namespace _011Global.Shared.Entities
{
    public class USAEpayTokenizationRequest
    {
        public string command { get; set; } = "cc:save";
        public CreditCardDTO creditcard { get; set; } = new CreditCardDTO();
    }
}
