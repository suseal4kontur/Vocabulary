using System.Collections.Generic;
using System.Runtime.Serialization;

namespace View.Entries
{
    public sealed class EntryUpdateInfo
    {
        [DataMember]
        [ReadOnlyListElementsStringLength(30)]
        public IReadOnlyList<string> Forms { get; set; }

        [DataMember]
        [ReadOnlyListElementsStringLength(30)]
        public IReadOnlyList<string> Synonyms { get; set; }
    }
}
