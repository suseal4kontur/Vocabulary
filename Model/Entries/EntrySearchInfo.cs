using System;
using Model.Meanings;

namespace Model.Entries
{
    public sealed class EntrySearchInfo
    {
        public PartOfSpeech? PartOfSpeech { get; set; }

        public string Form { get; set; }

        public string Synonym { get; set; }

        public DateTime? FromAddedAt { get; set; }

        public DateTime? ToAddedAt { get; set; }

        public int? Limit { get; set; }

        public int? Offset { get; set; }
    }
}
