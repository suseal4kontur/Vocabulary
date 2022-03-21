using System;
using System.Collections.Generic;
using Model.Meanings;

namespace Model.Entries
{
    public sealed class Entry
    {
        public string Lemma { get; set; }

        public IReadOnlyList<Meaning> Meanings { get; set; }

        public IReadOnlyList<string> Forms { get; set; }

        public IReadOnlyList<string> Synonyms { get; set; }

        public DateTime AddedAt { get; set; }
    }
}
