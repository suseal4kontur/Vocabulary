using System.Threading;
using System.Threading.Tasks;
using View.Meanings;

namespace VocabularyAPI.Meanings
{
    public interface IMeaningsService
    {
        public Task<Meaning> CreateMeaningAsync(
            string lemma,
            MeaningCreateInfo viewCreateInfo,
            CancellationToken token);

        public Task DeleteMeaningAsync(string lemma, string meaningId, CancellationToken token);

        public Task UpdateMeaningAsync(
            string lemma,
            string meaningId,
            MeaningUpdateInfo viewUpdateInfo,
            CancellationToken token);
    }
}