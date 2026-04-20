


using System.Text.Json.Serialization;

namespace SallaStoreIntegration.Enums.Products
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProductTypeEnum
    {
        product,
        service,
        group_products,
        codes,
        digital,
        food,
        booking,
        donating
    }
}
