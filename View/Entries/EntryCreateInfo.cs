using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using View.Meanings;

namespace View.Entries
{
    public sealed class EntryCreateInfo
    {
        [DataMember]
        [StringLength(30)]
        [Required]
        [JsonPropertyName("lemma")]
        public string Lemma { get; set; }

        [DataMember]
        [Required]
        [JsonPropertyName("meanings")]
        public IReadOnlyList<MeaningCreateInfo> Meanings { get; set; }

        [DataMember]
        [ReadOnlyListElementsStringLength(30)]
        [JsonPropertyName("forms")]
        public IReadOnlyList<string> Forms { get; set; }

        [ReadOnlyListElementsStringLength(30)]
        [JsonPropertyName("synonyms")]
        public IReadOnlyList<string> Synonyms { get; set; }

        [DataMember]
        [JsonPropertyName("addedAt")]
        public DateTime? AddedAt { get; set; }
    }
}
