using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace View.Meanings
{
    public sealed class MeaningCreateInfo
    {
        [DataMember]
        [PartOfSpeech]
        [Required]
        [JsonPropertyName("partOfSpeech")]
        public string PartOfSpeech { get; set; }

        [DataMember]
        [StringLength(1000)]
        [Required]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [DataMember]
        [StringLength(100)]
        [JsonPropertyName("example")]
        public string Example { get; set; }
    }
}
