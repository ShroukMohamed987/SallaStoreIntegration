using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos.Orders
{
    public class CreateInvoiceResponseDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("order_id")]
        public long OrderId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("invoice_number")]
        public string InvoiceNumber { get; set; }

        [JsonProperty("sub_total")]
        public MoneyDto SubTotal { get; set; }

        [JsonProperty("shipping_cost")]
        public MoneyDto ShippingCost { get; set; }

        [JsonProperty("cod_cost")]
        public MoneyDto CodCost { get; set; }

        [JsonProperty("discount")]
        public MoneyDto Discount { get; set; }

        [JsonProperty("tax")]
        public InvoiceTaxDto Tax { get; set; }

        [JsonProperty("total")]
        public MoneyDto Total { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("items")]
        public List<InvoiceItemDto> Items { get; set; }
    }
    public class MoneyDto
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
