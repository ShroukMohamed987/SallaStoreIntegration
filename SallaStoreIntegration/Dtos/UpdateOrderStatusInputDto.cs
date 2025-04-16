using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos
{
    public class UpdateOrderStatusInputDto
    {
        [JsonProperty("status_id")]
        public int StatusId { get; set; }
    }
}
