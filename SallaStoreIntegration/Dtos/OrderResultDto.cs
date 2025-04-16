using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos
{
    public class OrderResultDto
    {
        public string? ErrorMessage { get; set; }
        public int Id { get; set; }
        [JsonProperty("can_cancel")]
        public bool CanCancel { get; set; }
        [JsonProperty("total")]
        public TotalResultDto Total { get; set; } = new();
        [JsonProperty("status")]
        public StatusResultDto Status { get; set; } = new();
        [JsonProperty("items")]
        public List<ItemResultDto> Items { get; set; } = new();
        [JsonProperty("customer")]
        public CutomerResulDto Cutomer { get; set; } = new();
    }
    public class TotalResultDto
    {
        [JsonProperty("amount")]
        public decimal TotalPrice { get; set; }
        public string Currency { get; set; }
    }
    public class StatusResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class ItemResultDto
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
    }
    public class CutomerResulDto
    {
        public int Id { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        public string Name => FirstName + LastName;
        public string Email { get; set; }
    }
}
