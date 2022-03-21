using System;
using System.Collections.Generic;
using View.Meanings;

namespace View.Entries
{
    public class EntryShortInfo
    {
        public string Lemma { get; set; }

        public IReadOnlyList<Meaning> Meanings { get; set; }

        public DateTime AddedAt { get; set; }
    }
}
