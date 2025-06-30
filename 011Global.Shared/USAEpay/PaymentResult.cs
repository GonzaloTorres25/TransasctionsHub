
namespace _011Global.Shared.IUSAEpay
{
    public class PaymentResult
    {
        public bool Success { get; set; }            
        public string TransactionId { get; set; }   
        public string ResponseCode { get; set; }    
        public string ErrorMessage { get; set; }     
        public string SubErrorDesc1 { get; set; }    
        public string SubErrorDesc2 { get; set; }
        public string SubErrorDesc3 { get; set; }
    }
}
