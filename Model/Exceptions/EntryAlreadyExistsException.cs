using System;

namespace Model.Exceptions
{
    public class EntryAlreadyExistsException : Exception
    {
        public EntryAlreadyExistsException(string lemma) : base($"Entry with lemma {lemma} already exists.")
        {
        }
    }
}
