using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos.Orders
{
    public class InvoiceDetailsDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("invoice_number")]
        public string? InvoiceNumber { get; set; }

        [JsonProperty("uuid")]
        public string? Uuid { get; set; }

        [JsonProperty("order_id")]
        public long OrderId { get; set; }

        [JsonProperty("invoice_reference_id")]
        public string? InvoiceReferenceId { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("date")]
        public string? Date { get; set; }

        [JsonProperty("qr_code")]
        public string? QrCode { get; set; }

        [JsonProperty("payment_method")]
        public string? PaymentMethod { get; set; }

        [JsonProperty("sub_total")]
        public InvoiceAmountDto? SubTotal { get; set; }

        [JsonProperty("shipping_cost")]
        public ShippingCostDto? ShippingCost { get; set; }

        [JsonProperty("cod_cost")]
        public CodCostDto? CodCost { get; set; }

        [JsonProperty("discount")]
        public InvoiceAmountDto? Discount { get; set; }

        [JsonProperty("tax")]
        public InvoiceTaxDto? Tax { get; set; }

        [JsonProperty("total")]
        public InvoiceAmountDto? Total { get; set; }

        [JsonProperty("items")]
        public List<InvoiceItemDto>? Items { get; set; }
    }

    public class ShippingCostDto
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("taxable")]
        public bool Taxable { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }
    }

    public class CodCostDto
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("taxable")]
        public bool Taxable { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }
    }

    public class InvoiceTaxDto
    {
        [JsonProperty("percent")]
        public decimal Percent { get; set; }

        [JsonProperty("amount")]
        public InvoiceAmountDto? Amount { get; set; }
    }

    public class InvoiceItemDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("product_id")]
        public string? ProductId { get; set; }

        [JsonProperty("item_id")]
        public long ItemId { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("sku")]
        public string? Sku { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("price")]
        public InvoiceAmountDto? Price { get; set; }

        [JsonProperty("discount")]
        public InvoiceAmountDto? Discount { get; set; }

        [JsonProperty("tax")]
        public InvoiceTaxDto? Tax { get; set; }

        [JsonProperty("total")]
        public InvoiceAmountDto? Total { get; set; }
    }
}
