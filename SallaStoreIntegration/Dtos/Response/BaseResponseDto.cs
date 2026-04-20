using System.Text.Json.Serialization;

namespace SallaStoreIntegration.Dtos.Response
{
    public class BaseResponseDto
    {
    }
    public class SallaBaseResponse<T>
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("errors")]
        public object? Errors { get; set; }
    }
}
