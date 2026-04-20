namespace SallaStoreIntegration.Enums
{
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ActionName
    {
        [JsonPropertyName("create_shipping_policy")]
        CreateShippingPolicy,

        [JsonPropertyName("create_return_policy")]
        CreateReturnPolicy,

        [JsonPropertyName("print_shipping_policy")]
        PrintShippingPolicy,

        [JsonPropertyName("cancel_policy")]
        CancelPolicy,

        [JsonPropertyName("assign_users")]
        AssignUsers,

        [JsonPropertyName("export")]
        Export,

        [JsonPropertyName("assign_tags")]
        AssignTags,

        [JsonPropertyName("change_status")]
        ChangeStatus,

        [JsonPropertyName("print_prepare_list")]
        PrintPrepareList,

        [JsonPropertyName("print_invoice")]
        PrintInvoice,

        [JsonPropertyName("send_invoice")]
        SendInvoice,

        [JsonPropertyName("print_invoice_summary")]
        PrintInvoiceSummary,

        [JsonPropertyName("send_payment_link")]
        SendPaymentLink,

        [JsonPropertyName("extend_payment_due")]
        ExtendPaymentDue,

        [JsonPropertyName("resend_codes")]
        ResendCodes,

        [JsonPropertyName("send_rating")]
        SendRating,

        [JsonPropertyName("mark_as_read")]
        MarkAsRead,

        [JsonPropertyName("refund")]
        Refund,

        [JsonPropertyName("delete")]
        Delete
    }
}
