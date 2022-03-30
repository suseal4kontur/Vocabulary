using NUnit.Framework;
using FluentAssertions;
using System.Threading.Tasks;
using Client;
using View.Meanings;

namespace ClientTests.Tests
{
    [TestFixture]
    internal sealed class UpdateMeaningTests
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
        public async Task UpdateMeaningCorrectlyTest()
        {
            var meaningId = (await this.vocabularyClient.GetEntryAsync("vocabulary"))
                .Response.Meanings[0].Id;

            var updateInfo = new MeaningUpdateInfo()
            {
                PartOfSpeech = "Noun",
                Description = "A new amazing API.",
                Example = "Look at this API! It implements a vocabulary!"
            };

            var result = await this.vocabularyClient.UpdateMeaningAsync("vocabulary", meaningId, updateInfo);

            result.IsSuccessful().Should().BeTrue();

            var entry = (await this.vocabularyClient.GetEntryAsync("vocabulary")).Response;

            entry.Meanings[0].Id.Should().Be(meaningId);

            entry.Meanings[0].Should().Match<Meaning>(m => m.PartOfSpeech == updateInfo.PartOfSpeech
                && m.Description == updateInfo.Description && m.Example == updateInfo.Example);
        }

        [Test]
        public async Task UpdateMeaningWithIncorrectLemmaTest()
        {
            var meaningId = (await this.vocabularyClient.GetEntryAsync("vocabulary"))
                .Response.Meanings[0].Id;

            var updateInfo = new MeaningUpdateInfo()
            {
                PartOfSpeech = "Noun",
                Description = "A new amazing API.",
                Example = "Look at this API! It implements an fff!"
            };

            var result = await this.vocabularyClient.UpdateMeaningAsync("fff", meaningId, updateInfo);

            result.IsSuccessful().Should().BeFalse();
            result.Error.Title.Should().Contain("fff");
        }

        [Test]
        public async Task UpdateMeaningWithIncorrectMeaningIdTest()
        {
            var meaningId = "fff";

            var updateInfo = new MeaningUpdateInfo()
            {
                PartOfSpeech = "Noun",
                Description = "A new amazing API.",
                Example = "Look at this API! It implements a vocabulary!"
            };

            var result = await this.vocabularyClient.UpdateMeaningAsync("vocabulary", meaningId, updateInfo);

            result.IsSuccessful().Should().BeFalse();
            result.Error.Title.Should().Contain(meaningId);
        }

        [Test]
        public async Task UpdateMeaningWithIncorrectInfoTest()
        {
            var meaningId = (await this.vocabularyClient.GetEntryAsync("vocabulary"))
                .Response.Meanings[0].Id;

            var updateInfo = new MeaningUpdateInfo()
            {
                PartOfSpeech = "Noun",
                Description = "A new amazing API.",
                Example = "Look at this API! It implements... something!"
            };

            var result = await this.vocabularyClient.UpdateMeaningAsync("vocabulary", meaningId, updateInfo);

            result.IsSuccessful().Should().BeFalse();
        }

        [TearDown]
        public void TearDown()
        {
            vocabularyCleaner.DropEntries();
        }
    }
}
