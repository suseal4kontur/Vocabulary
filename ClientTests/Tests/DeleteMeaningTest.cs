using NUnit.Framework;
using FluentAssertions;
using System.Threading.Tasks;
using Client;

namespace ClientTests.Tests
{
    [TestFixture]
    internal sealed class DeleteMeaningTest
    {
        private IVocabularyClient vocabularyClient;
        private IVocabularyCleaner vocabularyCleaner;

        [OneTimeSetUp]
        public void Setup()
        {
            vocabularyClient = new VocabularyClient("https://localhost:5001/");
            vocabularyCleaner = new VocabularyCleaner("mongodb://localhost:27017");
            vocabularyCleaner.DropEntries();

            var entriesCreateInfo = DataProvider.GetData();

            foreach (var createInfo in entriesCreateInfo)
                vocabularyClient.CreateEntryAsync(createInfo).GetAwaiter().GetResult();
        }

        [Test]
        public async Task DeleteMeaningCorrectlyTest()
        {
            var meaningId = (await this.vocabularyClient.GetEntryAsync("play"))
                .Response.Meanings[0].Id;

            var result = await this.vocabularyClient.DeleteMeaningAsync("play", meaningId);

            result.IsSuccessful().Should().BeTrue();

            (await this.vocabularyClient.GetEntryByMeaningAsync(meaningId))
                .IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task DeleteMeaningWithIncorrectLemmaTest()
        {
            var meaningId = (await this.vocabularyClient.GetEntryAsync("play"))
                .Response.Meanings[0].Id;

            var result = await this.vocabularyClient.DeleteMeaningAsync("fff", meaningId);

            result.IsSuccessful().Should().BeFalse();
            result.Error.Title.Should().Contain("fff");
        }

        [Test]
        public async Task DeleteMeaningWithIncorrectMeaningIdTest()
        {
            var meaningId = "fff";

            var result = await this.vocabularyClient.DeleteMeaningAsync("play", meaningId);

            result.IsSuccessful().Should().BeFalse();
            result.Error.Title.Should().Contain(meaningId);
        }

        [Test]
        public async Task DeleteMeaningFromLemmaWithSingleMeaningTest()
        {
            var meaningId = (await this.vocabularyClient.GetEntryAsync("vocabulary"))
                .Response.Meanings[0].Id;

            var result = await this.vocabularyClient.DeleteMeaningAsync("vocabulary", meaningId);

            result.IsSuccessful().Should().BeFalse();
            result.Error.Title.Should().Contain(meaningId);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            vocabularyCleaner.DropEntries();
        }
    }
}
