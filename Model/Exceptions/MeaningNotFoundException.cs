using System;

namespace Model.Exceptions
{
    public class MeaningNotFoundException : Exception
    {
        public MeaningNotFoundException(string meaningId) : base($"Meaning with id {meaningId} not found.")
        {
        }
    }
}
