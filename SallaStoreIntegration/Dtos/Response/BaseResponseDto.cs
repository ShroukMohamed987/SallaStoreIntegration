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
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public T? Data { get; set; }

        [JsonProperty("errors")]
        public object? Errors { get; set; }

        [JsonProperty("pagination")] 
        public PaginationDto? Pagination { get; set; }
    }

    public class PaginationDto
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("perPage")]
        public int PerPage { get; set; }

        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("links")]
        public List<object>? Links { get; set; }
    }
}
