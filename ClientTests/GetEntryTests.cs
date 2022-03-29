using NUnit.Framework;
using FluentAssertions;
using System.Threading.Tasks;
using Client;

namespace ClientTests
{
    [TestFixture]
    internal sealed class GetEntryTests
    {
        private IVocabularyClient vocabularyClient;

        [OneTimeSetUp]
        public void Setup()
        {
            vocabularyClient = new VocabularyClient("https://localhost:5001/");
        }

        [Test]
        public async Task GetCorrectEntryByLemmaTest()
        {
            var lemma = "vocabulary";
            var result = await this.vocabularyClient.GetEntryAsync(lemma);

            result.IsSuccessful().Should().BeTrue();
            result.Response.Lemma.Should().Be(lemma);
        }

        [Test]
        public async Task GetIncorrectEntryByLemmaTest()
        {
            var lemma = "fff";
            var result = await this.vocabularyClient.GetEntryAsync(lemma);

            result.IsSuccessful().Should().BeFalse();
            result.Error.Title.Should().Contain(lemma);
        }
    }
}
