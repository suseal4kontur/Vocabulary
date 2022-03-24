using System.Text;
using System.Text.Json.Serialization;

namespace Client.ClientResults
{
    public sealed class ClientError
    {
        [JsonPropertyName("errors")]
        public ErrorIds Errors { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("traceId")]
        public string TraceId { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(Title);
            if (Errors != null)
                stringBuilder.Append(Errors);
            return stringBuilder.ToString();
        }
    }

    public sealed class ErrorIds
    {
        [JsonPropertyName("id")]
        public string[] Ids { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var id in Ids)
                stringBuilder.Append($" {id}");
            return stringBuilder.ToString();
        }
    }
}