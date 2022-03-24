using System.Text.Json.Serialization;

namespace View.Meanings
{
    public sealed class Meaning : MeaningShortInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("example")]
        public string Example { get; set; }
    }
}
