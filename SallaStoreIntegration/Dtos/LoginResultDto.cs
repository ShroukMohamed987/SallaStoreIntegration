using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos
{
    public class LoginResultDto
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = null!;
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; } = null!;

        public string? ErrorMessage { get; set; }
    }
}