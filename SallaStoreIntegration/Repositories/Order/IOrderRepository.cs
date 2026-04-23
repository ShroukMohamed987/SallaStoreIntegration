//using SallaStoreIntegration.Dtos;
using SallaStoreIntegration.Dtos.Orders;
using SallaStoreIntegration.Dtos.Response;

namespace SallaStoreIntegration.Repositories.Order
{
    public interface IOrderRepository
    {
        Task<List<Dtos.Orders.OrderResultDto>> GetOrdersAsync(string token, ListOrdersFilterDto filter = null);
        Task<Dtos.Orders.OrderResultDto> GetOrderAsync(long orderId, string token);
        Task<bool> UpdateOrderStatusAsync(string token, int id, UpdateOrderStatusInputDto inputDto);
        Task<List<StatusResultDto>> GetOrderStatusAsync(string token);
        Task<string> ExecuteOrderActionsAsync(OrderActionsRequestDto request, string token);
        Task<SallaBaseResponse<CreateOrderResponseDto>> CreateOrderAsync(CreateOrderRequestDto request, string token);
        Task<SallaBaseResponse<RelocateOrderStockResponseDto>> RelocateOrderStockAsync(RelocateOrderStockRequestDto request, long orderId, string token);
    }
}
