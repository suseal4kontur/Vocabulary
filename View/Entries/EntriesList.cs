using System.Collections.Generic;

namespace View.Entries
{
    public sealed class EntriesList
    {
        public IReadOnlyList<EntryShortInfo> Entries { get; set; }

        public int Total { get; set; }
    }
}
