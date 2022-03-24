using System.Collections.Generic;
using System.Text.Json.Serialization;
using View.Meanings;

namespace View.Entries
{
    public sealed class Entry : EntryShortInfo
    {
        [JsonPropertyName("meanings")]
        public new IReadOnlyList<Meaning> Meanings { get; set; }

        [JsonPropertyName("forms")]
        public IReadOnlyList<string> Forms { get; set; }

        [JsonPropertyName("synonyms")]
        public IReadOnlyList<string> Synonyms { get; set; }
    }
}
