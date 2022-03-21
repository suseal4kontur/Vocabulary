using System.Collections.Generic;

namespace Model.Entries
{
    public sealed class EntryUpdateInfo
    {
        public IReadOnlyList<string> Forms { get; set; }

        public IReadOnlyList<string> Synonyms { get; set; }
    }
}
