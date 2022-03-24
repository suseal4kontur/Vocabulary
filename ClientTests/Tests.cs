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
using System.Linq;

namespace ClientTests
{
    [TestFixture]
    internal sealed class Tests
    {
        private IVocabularyClient vocabularyClient;
        private List<EntryCreateInfo> entriesCreateInfo;

        [OneTimeSetUp]
        public void Setup()
        {
            vocabularyClient = new VocabularyClient();
            entriesCreateInfo = GetData();
        }

        [Test, Order(1)]
        public async Task GetCorrectEntryByLemmaTest()
        {
            var lemma = "vocabulary";
            var result = await this.vocabularyClient.GetEntryAsync(lemma);

            result.IsSuccessful().Should().BeTrue();
            result.Response.Lemma.Should().Be(lemma);
        }

        [Test, Order(2)]
        public async Task GetIncorrectEntryByLemmaTest()
        {
            var lemma = "fff";
            var result = await this.vocabularyClient.GetEntryAsync(lemma);

            result.IsSuccessful().Should().BeFalse();
            result.Error.Title.Should().Contain(lemma);
        }

        [Test, Order(3)]
        public async Task CreateEntriesCorrectlyTest()
        {
            foreach (var createInfo in entriesCreateInfo)
            {
                var result = await this.vocabularyClient.CreateEntryAsync(createInfo);

                result.IsSuccessful().Should().BeTrue();
                IsCreatedCorrectly(result.Response, createInfo).Should().BeTrue();
            }
        }

        [Test, Order(4)]
        public async Task CreateEntriesIncorrectlyTestOne()
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

        [Test, Order(4)]
        public async Task CreateEntriesIncorrectlyTestTwo()
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

        [Test, Order(4)]
        public async Task CreateEntriesIncorrectlyTestThree()
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
        }

        [Test, Order(5)]
        public async Task DeleteEntriesCorrectlyTest()
        {
            foreach (var createInfo in entriesCreateInfo)
            {
                var result = await this.vocabularyClient.DeleteEntryAsync(createInfo.Lemma);

                result.IsSuccessful().Should().BeTrue();
            }
        }

        [Test, Order(6)]
        public async Task DeleteEntriesIncorrectlyTest()
        {
            var lemma = "fff";
            var result = await this.vocabularyClient.DeleteEntryAsync(lemma);

            result.IsSuccessful().Should().BeFalse();
            result.Error.Title.Should().Contain(lemma);
        }

        private static List<EntryCreateInfo> GetData()
        {
            var directory = new DirectoryInfo("Data");
            var files = directory.GetFiles();
            var data = new List<EntryCreateInfo>();

            foreach (var file in files)
            {
                using var streamReader = new StreamReader(file.OpenRead());
                var content = streamReader.ReadToEnd();

                var entry = JsonSerializer.Deserialize<EntryCreateInfo>(content);
                data.Add(entry);
            }

            return data;
        }

        private bool IsCreatedCorrectly(Entry createdEntry, EntryCreateInfo createInfo)
        {
            if (createdEntry.Lemma != createInfo.Lemma
                || createInfo.AddedAt != null && createdEntry.AddedAt != createInfo.AddedAt)
                return false;

            if (createInfo.Forms != null)
                foreach (var formInfo in createInfo.Forms)
                {
                    var form = createdEntry.Forms
                        .Where(f => f == formInfo)
                        .FirstOrDefault();

                    if (form == null)
                        return false;
                }

            if (createInfo.Synonyms != null)
                foreach (var synonymInfo in createInfo.Synonyms)
                {
                    var synonym = createdEntry.Synonyms
                        .Where(s => s == synonymInfo)
                        .FirstOrDefault();

                    if (synonym == null)
                        return false;
                }

            foreach (var meaningCreateInfo in createInfo.Meanings)
            {
                var meaning = createdEntry.Meanings
                    .Where(m => m.Description == meaningCreateInfo.Description)
                    .FirstOrDefault();

                if (meaning == null
                    || meaning.Example != meaningCreateInfo.Example
                    || meaning.PartOfSpeech != meaningCreateInfo.PartOfSpeech)
                    return false;
            }

            return true;
        }
    }
}