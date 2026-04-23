using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SallaStoreIntegration.Dtos.Response
{
    public class BaseResponseDto
    {
    }

    public class SallaErrorDto
    {
        [JsonPropertyName("code")]
        [JsonProperty("code")]
        public object? Code { get; set; }

        [JsonPropertyName("message")]
        [JsonProperty("message")]
        public object? Message { get; set; }

        [JsonPropertyName("fields")]
        [JsonProperty("fields")]
        public Dictionary<string, List<string>>? Fields { get; set; }
    }
    public class SallaBaseResponse<T>
    {
        [JsonPropertyName("status")]
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonPropertyName("success")]
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        [JsonProperty("data")]
        public T? Data { get; set; }

        //[JsonPropertyName("errors")]
        //[JsonProperty("errors")]
        //public object? Errors { get; set; }

        [JsonPropertyName("error")]
        [JsonProperty("error")]
        public SallaErrorDto? Error { get; set; }
    }
}
