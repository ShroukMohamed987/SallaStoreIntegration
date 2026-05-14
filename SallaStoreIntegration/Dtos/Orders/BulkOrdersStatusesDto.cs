using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos.Orders
{
    public class UpdateBulkOrdersStatusesFormDto
    {
        public IFormFile File { get; set; } = null!;
    }

    public class BulkOrdersStatusesResponseDto
    {
        [JsonProperty("message")]
        public string? Message { get; set; }
    }
}
