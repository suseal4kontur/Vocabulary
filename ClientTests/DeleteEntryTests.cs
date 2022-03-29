using NUnit.Framework;
using FluentAssertions;
using System.Threading.Tasks;
using Client;

namespace ClientTests
{
    [TestFixture]
    internal sealed class DeleteEntryTests
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
        public async Task DeleteEntriesCorrectlyTest()
        {
            var entriesCreateInfo = DataProvider.GetData();

            foreach (var createInfo in entriesCreateInfo)
                await this.vocabularyClient.CreateEntryAsync(createInfo);

            foreach (var createInfo in entriesCreateInfo)
            {
                var result = await this.vocabularyClient.DeleteEntryAsync(createInfo.Lemma);

                result.IsSuccessful().Should().BeTrue();
            }
        }

        [Test]
        public async Task DeleteEntryWithIncorrectLemmaTest()
        {
            var lemma = "fff";
            var result = await this.vocabularyClient.DeleteEntryAsync(lemma);

            result.IsSuccessful().Should().BeFalse();
            result.Error.Title.Should().Contain(lemma);
        }
    }
}
