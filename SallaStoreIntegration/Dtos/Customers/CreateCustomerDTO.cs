using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos.Customers
{
    public class CreateCustomerDTO
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("mobile_code_country")]
        public string? MobileCodeCountry { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }
        [JsonProperty("groups")]
        public List<string?>? Groups { get; set; }
    }
}

