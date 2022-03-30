using NUnit.Framework;
using FluentAssertions;
using System.Threading.Tasks;
using Client;
using View.Entries;

namespace ClientTests.Tests
{
    [TestFixture]
    internal sealed class UpdateEntryTests
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
        public async Task UpdateEntryCorrectlyTest()
        {
            var updateInfo = new EntryUpdateInfo()
            {
                Forms = new string[]
                {
                    "vocabulary",
                    "vocabularies"
                },
                Synonyms = new string[]
                {
                    "dictionary"
                }
            };

            var result = await this.vocabularyClient.UpdateEntryAsync("vocabulary", updateInfo);

            result.IsSuccessful().Should().BeTrue();

            var entry = (await this.vocabularyClient.GetEntryAsync("vocabulary")).Response;

            entry.Forms.Should().BeEquivalentTo(updateInfo.Forms);
            entry.Synonyms.Should().BeEquivalentTo(updateInfo.Synonyms);
        }

        [Test]
        public async Task UpdateEntryWithIncorrectLemmaTest()
        {
            var updateInfo = new EntryUpdateInfo()
            {
                Forms = new string[]
                {
                    "fff"
                }
            };

            var result = await this.vocabularyClient.UpdateEntryAsync("fff", updateInfo);

            result.IsSuccessful().Should().BeFalse();
            result.Error.Title.Should().Contain("fff");
        }

        [Test]
        public async Task UpdateEntryWithEmptyUpdateInfoTest()
        {
            var updateInfo = new EntryUpdateInfo();

            var result = await this.vocabularyClient.UpdateEntryAsync("vocabulary", updateInfo);

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task UpdateEntryWithIncorrectFormsTest()
        {
            var updateInfo = new EntryUpdateInfo()
            {
                Forms = new string[]
                {
                    "fff"
                }
            };

            var result = await this.vocabularyClient.UpdateEntryAsync("vocabulary", updateInfo);

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task UpdateEntryWithRepeatingFormsTest()
        {
            var updateInfo = new EntryUpdateInfo()
            {
                Forms = new string[]
                {
                    "vocabulary",
                    "vocabularies",
                    "vocabularies"
                }
            };

            var result = await this.vocabularyClient.UpdateEntryAsync("vocabulary", updateInfo);

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task UpdateEntryWithIncorrectSynonymsTest()
        {
            var updateInfo = new EntryUpdateInfo()
            {
                Synonyms = new string[]
                {
                    "vocabulary"
                }
            };

            var result = await this.vocabularyClient.UpdateEntryAsync("vocabulary", updateInfo);

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task UpdateEntryWithRepeatingSynonymsTest()
        {
            var updateInfo = new EntryUpdateInfo()
            {
                Synonyms = new string[]
                {
                    "dictionary",
                    "dictionary"
                }
            };

            var result = await this.vocabularyClient.UpdateEntryAsync("vocabulary", updateInfo);

            result.IsSuccessful().Should().BeFalse();
        }

        [TearDown]
        public void TearDown()
        {
            vocabularyCleaner.DropEntries();
        }
    }
}
