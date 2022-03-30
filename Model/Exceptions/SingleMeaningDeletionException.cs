using System;

namespace Model.Exceptions
{
    public class SingleMeaningDeletionException : Exception
    {
        public SingleMeaningDeletionException(string meaningId) : base(
            $"Meaning {meaningId} can't be deleted, it is the only meaning in the lemma.")
        {
        }
    }
}
