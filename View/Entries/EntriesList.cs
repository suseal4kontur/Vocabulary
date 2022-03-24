using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace View.Entries
{
    public sealed class EntriesList
    {
        [JsonPropertyName("entries")]
        public IReadOnlyList<EntryShortInfo> Entries { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}
