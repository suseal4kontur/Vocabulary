using System;
using System.Collections.Generic;
using View.Meanings;

namespace View.Entries
{
    public sealed class Entry : EntryShortInfo
    {
        public new IReadOnlyList<Meaning> Meanings { get; set; }

        public IReadOnlyList<string> Forms { get; set; }

        public IReadOnlyList<string> Synonyms { get; set; }
    }
}
