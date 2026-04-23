using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace SallaStoreIntegration.Enums.Orders
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DeliveryMethodEnum
    {
        [EnumMember(Value = "shipping")]
        Shipping,

        [EnumMember(Value = "pickup")]
        Pickup
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentStatusEnum
    {
        [EnumMember(Value = "paid")]
        Paid,

        [EnumMember(Value = "pending_payment")]
        PendingPayment
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentMethodEnum
    {
        [EnumMember(Value = "credit_card")]
        CreditCard,

        [EnumMember(Value = "mada")]
        Mada,

        [EnumMember(Value = "bank")]
        Bank,

        [EnumMember(Value = "cod")]
        Cod,

        [EnumMember(Value = "subscription")]
        Subscription
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentAcceptedMethodEnum
    {
        [EnumMember(Value = "credit_card")]
        CreditCard,

        [EnumMember(Value = "mada")]
        Mada,

        [EnumMember(Value = "bank")]
        Bank,

        [EnumMember(Value = "cod")]
        Cod
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductIdentifierTypeEnum
    {
        [EnumMember(Value = "id")]
        Id,

        [EnumMember(Value = "sku")]
        Sku
    }
}
