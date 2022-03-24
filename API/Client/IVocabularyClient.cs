using System;
using System.Threading.Tasks;
using View.Entries;
using View.Meanings;

namespace VocabularyAPI.Client
{
    public interface IVocabularyClient
    {
        public Task<Entry> GetEntryAsync(string lemma);

        public Task<EntriesList> GetEntriesAsync(
            string partOfSpeech = null,
            string form = null,
            string synonym = null,
            DateTime? fromAddedAt = null,
            DateTime? toAddedAt = null,
            int? offset = null,
            int? limit = null);

        public Task CreateEntryAsync(EntryCreateInfo createInfo);

        public Task UpdateEntryAsync(string lemma, EntryUpdateInfo updateInfo);

        public Task DeleteEntryAsync(string lemma);

        public Task<Entry> GetEntryByMeaningAsync(string meaningId);

        public Task CreateMeaningAsync(string lemma, MeaningCreateInfo createInfo);

        public Task UpdateMeaningAsync(
            string lemma,
            string meaningId,
            MeaningUpdateInfo updateInfo);

        public Task DeleteMeaningAsync(string lemma, string meaningId);
    }
}
