using NUnit.Framework;
using FluentAssertions;
using System.Threading.Tasks;
using Client;

namespace ClientTests.Tests
{
    [TestFixture]
    internal sealed class GetEntryByMeaningTests
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
        public async Task GetEntryByMeaningIdTest()
        {
            var entry = (await this.vocabularyClient.GetEntryAsync("vocabulary")).Response;

            var meaningId = entry.Meanings[0].Id;

            var result = await this.vocabularyClient.GetEntryByMeaningAsync(meaningId);

            result.IsSuccessful().Should().BeTrue();
            result.Response.Should().BeEquivalentTo(entry);
        }

        [Test]
        public async Task GetEntryByIncorrectMeaningIdTest()
        {
            var meaningId = "fff";

            var result = await this.vocabularyClient.GetEntryByMeaningAsync(meaningId);

            result.IsSuccessful().Should().BeFalse();
            result.Error.Title.Should().Contain(meaningId);
        }
    }
}
