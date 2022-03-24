using System.Text.Json.Serialization;

namespace View.Meanings
{
    public class MeaningShortInfo
    {
        [JsonPropertyName("partOfSpeech")]
        public string PartOfSpeech { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
