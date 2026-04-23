using System.Text.Json;
using System.Text.Json.Serialization;
using SallaStoreIntegration.Enums;

namespace SallaStoreIntegration.Dtos.Orders
{
    public class OrdersDtos
    {
    }
    // ===== Request DTOs =====

    //public class CreateOrderRequestDto
    //{
    //    [JsonPropertyName("customer")]
    //    public OrderCustomerDto Customer { get; set; }

    //    [JsonPropertyName("receiver")]
    //    public OrderReceiverDto Receiver { get; set; }

    //    [JsonPropertyName("delivery_method")]
    //    public string DeliveryMethod { get; set; }

    //    [JsonPropertyName("branch_id")]
    //    public long? BranchId { get; set; }

    //    [JsonPropertyName("courier_id")]
    //    public string CourierId { get; set; }

    //    [JsonPropertyName("ship_to")]
    //    public OrderShipToDto ShipTo { get; set; }

    //    [JsonPropertyName("payment")]
    //    public OrderPaymentDto Payment { get; set; }

    //    [JsonPropertyName("products")]
    //    public List<OrderProductDto> Products { get; set; }

    //    [JsonPropertyName("coupon_code")]
    //    public string CouponCode { get; set; }
    //}

    public class OrderCustomerDto
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("mobile")]
        public string Mobile { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    public class OrderReceiverDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("notify")]
        public bool Notify { get; set; }
    }

    public class OrderShipToDto
    {
        [JsonPropertyName("country")]
        public long? Country { get; set; }

        [JsonPropertyName("city")]
        public long? City { get; set; }

        [JsonPropertyName("district")]
        public long? District { get; set; }

        [JsonPropertyName("block")]
        public string Block { get; set; }

        [JsonPropertyName("street_number")]
        public string StreetNumber { get; set; }

        [JsonPropertyName("address_line")]
        public string AddressLine { get; set; }

        [JsonPropertyName("postal_code")]
        public string PostalCode { get; set; }

        [JsonPropertyName("short_address")]
        public string ShortAddress { get; set; }

        [JsonPropertyName("building_number")]
        public string BuildingNumber { get; set; }

        [JsonPropertyName("additional_number")]
        public string AdditionalNumber { get; set; }

        [JsonPropertyName("geo_coordinates")]
        public GeoCoordinatesDto GeoCoordinates { get; set; }
    }

    public class GeoCoordinatesDto
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }
    }

    public class OrderPaymentDto
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("store_bank_id")]
        public long? StoreBankId { get; set; }

        [JsonPropertyName("receipt_image_path")]
        public string ReceiptImagePath { get; set; }

        [JsonPropertyName("accepted_methods")]
        public List<string> AcceptedMethods { get; set; }

        [JsonPropertyName("cash_on_delivery")]
        public CashOnDeliveryDto CashOnDelivery { get; set; }

        [JsonPropertyName("recurring")]
        public bool? Recurring { get; set; }
    }

    public class CashOnDeliveryDto
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }

    public class OrderProductDto
    {
        [JsonPropertyName("identifier_type")]
        public string IdentifierType { get; set; }

        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("options")]
        public List<OrderProductOptionDto> Options { get; set; }
    }

    public class OrderProductOptionDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("value")]
        public List<string> Value { get; set; }
    }

    public class UpdateOrderRequestDto
    {
        [JsonPropertyName("customer")]
        public OrderCustomerDto Customer { get; set; }

        [JsonPropertyName("receiver")]
        public OrderReceiverDto Receiver { get; set; }

        [JsonPropertyName("delivery_method")]
        public string DeliveryMethod { get; set; }

        [JsonPropertyName("branch_id")]
        public long? BranchId { get; set; }

        [JsonPropertyName("courier_id")]
        public long? CourierId { get; set; }

        [JsonPropertyName("ship_to")]
        public OrderShipToDto ShipTo { get; set; }

        [JsonPropertyName("payment")]
        public OrderPaymentDto Payment { get; set; }

        [JsonPropertyName("coupon_code")]
        public string CouponCode { get; set; }

        [JsonPropertyName("employees")]
        public List<long> Employees { get; set; }
    }

    public class DuplicateOrderRequestDto
    {
        [JsonPropertyName("order_id")]
        public long OrderId { get; set; }
    }

    public class OrderActionsRequestDto
    {
        public List<OperationDto> Operations { get; set; }
        public FiltersDto Filters { get; set; }
    }

    public class OperationDto
    {
        public string ActionName { get; set; }

        // ممكن يكون object أو array → نخليه dynamic
        public object Value { get; set; }
    }

    public class FiltersDto
    {
        public List<long> OrderIds { get; set; }
        public List<int> OrderStatus { get; set; }
    }
    //public class RelocateOrderStockRequestDto
    //{
    //    [JsonPropertyName("source")]
    //    public long Source { get; set; }

    //    [JsonPropertyName("destination")]
    //    public long Destination { get; set; }

    //    [JsonPropertyName("items")]
    //    public List<RelocateItemDto> Items { get; set; }
    //}

    public class RelocateItemDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }

    public class ListOrdersFilterDto
    {
        public string? Keyword { get; set; }
        public string? PaymentMethod { get; set; }
        public List<string>? Status { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public int? Country { get; set; }
        public string? City { get; set; }
        public string? Product { get; set; }
        public int? Page { get; set; }
        public string? SortBy { get; set; }
    }

    // ===== Result DTOs =====

    public class OrderResultDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("reference_id")]
        public long ReferenceId { get; set; }

        [JsonPropertyName("status")]
        public OrderStatusDto Status { get; set; }

        [JsonPropertyName("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("amounts")]
        public OrderAmountsDto Amounts { get; set; }

        [JsonPropertyName("customer")]
        public OrderCustomerResultDto Customer { get; set; }

        [JsonPropertyName("can_cancel")]
        public bool CanCancel { get; set; }

        [JsonPropertyName("can_reorder")]
        public bool CanReorder { get; set; }

        [JsonPropertyName("is_pending_payment")]
        public bool IsPendingPayment { get; set; }

        public string Message { get; set; }
    }
    public class StatusResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class OrderStatusDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }
    }

    public class OrderAmountsDto
    {
        [JsonPropertyName("sub_total")]
        public AmountDto SubTotal { get; set; }

        [JsonPropertyName("shipping_cost")]
        public AmountDto ShippingCost { get; set; }

        [JsonPropertyName("tax")]
        public OrderTaxDto Tax { get; set; }

        [JsonPropertyName("total")]
        public AmountDto Total { get; set; }
    }

    public class AmountDto
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }

    public class OrderTaxDto
    {
        [JsonPropertyName("percent")]
        public string Percent { get; set; }

        [JsonPropertyName("amount")]
        public AmountDto Amount { get; set; }
    }

    public class OrderCustomerResultDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("mobile")]
        public string Mobile { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    public class DraftOrderResultDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("reference_id")]
        public long ReferenceId { get; set; }

        public string Message { get; set; }
    }

    public class BulkActionResultDto
    {
        [JsonPropertyName("operation_id")]
        public string OperationId { get; set; }

        [JsonPropertyName("action_name")]
        public string ActionName { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
    public class ChangeStatusValueDto
    {
        public long Status { get; set; }
        public bool Send_Status_Sms { get; set; }
        public bool Return_Police { get; set; }
        public bool Restore_Items { get; set; }
        public string Note { get; set; }
        public long Branch_Id { get; set; }
    }
}
