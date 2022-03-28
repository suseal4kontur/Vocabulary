using System.Threading;
using System.Threading.Tasks;
using View.Entries;

namespace VocabularyAPI.EntriesByMeaning
{
    public interface IEntriesByMeaningService
    {
        public Task<Entry> GetEntryByMeaningAsync(string meaningId, CancellationToken token);
    }
}