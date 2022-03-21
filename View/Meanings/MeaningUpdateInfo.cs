using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace View.Meanings
{
    public sealed class MeaningUpdateInfo
    {
        [DataMember]
        [PartOfSpeech]
        public string PartOfSpeech { get; set; }

        [DataMember]
        [StringLength(1000)]
        public string Description { get; set; }

        [DataMember]
        [StringLength(100)]
        public string Example { get; set; }
    }
}
