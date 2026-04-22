using System.Runtime.Serialization;

namespace SallaStoreIntegration.Dtos.Product
{
    public enum ImportProductEnum
    {
        [EnumMember(Value = "products")]
        Products,
        [EnumMember(Value = "quantities")]
        Quantities,
        [EnumMember(Value = "seo")]
        Seo,
        [EnumMember(Value = "prices")]
        Prices,
        [EnumMember(Value = "hs-codes")]
        HsCodes
    }
}
