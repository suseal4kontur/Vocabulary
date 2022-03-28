using System.Threading;
using System.Threading.Tasks;
using View.Entries;

namespace VocabularyAPI.Entries
{
    public interface IEntriesService
    {
        public Task<Entry> CreateEntryAsync(EntryCreateInfo viewCreateInfo, CancellationToken token);

        public Task DeleteEntryAsync(string lemma, CancellationToken token);

        public Task<Entry> GetEntryAsync(string lemma, CancellationToken token);

        public Task<EntriesList> SearchEntriesAsync(EntriesSearchInfo viewSearchInfo, CancellationToken token);

        public Task UpdateEntryAsync(string lemma, EntryUpdateInfo viewUpdateInfo, CancellationToken token);
    }
}