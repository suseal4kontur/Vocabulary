using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Model.Exceptions;
using Model.Meanings;
using Model.Entries;
using MongoDB.Driver;

namespace Model
{
    public sealed class Vocabulary : IVocabulary
    {
        private readonly IMongoCollection<Entry> entriesCollection;

        public Vocabulary(IMongoCollection<Entry> entriesCollection)
        {
            this.entriesCollection = entriesCollection ?? throw new ArgumentNullException(nameof(entriesCollection));

            InitializeFirstEntry();
        }

        public async Task<Entry> GetEntryAsync(string lemma, CancellationToken token)
        {
            var entry = await TryFindEntry(lemma, token);

            return entry ?? throw new EntryNotFoundException(lemma);
        }

        public async Task<Entry> CreateEntryAsync(EntryCreateInfo createInfo, CancellationToken token)
        {
            if (await TryFindEntry(createInfo.Lemma, token) != null)
                throw new EntryAlreadyExistsException(createInfo.Lemma);

            var entry = new Entry
            {
                Id = Guid.NewGuid().ToString(),
                Lemma = createInfo.Lemma,
                Meanings = GetMeanings(createInfo.Meanings),
                Forms = createInfo.Forms ?? new string[] { createInfo.Lemma },
                Synonyms = createInfo.Synonyms,
                AddedAt = createInfo.AddedAt ?? DateTime.UtcNow
            };

            await this.entriesCollection.InsertOneAsync(entry, cancellationToken: token);

            return entry;
        }

        public async Task<EntriesList> SearchEntriesAsync(EntriesSearchInfo searchInfo, CancellationToken token)
        {
            var builder = Builders<Entry>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(searchInfo.Form))
            {
                filter &= builder.Where(e => e.Forms.Any(s => s == searchInfo.Form));
            }

            if (!string.IsNullOrEmpty(searchInfo.Synonym))
            {
                filter &= builder.Where(e => e.Synonyms.Any(s => s == searchInfo.Synonym));
            }

            if (searchInfo.FromAddedAt != null)
            {
                filter &= builder.Gte(e => e.AddedAt, searchInfo.FromAddedAt);
            }

            if (searchInfo.ToAddedAt != null)
            {
                filter &= builder.Lt(e => e.AddedAt, searchInfo.ToAddedAt);
            }

            if (searchInfo.PartOfSpeech != null)
            {
                filter &= builder.Where(e => e.Meanings.Any(m => m.PartOfSpeech == searchInfo.PartOfSpeech));
            }

            var result = this.entriesCollection.Find(filter);
            var total = (int)await result.CountDocumentsAsync(token);

            var entries = await result
                .Limit(searchInfo.Limit)
                .Skip(searchInfo.Offset)
                .ToListAsync(token);

            return new EntriesList { Entries = entries, Total = total };
        }

        public async Task UpdateEntryAsync(string lemma, EntryUpdateInfo updateInfo, CancellationToken token)
        {
            var updates = new List<UpdateDefinition<Entry>>();

            if (updateInfo.Forms != null)
            {
                updates.Add(Builders<Entry>.Update.Set(e => e.Forms, updateInfo.Forms));
            }

            if (updateInfo.Synonyms != null)
            {
                updates.Add(Builders<Entry>.Update.Set(e => e.Synonyms, updateInfo.Synonyms));
            }

            var update = Builders<Entry>.Update.Combine(updates);

            var updateResult = await this.entriesCollection
                .UpdateOneAsync(e => e.Lemma == lemma, update, cancellationToken: token)
                .ConfigureAwait(false);

            if (updateResult.ModifiedCount == 0)
            {
                throw new EntryNotFoundException(lemma);
            }
        }

        public async Task DeleteEntryAsync(string lemma, CancellationToken token)
        {
            var deleteResult = await this.entriesCollection.DeleteOneAsync(e => e.Lemma == lemma, token);

            if (deleteResult.DeletedCount == 0)
            {
                throw new EntryNotFoundException(lemma);
            }
        }

        public async Task<Entry> GetEntryByMeaningAsync(string meaningId, CancellationToken token)
        {
            var filter = Builders<Entry>.Filter.Where(e => e.Meanings.Any(m => m.Id == meaningId));

            var entry = await this.entriesCollection
                .Find(filter)
                .FirstOrDefaultAsync(token)
                .ConfigureAwait(false);

            return entry ?? throw new MeaningNotFoundException(meaningId);
        }

