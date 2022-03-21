using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using View.Meanings;

namespace View.Entries
{
    public sealed class EntriesSearchInfo
    {
        [DataMember]
        [PartOfSpeech]
        public string PartOfSpeech { get; set; }

        [DataMember]
        [StringLength(30)]
        public string Form { get; set; }

        [DataMember]
        [StringLength(30)]
        public string Synonym { get; set; }

        [DataMember]
        public DateTime? FromAddedAt { get; set; }

        [DataMember]
        public DateTime? ToAddedAt { get; set; }

        [DataMember]
        [Range(1, 1000)]
        public int? Limit { get; set; } = 20;

        [DataMember]
        [Range(0, 1000)]
        public int? Offset { get; set; } = 0;
    }
}
