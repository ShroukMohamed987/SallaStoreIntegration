using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos.Product
{
    public class UpdateProductPriceBySkuDto
    {
        [JsonProperty("price")]
        public decimal price { get; set; }
         
    }
}
