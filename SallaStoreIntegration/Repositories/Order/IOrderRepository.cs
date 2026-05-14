//using SallaStoreIntegration.Dtos;
using SallaStoreIntegration.Dtos.OrderInvoice;
using SallaStoreIntegration.Dtos.Orders;
using SallaStoreIntegration.Dtos.Response;

namespace SallaStoreIntegration.Repositories.Order
{
    public interface IOrderRepository
    {
        Task<List<Dtos.Orders.OrderResultDto>> GetOrdersAsync(string token, ListOrdersFilterDto filter = null);
        Task<List<Dtos.Orders.OrderResultDto>> GetAllOrdersByStatusAsync(string token, List<int> statusIds, CancellationToken ct = default);
        Task<Newtonsoft.Json.Linq.JObject> GetOrderDetailsRawAsync(long orderId, string token, CancellationToken ct = default);
        Task<PendingOrdersBatchDto> PullPendingOrdersAsync(string token, List<int> statusIds, bool includeDetails = true, CancellationToken ct = default);
        Task<Dtos.Orders.OrderResultDto> GetOrderAsync(long orderId, string token);
        Task<bool> UpdateOrderStatusAsync(string token, int id, UpdateOrderStatusInputDto inputDto);
        Task<List<StatusResultDto>> GetOrderStatusAsync(string token);
        Task<string> ExecuteOrderActionsAsync(OrderActionsRequestDto request, string token);
        Task<SallaBaseResponse<CreateOrderResponseDto>> CreateOrderAsync(CreateOrderRequestDto request, string token);
        Task<SallaBaseResponse<RelocateOrderStockResponseDto>> RelocateOrderStockAsync(RelocateOrderStockRequestDto request, long orderId, string token);
        Task<SallaBaseResponse<BulkOrdersStatusesResponseDto>> UpdateBulkOrdersStatusesAsync(UpdateBulkOrdersStatusesFormDto form, string token);
        Task<SallaBaseResponse<OrderInvoiceResponseDto>> CreateOrderInvoiceAsync(long orderId, string token);
        Task<SallaBaseResponse<List<InvoiceDto>>> ListInvoicesAsync(string? fromDate, string? toDate, int? orderId, string token);
        Task<SallaBaseResponse<InvoiceDetailsDto>> GetInvoiceDetailsAsync(long invoiceId, string token);
        Task<SallaBaseResponse<CreateInvoiceResponseDto>> CreateInvoiceAsync(CreateInvoiceDto dto, string token);
    }
}
