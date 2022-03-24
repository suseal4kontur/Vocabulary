using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using View.Meanings;

namespace View.Entries
{
    public class EntryShortInfo
    {
        [JsonPropertyName("lemma")]
        public string Lemma { get; set; }

        [JsonPropertyName("meanings")]
        public IReadOnlyList<MeaningShortInfo> Meanings { get; set; }

        [JsonPropertyName("addedAt")]
        public DateTime AddedAt { get; set; }
    }
}
