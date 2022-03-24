using Client.ClientResults;
using System;
using System.Threading.Tasks;
using View.Entries;
using View.Meanings;

namespace Client
{
    public interface IVocabularyClient
    {
        public Task<ClientResult<Entry>> GetEntryAsync(string lemma);

        public Task<ClientResult<EntriesList>> GetEntriesAsync(
            string partOfSpeech = null,
            string form = null,
            string synonym = null,
            DateTime? fromAddedAt = null,
            DateTime? toAddedAt = null,
            int? offset = null,
            int? limit = null);

        public Task<ClientResult<Entry>> CreateEntryAsync(EntryCreateInfo createInfo);

        public Task<ClientResult> UpdateEntryAsync(string lemma, EntryUpdateInfo updateInfo);

        public Task<ClientResult> DeleteEntryAsync(string lemma);

        public Task<ClientResult<Entry>> GetEntryByMeaningAsync(string meaningId);

        public Task<ClientResult<Meaning>> CreateMeaningAsync(string lemma, MeaningCreateInfo createInfo);

        public Task<ClientResult> UpdateMeaningAsync(
            string lemma,
            string meaningId,
            MeaningUpdateInfo updateInfo);

        public Task<ClientResult> DeleteMeaningAsync(string lemma, string meaningId);
    }
}
