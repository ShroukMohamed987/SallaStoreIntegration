using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos.Orders
{
    public class UpdateOrderStatusInputDto
    {
        [JsonProperty("status_id")]
        public int StatusId { get; set; }
    }
}
