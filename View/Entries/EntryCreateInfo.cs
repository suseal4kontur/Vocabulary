using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using View.Meanings;

namespace View.Entries
{
    public sealed class EntryCreateInfo
    {
        [DataMember]
        [StringLength(30)]
        [Required]
        public string Lemma { get; set; }

        [DataMember]
        [Required]
        public IReadOnlyList<MeaningCreateInfo> Meanings { get; set; }

        [DataMember]
        [ReadOnlyListElementsStringLength(30)]
        public IReadOnlyList<string> Forms { get; set; }

        [ReadOnlyListElementsStringLength(30)]
        public IReadOnlyList<string> Synonyms { get; set; }

        [DataMember]
        public DateTime? AddedAt { get; set; }
    }
}