        public async Task<Meaning> CreateMeaningAsync(string lemma, MeaningCreateInfo createInfo, CancellationToken token)
        {
            var meaning = GetMeanings(new MeaningCreateInfo[] { createInfo })[0];

            var update = Builders<Entry>.Update.AddToSet(e => e.Meanings, meaning);

            var updateResult = await this.entriesCollection
                .UpdateOneAsync(e => e.Lemma == lemma, update, cancellationToken: token)
                .ConfigureAwait(false);

            if (updateResult.ModifiedCount == 0)
            {
                throw new EntryNotFoundException(lemma);
            }

            return meaning;
        }

        public async Task UpdateMeaningAsync(string lemma, string meaningId, MeaningUpdateInfo updateInfo, CancellationToken token)
        {
            if (await TryFindEntry(lemma, token) == null)
                throw new EntryNotFoundException(lemma);

            var updates = new List<UpdateDefinition<Entry>>();

            if (updateInfo.Description != null)
            {
                updates.Add(Builders<Entry>.Update.Set(e => e.Meanings[-1].Description, updateInfo.Description));
            }

            if (updateInfo.Example != null)
            {
                updates.Add(Builders<Entry>.Update.Set(e => e.Meanings[-1].Example, updateInfo.Example));
            }

            if (updateInfo.PartOfSpeech != null)
            {
                updates.Add(Builders<Entry>.Update.Set(e => e.Meanings[-1].PartOfSpeech, updateInfo.PartOfSpeech));
            }

            var update = Builders<Entry>.Update.Combine(updates);

            var filter = Builders<Entry>.Filter.Eq(e => e.Lemma, lemma)
                & Builders<Entry>.Filter.ElemMatch(e => e.Meanings, Builders<Meaning>.Filter.Eq(m => m.Id, meaningId));

            var updateResult = await this.entriesCollection
                .UpdateOneAsync(filter, update, cancellationToken: token)
                .ConfigureAwait(false);

            if (updateResult.ModifiedCount == 0)
            {
                throw new MeaningNotFoundException(meaningId);
            }
        }

        public async Task DeleteMeaningAsync(string lemma, string meaningId, CancellationToken token)
        {
            var entry = await TryFindEntry(lemma, token);

            if (entry == null)
                throw new EntryNotFoundException(lemma);

            if (entry.Meanings.Count == 1)
                throw new SingleMeaningDeletionException(meaningId);

            var update = Builders<Entry>.Update
                .PullFilter(e => e.Meanings, Builders<Meaning>.Filter.Eq(m => m.Id, meaningId));

            var filter = Builders<Entry>.Filter.Eq(e => e.Lemma, lemma);

            var updateResult = await entriesCollection
                .UpdateOneAsync(filter, update, cancellationToken: token)
                .ConfigureAwait(false);

            if (updateResult.ModifiedCount == 0)
            {
                throw new MeaningNotFoundException(meaningId);
            }
        }

        private void InitializeFirstEntry()
        {
            if (this.entriesCollection
                .Find(e => e.Lemma == "vocabulary")
                .FirstOrDefaultAsync()
                .GetAwaiter()
                .GetResult() == null)
            {
                CreateEntryAsync(
                        new EntryCreateInfo()
                        {
                            Lemma = "vocabulary",
                            Meanings = new MeaningCreateInfo[]
                            {
                            new MeaningCreateInfo()
                            {
                                PartOfSpeech = PartOfSpeech.Noun,
                                Description = "The body of words used in a particular language.",
                                Example = "The term became part of business vocabulary."
                            }
                            }
                        },
                        CancellationToken.None)
                    .GetAwaiter()
                    .GetResult();
            }
        }

        private static IReadOnlyList<Meaning> GetMeanings(IReadOnlyList<MeaningCreateInfo> createInfos)
        {
            var meanings = new List<Meaning>();

            foreach (var createInfo in createInfos)
            {
                meanings.Add(new Meaning
                {
                    Id = Guid.NewGuid().ToString(),
                    PartOfSpeech = createInfo.PartOfSpeech,
                    Description = createInfo.Description,
                    Example = createInfo.Example
                });
            }

            return meanings;
        }

        private async Task<Entry> TryFindEntry(string lemma, CancellationToken token)
        {
            return await this.entriesCollection
                .Find(e => e.Lemma == lemma)
                .FirstOrDefaultAsync(token)
                .ConfigureAwait(false);
        }
    }
}
