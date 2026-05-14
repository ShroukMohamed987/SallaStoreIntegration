using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SallaStoreIntegration.Dtos.OrderInvoice
{
    public class OrderInvoiceResponseDto
    {
        [JsonPropertyName("url")]
        [JsonProperty("url")]
        public string? Url { get; set; }

        public string? Message { get; set; }
    }
}
