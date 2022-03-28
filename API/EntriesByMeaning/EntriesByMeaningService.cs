using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Model;
using ViewEntries = View.Entries;
using ModelEntries = Model.Entries;

namespace VocabularyAPI.EntriesByMeaning
{
    public sealed class EntriesByMeaningService : IEntriesByMeaningService
    {
        private readonly IVocabulary vocabulary;
        private readonly IMapper mapper;

        public EntriesByMeaningService(IVocabulary vocabulary, IMapper mapper)
        {
            this.vocabulary = vocabulary ?? throw new ArgumentNullException(nameof(vocabulary));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<ViewEntries.Entry> GetEntryByMeaningAsync(
            string meaningId,
            CancellationToken token)
        {
            var modelEntry = await this.vocabulary.GetEntryByMeaningAsync(meaningId, token);

            var viewEntry = this.mapper.Map<ModelEntries.Entry, ViewEntries.Entry>(modelEntry);
            return viewEntry;
        }
    }
}
