using Model.Entries;
using System;

namespace ClientTests
{
    internal interface IVocabularyCleaner
    {
        void DropEntries();
        void RemoveFromEntries(Func<Entry, bool> condition);
    }
}