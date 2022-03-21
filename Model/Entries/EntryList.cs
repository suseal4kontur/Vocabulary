using System.Collections.Generic;

namespace Model.Entries
{
    public sealed class EntryList
    {
        public IReadOnlyList<Entry> Entries { get; set; }

        public int Total { get; set; }
    }
}
