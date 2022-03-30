using NUnit.Framework;
using FluentAssertions;
using System.Threading.Tasks;
using Client;
using View.Meanings;

namespace ClientTests.Tests
{
    [TestFixture]
    internal sealed class CreateMeaningTests
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
        public async Task CreateCorrectMeaningTest()
        {
            var createInfo = new MeaningCreateInfo()
            {
                PartOfSpeech = "Noun",
                Description = "A new amazing API.",
                Example = "Look at this API! It implements a vocabulary!"
            };

            var result = await this.vocabularyClient.CreateMeaningAsync("vocabulary", createInfo);

            result.IsSuccessful().Should().BeTrue();

            var entry = (await this.vocabularyClient.GetEntryAsync("vocabulary")).Response;

            entry.Meanings.Should().Contain(m => m.PartOfSpeech == createInfo.PartOfSpeech
                && m.Description == createInfo.Description && m.Example == createInfo.Example);
        }

        [Test]
        public async Task CreateMeaningWithIncorrectLemmaTest()
        {
            var createInfo = new MeaningCreateInfo()
            {
                PartOfSpeech = "Noun",
                Description = "A new amazing API.",
                Example = "Look at this API! It implements an fff!"
            };

            var result = await this.vocabularyClient.CreateMeaningAsync("fff", createInfo);

            result.IsSuccessful().Should().BeFalse();
            result.Error.Title.Should().Contain("fff");
        }

        [Test]
        public async Task CreateMeaningWithIncorrectInfoTest()
        {
            var createInfo = new MeaningCreateInfo()
            {
                PartOfSpeech = "Noun",
                Description = "A new amazing API.",
                Example = "Look at this API! It implements... something!"
            };

            var result = await this.vocabularyClient.CreateMeaningAsync("vocabulary", createInfo);

            result.IsSuccessful().Should().BeFalse();
        }

        [TearDown]
        public void TearDown()
        {
            vocabularyCleaner.DropEntries();
        }
    }
}
