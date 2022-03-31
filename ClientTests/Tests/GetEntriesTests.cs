using NUnit.Framework;
using FluentAssertions;
using System.Threading.Tasks;
using Client;

namespace ClientTests.Tests
{
    [TestFixture]
    internal sealed class GetEntriesTests
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
        public async Task GetAllEntriesTest()
        {
            var result = await this.vocabularyClient.GetEntriesAsync();

            result.IsSuccessful().Should().BeTrue();
            result.Response.Entries.Should().HaveCount(10);
        }

        [Test]
        public async Task GetEntriesByPartOfSpeechTest()
        {
            var result = await this.vocabularyClient.GetEntriesAsync(
                partOfSpeech: "verb");

            result.IsSuccessful().Should().BeTrue();
            result.Response.Entries.Should().HaveCount(7);

            foreach (var entryShortInfo in result.Response.Entries)
                entryShortInfo.Meanings.Should().Contain(m => m.PartOfSpeech == "Verb");
        }

        [Test]
        public async Task GetEntriesByFormTest()
        {
            var result = await this.vocabularyClient.GetEntriesAsync(
                form: "thought");

            result.IsSuccessful().Should().BeTrue();
            result.Response.Entries.Should().HaveCount(2);

            foreach (var entryShortInfo in result.Response.Entries)
                (await this.vocabularyClient.GetEntryAsync(entryShortInfo.Lemma))
                    .Response.Forms.Should().Contain("thought");
        }

        [Test]
        public async Task GetEntriesBySynonymTest()
        {
            var result = await this.vocabularyClient.GetEntriesAsync(
                synonym: "have fun");

            result.IsSuccessful().Should().BeTrue();
            result.Response.Entries.Should().HaveCount(2);

            foreach (var entryShortInfo in result.Response.Entries)
                (await this.vocabularyClient.GetEntryAsync(entryShortInfo.Lemma))
                    .Response.Synonyms.Should().Contain("have fun");
        }

        [Test]
        public async Task GetEntriesByDatesTest()
        {
            var result = await this.vocabularyClient.GetEntriesAsync(
                fromAddedAt: new System.DateTime(2022, 03, 22),
                toAddedAt: System.DateTime.Today);

            result.IsSuccessful().Should().BeTrue();
            result.Response.Entries.Should().HaveCount(2);

            foreach (var entryShortInfo in result.Response.Entries)
            {
                entryShortInfo.AddedAt.Should().NotBeBefore(new System.DateTime(2022, 03, 22));
                entryShortInfo.AddedAt.Should().BeBefore(System.DateTime.Today.ToUniversalTime());
            }
        }

        [Test]
        public async Task GetEntriesWithOffsetTest()
        {
            var result = await this.vocabularyClient.GetEntriesAsync(
                offset: 2);

            result.IsSuccessful().Should().BeTrue();
            result.Response.Entries.Should().HaveCount(8);
            result.Response.Total.Should().Be(10);
        }

        [Test]
        public async Task GetEntriesWithLimitTest()
        {
            var result = await this.vocabularyClient.GetEntriesAsync(
                limit: 3);

            result.IsSuccessful().Should().BeTrue();
            result.Response.Entries.Should().HaveCount(3);
            result.Response.Total.Should().Be(10);
        }

        [Test]
        public async Task GetEntriesByMultipleFieldsTest()
        {
            var result = await this.vocabularyClient.GetEntriesAsync(
                partOfSpeech: "Noun",
                fromAddedAt: System.DateTime.Today);

            result.IsSuccessful().Should().BeTrue();
            result.Response.Entries.Should().HaveCount(4);

            foreach (var entryShortInfo in result.Response.Entries)
            {
                entryShortInfo.Meanings.Should().Contain(m => m.PartOfSpeech == "Noun");
                entryShortInfo.AddedAt.Should().NotBeBefore(System.DateTime.Today.ToUniversalTime());
            }
        }

        [Test]
        public async Task GetEntriesWithIncorrectPartOfSpeechTest()
        {
            var result = await this.vocabularyClient.GetEntriesAsync(
                partOfSpeech: "fff");

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task GetEntriesWithLongFormTest()
        {
            var result = await this.vocabularyClient.GetEntriesAsync(
                form: "fffffffffffffffffffffffffffffffffff");

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task GetEntriesWithLongSynonymTest()
        {
            var result = await this.vocabularyClient.GetEntriesAsync(
                synonym: "fffffffffffffffffffffffffffffffffff");

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task GetEntriesWithIncorrectLimitTest()
        {
            var result = await this.vocabularyClient.GetEntriesAsync(
                limit: -3);

            result.IsSuccessful().Should().BeFalse();
        }

        [Test]
        public async Task GetEntriesWithIncorrectOffsetTest()
        {
            var result = await this.vocabularyClient.GetEntriesAsync(
                offset: -3);

            result.IsSuccessful().Should().BeFalse();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            vocabularyCleaner.DropEntries();
        }
    }
}
