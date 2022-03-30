using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Model;
using ViewEntries = View.Entries;
using ModelEntries = Model.Entries;

namespace VocabularyAPI.Entries
{
    public sealed class EntriesService : IEntriesService
    {
        private readonly IVocabulary vocabulary;
        private readonly IMapper mapper;

        public EntriesService(IVocabulary vocabulary, IMapper mapper)
        {
            this.vocabulary = vocabulary ?? throw new ArgumentNullException(nameof(vocabulary));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ViewEntries.Entry> GetEntryAsync(
            string lemma,
            CancellationToken token)
        {
            var modelEntry = await this.vocabulary.GetEntryAsync(lemma, token);

            var viewEntry = this.mapper.Map<ModelEntries.Entry, ViewEntries.Entry>(modelEntry);
            return viewEntry;
        }

        public async Task<ViewEntries.Entry> CreateEntryAsync(
            ViewEntries.EntryCreateInfo viewCreateInfo,
            CancellationToken token)
        {
            var modelCreateInfo = this.mapper
                .Map<ViewEntries.EntryCreateInfo, ModelEntries.EntryCreateInfo>(viewCreateInfo);
            ValidateOnCreate(modelCreateInfo);

            var modelEntry = await this.vocabulary.CreateEntryAsync(modelCreateInfo, token);

            var viewEntry = this.mapper.Map<ModelEntries.Entry, ViewEntries.Entry>(modelEntry);
            return viewEntry;
        }

        public async Task<ViewEntries.EntriesList> SearchEntriesAsync(
            ViewEntries.EntriesSearchInfo viewSearchInfo,
            CancellationToken token)
        {
            var modelSearchInfo = this.mapper
                .Map<ViewEntries.EntriesSearchInfo, ModelEntries.EntriesSearchInfo>(viewSearchInfo);
            var modelEntriesList = await this.vocabulary.SearchEntriesAsync(modelSearchInfo, token);

            var viewEntriesList = this.mapper
                .Map<ModelEntries.EntriesList, ViewEntries.EntriesList>(modelEntriesList);
            return viewEntriesList;
        }

        public async Task UpdateEntryAsync(
            string lemma,
            ViewEntries.EntryUpdateInfo viewUpdateInfo,
            CancellationToken token)
        {
            var modelUpdateInfo = this.mapper
                .Map<ViewEntries.EntryUpdateInfo, ModelEntries.EntryUpdateInfo>(viewUpdateInfo);
            ValidateOnUpdate(modelUpdateInfo, lemma);
            await this.vocabulary.UpdateEntryAsync(lemma, modelUpdateInfo, token);
        }

        public async Task DeleteEntryAsync(
            string lemma,
            CancellationToken token)
        {
            await this.vocabulary.DeleteEntryAsync(lemma, token);
        }

        private static void ValidateOnCreate(ModelEntries.EntryCreateInfo createInfo)
        {
            if (createInfo.Forms != null && !createInfo.Forms.Contains(createInfo.Lemma))
                throw new ValidationException("Forms must contain lemma.");

            if (createInfo.Forms?.Count > createInfo.Forms?.Distinct().Count())
                throw new ValidationException("All forms must be different.");

            if (createInfo.Synonyms?.Count > createInfo.Synonyms?.Distinct().Count())
                throw new ValidationException("All synonyms must be different.");

            if (createInfo.Synonyms != null && createInfo.Synonyms.Contains(createInfo.Lemma))
                throw new ValidationException("Synonyms must not contain lemma.");
        }

        private static void ValidateOnUpdate(ModelEntries.EntryUpdateInfo updateInfo, string lemma)
        {
            if (updateInfo.Forms == null && updateInfo.Synonyms == null)
                throw new ValidationException("Update info is empty.");

            if (updateInfo.Forms != null && !updateInfo.Forms.Contains(lemma))
                throw new ValidationException("Forms must contain lemma.");

            if (updateInfo.Forms?.Count > updateInfo.Forms?.Distinct().Count())
                throw new ValidationException("All forms must be different.");

            if (updateInfo.Synonyms?.Count > updateInfo.Synonyms?.Distinct().Count())
                throw new ValidationException("All synonyms must be different.");

            if (updateInfo.Synonyms != null && updateInfo.Synonyms.Contains(lemma))
                throw new ValidationException("Synonyms must not contain lemma.");
        }
    }
}
