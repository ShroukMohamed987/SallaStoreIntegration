using System.Text.Json.Serialization;

namespace SallaStoreIntegration.Dtos.Customers
{
    
    public class CreateCustomerDTO
    {
        [JsonPropertyName("first_name")]
        public string first_name { get; set; }

        [JsonPropertyName("last_name")]
        public string last_name { get; set; }

        [JsonPropertyName("mobile")]
        public string mobile { get; set; }

        [JsonPropertyName("mobile_code_country")]
        public string? mobile_code_country { get; set; }

        [JsonPropertyName("email")]
        public string? email { get; set; }

        [JsonPropertyName("groups")]
        public List<string?>? groups { get; set; }
    }
}

