using NUnit.Framework;
using FluentAssertions;
using System.Threading.Tasks;
using Client;
using View.Entries;
using View.Meanings;
using System;
using System.Text;

namespace ClientTests.Tests
{
    [TestFixture]
    internal sealed class CreateEntryTests
    {
        private IVocabularyClient vocabularyClient;
        private IVocabularyCleaner vocabularyCleaner;

        [OneTimeSetUp]
        public void Setup()
        {
            vocabularyClient = new VocabularyClient("https://localhost:5001/");
            vocabularyCleaner = new VocabularyCleaner("mongodb://localhost:27017");
            vocabularyCleaner.DropEntries();
        }

        [Test]
        public async Task CreateEntriesCorrectlyTest()
        {
            var entriesCreateInfo = DataProvider.GetData();

            foreach (var createInfo in entriesCreateInfo)
            {
                var result = await this.vocabularyClient.CreateEntryAsync(createInfo);

                result.IsSuccessful().Should().BeTrue();
                TestIfCreatedCorrectly(result.Response, createInfo);

                (await this.vocabularyClient.GetEntryAsync(createInfo.Lemma))
                    .IsSuccessful().Should().BeTrue();
            }
        }

        [Test]
        public async Task CreateEntryWithIncorrectMeaningTest()
        {
            var meaningCreateInfo = new MeaningCreateInfo()
            {
                PartOfSpeech = "Adverb",
                Description = null,
                Example = "word"
            };

            var result = await this.vocabularyClient.CreateEntryAsync(new EntryCreateInfo()
            {
                Lemma = "word",
                Meanings = new MeaningCreateInfo[] { meaningCreateInfo }
            });

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task CreateEntryWithoutMeaningTest()
        {
            var result = await this.vocabularyClient.CreateEntryAsync(new EntryCreateInfo()
            {
                Lemma = "word",
                Meanings = null
            });

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task CreateEntryWithIncorrectLemmaTest()
        {
            var meaningCreateInfo = new MeaningCreateInfo()
            {
                PartOfSpeech = "Adverb",
                Description = "word",
                Example = "word"
            };

            var result = await this.vocabularyClient.CreateEntryAsync(new EntryCreateInfo()
            {
                Lemma = null,
                Meanings = new MeaningCreateInfo[] { meaningCreateInfo }
            });

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task CreateEntryWithIncorrectFormsTest()
        {
            var meaningCreateInfo = new MeaningCreateInfo()
            {
                PartOfSpeech = "Adverb",
                Description = "word",
                Example = "word"
            };

            var result = await this.vocabularyClient.CreateEntryAsync(new EntryCreateInfo()
            {
                Lemma = "word",
                Meanings = new MeaningCreateInfo[] { meaningCreateInfo },
                Forms = new string[] { "words" }
            });

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task CreateEntryWithRepeatingFormsTest()
        {
            var meaningCreateInfo = new MeaningCreateInfo()
            {
                PartOfSpeech = "Adverb",
                Description = "word",
                Example = "word"
            };

            var result = await this.vocabularyClient.CreateEntryAsync(new EntryCreateInfo()
            {
                Lemma = "word",
                Meanings = new MeaningCreateInfo[] { meaningCreateInfo },
                Forms = new string[] { "word", "words", "words" }
            });

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task CreateEntryWithIncorrectSynonymsTest()
        {
            var meaningCreateInfo = new MeaningCreateInfo()
            {
                PartOfSpeech = "Adverb",
                Description = "word",
                Example = "word"
            };

            var result = await this.vocabularyClient.CreateEntryAsync(new EntryCreateInfo()
            {
                Lemma = "word",
                Meanings = new MeaningCreateInfo[] { meaningCreateInfo },
                Synonyms = new string[] { "word" }
            });

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task CreateEntryWithRepeatingSynonymsTest()
        {
            var meaningCreateInfo = new MeaningCreateInfo()
            {
                PartOfSpeech = "Adverb",
                Description = "word",
                Example = "word"
            };

            var result = await this.vocabularyClient.CreateEntryAsync(new EntryCreateInfo()
            {
                Lemma = "word",
                Meanings = new MeaningCreateInfo[] { meaningCreateInfo },
                Synonyms = new string[] { "lemma", "lemma" }
            });

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task CreateEntryThatAlreadyExistsTest()
        {
            var meaningCreateInfo = new MeaningCreateInfo()
            {
                PartOfSpeech = "Adverb",
                Description = "word",
                Example = "word"
            };

            var result = await this.vocabularyClient.CreateEntryAsync(new EntryCreateInfo()
            {
                Lemma = "vocabulary",
                Meanings = new MeaningCreateInfo[] { meaningCreateInfo }
            });

            result.IsSuccessful().Should().BeFalse();
            result.Error.Title.Should().Contain("vocabulary");
        }

        [Test]
        public async Task CreateEntryWithLongLemmaTest()
        {
            var meaningCreateInfo = new MeaningCreateInfo()
            {
                PartOfSpeech = "Adverb",
                Description = "word",
                Example = "fffffffffffffffffffffffffffffffffff"
            };

            var result = await this.vocabularyClient.CreateEntryAsync(new EntryCreateInfo()
            {
                Lemma = "fffffffffffffffffffffffffffffffffff",
                Meanings = new MeaningCreateInfo[] { meaningCreateInfo }
            });

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task CreateEntryWithIncorrectPartOfSpeechTest()
        {
            var meaningCreateInfo = new MeaningCreateInfo()
            {
                PartOfSpeech = "fff",
                Description = "word",
                Example = "word"
            };

            var result = await this.vocabularyClient.CreateEntryAsync(new EntryCreateInfo()
            {
                Lemma = "word",
                Meanings = new MeaningCreateInfo[] { meaningCreateInfo }
            });

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task CreateEntryWithLongDescriptionTest()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append('f', 1001);

            var meaningCreateInfo = new MeaningCreateInfo()
            {
                PartOfSpeech = "Adverb",
                Description = stringBuilder.ToString(),
                Example = "word"
            };

            var result = await this.vocabularyClient.CreateEntryAsync(new EntryCreateInfo()
            {
                Lemma = "word",
                Meanings = new MeaningCreateInfo[] { meaningCreateInfo }
            });

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task CreateEntryWithLongExampleTest()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("word ");
            stringBuilder.Append('f', 100);

            var meaningCreateInfo = new MeaningCreateInfo()
            {
                PartOfSpeech = "Adverb",
                Description = "word",
                Example = stringBuilder.ToString()
            };

            var result = await this.vocabularyClient.CreateEntryAsync(new EntryCreateInfo()
            {
                Lemma = "word",
                Meanings = new MeaningCreateInfo[] { meaningCreateInfo }
            });

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task CreateEntryWithLongFormTest()
        {
            var meaningCreateInfo = new MeaningCreateInfo()
            {
                PartOfSpeech = "Adverb",
                Description = "word",
                Example = "word"
            };

            var result = await this.vocabularyClient.CreateEntryAsync(new EntryCreateInfo()
            {
                Lemma = "word",
                Meanings = new MeaningCreateInfo[] { meaningCreateInfo },
                Forms = new string[] { "word", "fffffffffffffffffffffffffffffffffff" }
            });

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task CreateEntryWithLongSynonymTest()
        {
            var meaningCreateInfo = new MeaningCreateInfo()
            {
                PartOfSpeech = "Adverb",
                Description = "word",
                Example = "word"
            };

            var result = await this.vocabularyClient.CreateEntryAsync(new EntryCreateInfo()
            {
                Lemma = "word",
                Meanings = new MeaningCreateInfo[] { meaningCreateInfo },
                Synonyms = new string[] { "lemma", "fffffffffffffffffffffffffffffffffff" }
            });

            result.IsSuccessful().Should().BeFalse();
        }

        private static void TestIfCreatedCorrectly(Entry createdEntry, EntryCreateInfo createInfo)
        {
            createdEntry.Lemma.Should().Be(createInfo.Lemma);

            if (createInfo.Forms != null)
                createdEntry.Forms.Should().BeEquivalentTo(createInfo.Forms);
            else
                createdEntry.Forms.Should().BeEquivalentTo(new string[] { createInfo.Lemma });

            createdEntry.Synonyms.Should().BeEquivalentTo(createInfo.Synonyms);

            if (createInfo.AddedAt != null)
                createdEntry.AddedAt.Should().Be(createInfo.AddedAt);

            foreach (var meaningInfo in createInfo.Meanings)
            {
                meaningInfo.PartOfSpeech = Enum.GetName(typeof(PartOfSpeech),
                    Enum.Parse(typeof(PartOfSpeech), meaningInfo.PartOfSpeech, true));
            }

            createdEntry.Meanings.Should().BeEquivalentTo(createInfo.Meanings);
        }

        [TearDown]
        public void TearDown()
        {
            vocabularyCleaner.DropEntries();
        }
    }
}
