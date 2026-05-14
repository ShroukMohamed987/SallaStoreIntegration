using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos.Orders
{
    public class CreateInvoiceDto
    {
        [JsonProperty("order_id")]
        public long OrderId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("data_type")]
        public string DataType { get; set; }

        [JsonProperty("url")]
        public string? Url { get; set; }

        [JsonProperty("invoice_number")]
        public string? InvoiceNumber { get; set; }

        [JsonProperty("sub_total")]
        public decimal? SubTotal { get; set; }

        [JsonProperty("total")]
        public decimal? Total { get; set; }

        [JsonProperty("shipping_cost")]
        public decimal? ShippingCost { get; set; }

        [JsonProperty("cash_on_delivery_cost")]
        public decimal? CashOnDeliveryCost { get; set; }

        [JsonProperty("tax")]
        public decimal? Tax { get; set; }

        [JsonProperty("tax_value")]
        public decimal? TaxValue { get; set; }
    }
}
