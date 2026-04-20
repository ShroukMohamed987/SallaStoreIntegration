
using System.Text.Json.Serialization;

namespace SallaStoreIntegration.Enums.Products
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProductStatusEnum
    {
        sale = 1,
        @out =2,
        hidden = 3,
        deleted = 4
    }
}
