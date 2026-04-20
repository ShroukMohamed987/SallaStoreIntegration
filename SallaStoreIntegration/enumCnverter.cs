using SallaStoreIntegration.Enums.Products;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SallaStoreIntegration
{
    public class enumCnverter
    {
    }
    public class ProductStatusEnumConverter : JsonConverter<ProductStatusEnum>
    {
        public override ProductStatusEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return value switch
            {
                "sale" => ProductStatusEnum.sale,
                "out" => ProductStatusEnum.@out,
                "disabled" => ProductStatusEnum.deleted,
                "hidden" => ProductStatusEnum.hidden,
                _ => throw new JsonException($"Unknown status value: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, ProductStatusEnum value, JsonSerializerOptions options)
        {
            var str = value switch
            {
                ProductStatusEnum.sale => "sale",
                ProductStatusEnum.@out => "out",
                ProductStatusEnum.deleted => "deleted",
                ProductStatusEnum.hidden => "hidden",
                _ => throw new JsonException($"Unknown status value: {value}")
            };
            writer.WriteStringValue(str);
        }
    }

    public class ProductTypeEnumConverter : JsonConverter<ProductTypeEnum>
    {
        public override ProductTypeEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return value switch
            {
                "product" => ProductTypeEnum.product,
                "service" => ProductTypeEnum.service,
                "group_products" => ProductTypeEnum.group_products,
                "codes" => ProductTypeEnum.codes,
                "digital" => ProductTypeEnum.digital,
                "food" => ProductTypeEnum.food,
                "booking" => ProductTypeEnum.booking,
                "donating" => ProductTypeEnum.donating,
                _ => throw new JsonException($"Unknown product type: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, ProductTypeEnum value, JsonSerializerOptions options)
        {
            var str = value switch
            {
                ProductTypeEnum.product => "product",
                ProductTypeEnum.service => "service",
                ProductTypeEnum.group_products => "group_products",
                ProductTypeEnum.codes => "codes",
                ProductTypeEnum.digital => "digital",
                ProductTypeEnum.food => "food",
                ProductTypeEnum.booking => "booking",
                ProductTypeEnum.donating => "donating",
                _ => throw new JsonException($"Unknown product type: {value}")
            };
            writer.WriteStringValue(str);
        }
    }
}
