using System.Text.Json.Serialization;

namespace _011Global.Shared.Entities
{
    public class TokenResult
    {
        [JsonPropertyName("key")]
        public string token { get; set; }
    }
}
