namespace Model.Meanings
{
    public sealed class Meaning
    {
        public string Id { get; set; }

        public PartOfSpeech PartOfSpeech { get; set; }

        public string Description { get; set; }

        public string Example { get; set; }
    }
}
