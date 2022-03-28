using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Client.ClientResults;
using View.Entries;
using View.Meanings;

namespace Client
{
    public sealed class VocabularyClient : IVocabularyClient
    {
        private readonly HttpClient httpClient;

        public VocabularyClient(string uriString)
        {
            this.httpClient = new HttpClient
            {
                BaseAddress = new Uri(uriString)
            };
        }

        public async Task<ClientResult<Entry>> GetEntryAsync(string lemma)
        {
            var response = await this.httpClient.GetAsync($"entries/{lemma}");

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new ClientResult<Entry>((int)response.StatusCode,
                    JsonSerializer.Deserialize<Entry>(content));
            else
                return new ClientResult<Entry>((int)response.StatusCode,
                    JsonSerializer.Deserialize<ClientError>(content));
        }

        public async Task<ClientResult<EntriesList>> GetEntriesAsync(
            string partOfSpeech = null,
            string form = null,
            string synonym = null,
            DateTime? fromAddedAt = null,
            DateTime? toAddedAt = null,
            int? offset = null,
            int? limit = null)
        {
            var query = string.Join(
                '&',
                $"partOfSpeech={partOfSpeech}",
                $"form={form}",
                $"synonym={synonym}",
                $"fromAddedAt={fromAddedAt}",
                $"toAddedAt={toAddedAt}",
                $"offset={offset}",
                $"limit={limit}");

            var response = await this.httpClient.GetAsync("entries?" + query);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new ClientResult<EntriesList>((int)response.StatusCode,
                    JsonSerializer.Deserialize<EntriesList>(content));
            else
                return new ClientResult<EntriesList>((int)response.StatusCode,
                    JsonSerializer.Deserialize<ClientError>(content));
        }

        public async Task<ClientResult<Entry>> CreateEntryAsync(EntryCreateInfo createInfo)
        {
            var stringContent = new StringContent(JsonSerializer
                .Serialize(createInfo, typeof(EntryCreateInfo)), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"entries/", stringContent);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new ClientResult<Entry>((int)response.StatusCode,
                    JsonSerializer.Deserialize<Entry>(content));
            else
                return new ClientResult<Entry>((int)response.StatusCode,
                    JsonSerializer.Deserialize<ClientError>(content));
        }

        public async Task<ClientResult> UpdateEntryAsync(string lemma, EntryUpdateInfo updateInfo)
        {
            var stringContent = new StringContent(JsonSerializer
                .Serialize(updateInfo, typeof(EntryUpdateInfo)));

            var response = await httpClient.PatchAsync($"entries/{lemma}", stringContent);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new ClientResult((int)response.StatusCode);
            else
                return new ClientResult((int)response.StatusCode,
                    JsonSerializer.Deserialize<ClientError>(content));
        }

        public async Task<ClientResult> DeleteEntryAsync(string lemma)
        {
            var response = await httpClient.DeleteAsync($"entries/{lemma}");

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new ClientResult((int)response.StatusCode);
            else
                return new ClientResult((int)response.StatusCode,
                    JsonSerializer.Deserialize<ClientError>(content));
        }

        public async Task<ClientResult<Entry>> GetEntryByMeaningAsync(string meaningId)
        {
            var response = await this.httpClient.GetAsync($"entriesByMeaning/{meaningId}");

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new ClientResult<Entry>((int)response.StatusCode,
                    JsonSerializer.Deserialize<Entry>(content));
            else
                return new ClientResult<Entry>((int)response.StatusCode,
                    JsonSerializer.Deserialize<ClientError>(content));
        }

        public async Task<ClientResult<Meaning>> CreateMeaningAsync(string lemma, MeaningCreateInfo createInfo)
        {
            var stringContent = new StringContent(JsonSerializer
                .Serialize(createInfo, typeof(MeaningCreateInfo)));

            var response = await httpClient.PostAsync($"entries/{lemma}/meanings/", stringContent);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new ClientResult<Meaning>((int)response.StatusCode,
                    JsonSerializer.Deserialize<Meaning>(content));
            else
                return new ClientResult<Meaning>((int)response.StatusCode,
                    JsonSerializer.Deserialize<ClientError>(content));
        }

        public async Task<ClientResult> UpdateMeaningAsync(
            string lemma,
            string meaningId,
            MeaningUpdateInfo updateInfo)
        {
            var stringContent = new StringContent(JsonSerializer
                .Serialize(updateInfo, typeof(MeaningUpdateInfo)));

            var response = await httpClient
                .PatchAsync($"entries/{lemma}/meanings/{meaningId}", stringContent);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new ClientResult((int)response.StatusCode);
            else
                return new ClientResult((int)response.StatusCode,
                    JsonSerializer.Deserialize<ClientError>(content));
        }

        public async Task<ClientResult> DeleteMeaningAsync(string lemma, string meaningId)
        {
            var response = await httpClient.DeleteAsync($"entries/{lemma}/meanings/{meaningId}");

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new ClientResult((int)response.StatusCode);
            else
                return new ClientResult((int)response.StatusCode,
                    JsonSerializer.Deserialize<ClientError>(content));
        }
    }
}
