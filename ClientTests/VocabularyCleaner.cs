using MongoDB.Driver;
using System;
using Model.Entries;
using Model.Meanings;

namespace ClientTests
{
    internal sealed class VocabularyCleaner : IVocabularyCleaner
    {
        private readonly IMongoDatabase database;

        public VocabularyCleaner(string connectionString)
        {
            var client = new MongoClient(connectionString);
            database = client.GetDatabase("vocabulary");
        }

        public void DropEntries()
        {
            database.DropCollectionAsync("entries").GetAwaiter().GetResult();

            InitializeFirstEntry();
        }

        public  void RemoveFromEntries(Func<Entry, bool> condition)
        {
            database.GetCollection<Entry>("entries")
                .DeleteManyAsync(Builders<Entry>.Filter.Where(t => condition(t)))
                .GetAwaiter()
                .GetResult();

            InitializeFirstEntry();
        }

        private void InitializeFirstEntry()
        {
            if (database.GetCollection<Entry>("entries")
                .Find(e => e.Lemma == "vocabulary")
                .FirstOrDefaultAsync()
                .GetAwaiter()
                .GetResult() == null)
            {
                var entry = new Entry
                {
                    Id = Guid.NewGuid().ToString(),
                    Lemma = "vocabulary",
                    Meanings = new Meaning[]
                    {
                        new Meaning()
                        {
                            PartOfSpeech = PartOfSpeech.Noun,
                            Description = "The body of words used in a particular language.",
                            Example = "The term became part of business vocabulary."
                        }
                    },
                    Forms = new string[] { "vocabulary" },
                    AddedAt = DateTime.UtcNow
                };

                database.GetCollection<Entry>("entries")
                    .InsertOneAsync(entry)
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}
