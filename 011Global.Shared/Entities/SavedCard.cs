using System.Text.Json.Serialization;

namespace _011Global.Shared.Entities
{
    public class SavedCard
    {
        [JsonPropertyName("type")]
        public string type { get; set; }

        [JsonPropertyName("key")]
        public string key { get; set; }

        [JsonPropertyName("cardnumber")]
        public string cardnumber { get; set; }
    }
}
