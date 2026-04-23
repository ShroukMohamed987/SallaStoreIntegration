using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SallaStoreIntegration.Enums.Orders;

namespace SallaStoreIntegration.Dtos.Orders
{
    public class CreateOrderRequestDto
    {
        [JsonProperty("customer", NullValueHandling = NullValueHandling.Ignore)]
        public CreateOrderCustomerDto? Customer { get; set; }

        [JsonProperty("receiver", NullValueHandling = NullValueHandling.Ignore)]
        public CreateOrderReceiverDto? Receiver { get; set; }

        [JsonProperty("delivery_method", NullValueHandling = NullValueHandling.Ignore)]
        public DeliveryMethodEnum? DeliveryMethod { get; set; }

        [JsonProperty("branch_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? BranchId { get; set; }

        [JsonProperty("courier_id", NullValueHandling = NullValueHandling.Ignore)]
        public string? CourierId { get; set; }

        [JsonProperty("ship_to", NullValueHandling = NullValueHandling.Ignore)]
        public CreateOrderShipToDto? ShipTo { get; set; }

        [JsonProperty("payment")]
        public CreateOrderPaymentDto Payment { get; set; } = new();

        [JsonProperty("products")]
        public List<CreateOrderProductDto> Products { get; set; } = new();

        [JsonProperty("coupon_code", NullValueHandling = NullValueHandling.Ignore)]
        public string? CouponCode { get; set; }

        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        public string? Source { get; set; }

        [JsonProperty("source_device", NullValueHandling = NullValueHandling.Ignore)]
        public string? SourceDevice { get; set; }

        [JsonProperty("meta", NullValueHandling = NullValueHandling.Ignore)]
        public CreateOrderMetaDto? Meta { get; set; }
    }

    public class CreateOrderCustomerDto
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("mobile", NullValueHandling = NullValueHandling.Ignore)]
        public string? Mobile { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string? Name { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string? Email { get; set; }
    }

    public class CreateOrderReceiverDto
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("country_code")]
        public string? CountryCode { get; set; }

        [JsonProperty("phone")]
        public string? Phone { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("notify")]
        public bool? Notify { get; set; }
    }

    public class CreateOrderShipToDto
    {
        [JsonProperty("country")]
        public long Country { get; set; }

        [JsonProperty("city")]
        public long City { get; set; }

        [JsonProperty("district", NullValueHandling = NullValueHandling.Ignore)]
        public long? District { get; set; }

        [JsonProperty("block", NullValueHandling = NullValueHandling.Ignore)]
        public string? Block { get; set; }

        [JsonProperty("street_number", NullValueHandling = NullValueHandling.Ignore)]
        public string? StreetNumber { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string? Address { get; set; }

        [JsonProperty("address_line", NullValueHandling = NullValueHandling.Ignore)]
        public string? AddressLine { get; set; }

        [JsonProperty("postal_code", NullValueHandling = NullValueHandling.Ignore)]
        public string? PostalCode { get; set; }

        [JsonProperty("short_address", NullValueHandling = NullValueHandling.Ignore)]
        public string? ShortAddress { get; set; }

        [JsonProperty("building_number", NullValueHandling = NullValueHandling.Ignore)]
        public string? BuildingNumber { get; set; }

        [JsonProperty("additional_number", NullValueHandling = NullValueHandling.Ignore)]
        public long? AdditionalNumber { get; set; }

        [JsonProperty("geo_coordinates", NullValueHandling = NullValueHandling.Ignore)]
        public CreateOrderGeoCoordinatesDto? GeoCoordinates { get; set; }
    }

    public class CreateOrderGeoCoordinatesDto
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }

    public class CreateOrderPaymentDto
    {
        [JsonProperty("status")]
        public PaymentStatusEnum Status { get; set; } = PaymentStatusEnum.Paid;

        [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentMethodEnum? Method { get; set; }

        [JsonProperty("store_bank_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? StoreBankId { get; set; }

        [JsonProperty("receipt_image_path", NullValueHandling = NullValueHandling.Ignore)]
        public string? ReceiptImagePath { get; set; }

        [JsonProperty("accepted_methods", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaymentAcceptedMethodEnum>? AcceptedMethods { get; set; }

        [JsonProperty("reference_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? ReferenceId { get; set; }

        [JsonProperty("cash_on_delivery", NullValueHandling = NullValueHandling.Ignore)]
        public CreateOrderCashOnDeliveryDto? CashOnDelivery { get; set; }

        [JsonProperty("recurring", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Recurring { get; set; }
    }

    public class CreateOrderCashOnDeliveryDto
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; } = "SAR";
    }

    public class CreateOrderProductDto
    {
        [JsonProperty("identifier_type")]
        public ProductIdentifierTypeEnum IdentifierType { get; set; } = ProductIdentifierTypeEnum.Id;

        [JsonProperty("identifier")]
        public object Identifier { get; set; } = null!;

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("options", NullValueHandling = NullValueHandling.Ignore)]
        public List<CreateOrderProductOptionDto>? Options { get; set; }

        [JsonProperty("donating_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? DonatingAmount { get; set; }
    }

    public class CreateOrderProductOptionDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("value")]
        public object? Value { get; set; }
    }

    public class CreateOrderMetaDto
    {
        [JsonProperty("note", NullValueHandling = NullValueHandling.Ignore)]
        public string? Note { get; set; }
    }

    public class CreateOrderResponseDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("reference_id")]
        public long? ReferenceId { get; set; }

        [JsonProperty("urls")]
        public object? Urls { get; set; }

        [JsonProperty("date")]
        public object? Date { get; set; }

        [JsonProperty("source")]
        public string? Source { get; set; }

        [JsonProperty("status")]
        public object? Status { get; set; }

        [JsonProperty("payment_method")]
        public string? PaymentMethod { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }

        [JsonProperty("amounts")]
        public object? Amounts { get; set; }

        [JsonProperty("customer")]
        public object? Customer { get; set; }

        [JsonProperty("items")]
        public object? Items { get; set; }
    }
}
