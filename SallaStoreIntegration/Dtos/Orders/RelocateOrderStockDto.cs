using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos.Orders
{
    public class RelocateOrderStockRequestDto
    {
        [JsonProperty("source")]
        public long Source { get; set; }

        [JsonProperty("destination")]
        public long Destination { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<RelocateOrderStockItemDto>? Items { get; set; }
    }

    public class RelocateOrderStockItemDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }

    public class RelocateOrderStockResponseDto
    {
        [JsonProperty("message")]
        public string? Message { get; set; }
    }
}
