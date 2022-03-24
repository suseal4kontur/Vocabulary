using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace View.Entries
{
    public sealed class EntryUpdateInfo
    {
        [DataMember]
        [ReadOnlyListElementsStringLength(30)]
        [JsonPropertyName("forms")]
        public IReadOnlyList<string> Forms { get; set; }

        [DataMember]
        [ReadOnlyListElementsStringLength(30)]
        [JsonPropertyName("synonyms")]
        public IReadOnlyList<string> Synonyms { get; set; }
    }
}
