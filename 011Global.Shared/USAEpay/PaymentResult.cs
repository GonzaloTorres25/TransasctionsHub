
using System.Text.Json.Serialization;

namespace _011Global.Shared.IUSAEpay
{
    public class PaymentResult
    {
        [JsonPropertyName("key")]
        public string key {  get; set; }
        [JsonPropertyName("refnum")]
        public string refnum { get; set; }
        [JsonPropertyName("authcode")]
        public string authcode { get; set; }
        [JsonPropertyName("auth_amount")]
        public string auth_amount { get; set; }
        [JsonPropertyName("result")]
        public string result {  get; set; }
        [JsonPropertyName("error")]
        public string? error { get; set; }
    }
}
