namespace SallaStoreIntegration.Setting
{
    public class SallaConfigSetting
    {
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        public string GrantType { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Scope { get; set; } = null!;
        public string ResponseType { get; set; } = null!;
        public string RedirectUri { get; set; } = null!;
        public string TokenUri { get; set; } = null!;
        public string AuthUri { get; set; } = null!;

    }
}
