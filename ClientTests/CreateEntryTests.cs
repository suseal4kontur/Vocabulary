using NUnit.Framework;
using FluentAssertions;
using System.Threading.Tasks;
using Client;
using View.Entries;
using View.Meanings;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System;

namespace ClientTests
{
    [TestFixture]
    internal sealed class CreateEntryTests
    {
        private IVocabularyClient vocabularyClient;
        private IVocabularyCleaner vocabularyCleaner;

        [SetUp]
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
            }
        }

        [Test]
        public async Task CreateEntryWithIncorrectMeaning()
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
        public async Task CreateEntryWithoutMeaning()
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
    }
}
