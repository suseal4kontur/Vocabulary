using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Model;
using ViewMeanings = View.Meanings;
using ModelMeanings = Model.Meanings;

namespace VocabularyAPI.Meanings
{
    public sealed class MeaningsService : IMeaningsService
    {
        private readonly IVocabulary vocabulary;
        private readonly IMapper mapper;

        public MeaningsService(IVocabulary vocabulary, IMapper mapper)
        {
            this.vocabulary = vocabulary ?? throw new ArgumentNullException(nameof(vocabulary));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ViewMeanings.Meaning> CreateMeaningAsync(
            string lemma,
            ViewMeanings.MeaningCreateInfo viewCreateInfo,
            CancellationToken token)
        {
            var modelCreateInfo = this.mapper
                .Map<ViewMeanings.MeaningCreateInfo, ModelMeanings.MeaningCreateInfo>(viewCreateInfo);
            ValidateOnCreate(modelCreateInfo, lemma);

            var modelMeaning = await this.vocabulary.CreateMeaningAsync(lemma, modelCreateInfo, token);

            var viewMeaning = this.mapper.Map<ModelMeanings.Meaning, ViewMeanings.Meaning>(modelMeaning);
            return viewMeaning;
        }

        public async Task UpdateMeaningAsync(
            string lemma,
            string meaningId,
            ViewMeanings.MeaningUpdateInfo viewUpdateInfo,
            CancellationToken token)
        {
            var modelUpdateInfo = this.mapper
                .Map<ViewMeanings.MeaningUpdateInfo, ModelMeanings.MeaningUpdateInfo>(viewUpdateInfo);
            ValidateOnUpdate(modelUpdateInfo, lemma);
            await this.vocabulary.UpdateMeaningAsync(lemma, meaningId, modelUpdateInfo, token);
        }

        public async Task DeleteMeaningAsync(
            string lemma,
            string meaningId,
            CancellationToken token)
        {
            await this.vocabulary.DeleteMeaningAsync(lemma, meaningId, token);
        }

        private static void ValidateOnCreate(ModelMeanings.MeaningCreateInfo createInfo, string lemma)
        {
            if (!createInfo.Example.Contains(lemma))
                throw new ValidationException("Example must contain lemma.");
        }

        private static void ValidateOnUpdate(ModelMeanings.MeaningUpdateInfo updateInfo, string lemma)
        {
            if (!updateInfo.Example.Contains(lemma))
                throw new ValidationException("Example must contain lemma.");
        }
    }
}
