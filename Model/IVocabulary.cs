using System.Threading;
using System.Threading.Tasks;
using Model.Entries;
using Model.Meanings;

namespace Model
{
    public interface IVocabulary
    {
        public Task<Entry> GetEntryAsync(string lemma, CancellationToken token);

        public Task<Entry> CreateEntryAsync(EntryCreateInfo createInfo, CancellationToken token);

        public Task<EntriesList> SearchEntriesAsync(EntriesSearchInfo searchInfo, CancellationToken token);

        public Task UpdateEntryAsync(string lemma, EntryUpdateInfo updateInfo, CancellationToken token);

        public Task DeleteEntryAsync(string lemma, CancellationToken token);

        public Task<Entry> GetEntryByMeaningAsync(string meaningId, CancellationToken token);

        public Task<Meaning> CreateMeaningAsync(string lemma, MeaningCreateInfo createInfo, CancellationToken token);

        public Task UpdateMeaningAsync(string lemma, string meaningId, MeaningUpdateInfo updateInfo, CancellationToken token);

        public Task DeleteMeaningAsync(string lemma, string meaningId, CancellationToken token);
    }
}
