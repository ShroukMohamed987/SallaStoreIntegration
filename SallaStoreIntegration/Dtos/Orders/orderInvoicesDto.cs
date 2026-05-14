using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos.Orders
{
    public class orderInvoicesDto
    {
    }
    public class ListInvoicesQueryDto
    {
        [FromQuery(Name = "from_date")]
        public string? FromDate { get; set; }

        [FromQuery(Name = "to_date")]
        public string? ToDate { get; set; }

        [FromQuery(Name = "order_id")]
        public int? OrderId { get; set; }
    }

    public class InvoiceAmountDto
    {
        [JsonProperty("amount")]
        public string? Amount { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }
    }

    public class InvoiceDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("order_id")]
        public long OrderId { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("invoice_number")]
        public string? InvoiceNumber { get; set; }

        [JsonProperty("sub_total")]
        public InvoiceAmountDto? SubTotal { get; set; }

        [JsonProperty("shipping_cost")]
        public InvoiceAmountDto? ShippingCost { get; set; }

        [JsonProperty("cod_cost")]
        public InvoiceAmountDto? CodCost { get; set; }
    }
}
