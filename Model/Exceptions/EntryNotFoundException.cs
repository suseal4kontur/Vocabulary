using System;

namespace Model.Exceptions
{
    public class EntryNotFoundException : Exception
    {
        public EntryNotFoundException(string lemma) : base($"Entry with lemma {lemma} not found.")
        {
        }
    }
}
