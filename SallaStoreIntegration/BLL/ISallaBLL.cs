using ExternalStores.DTO.Salla;
using SallaStoreIntegration.Dtos;
using SallaStoreIntegration.Dtos.Brands;
using SallaStoreIntegration.Dtos.Customers;

namespace SallaStoreIntegration.BLL
{
    public interface ISallaBLL
    {
        Task<string> AuthorizeAsync();
        Task<LoginResultDto> CallbackAsync(string code, string state);
        Task<LoginResultDto> RefreshTokenAsync(string refreshToken);

        Task<bool> CreateCategoryAsync(CreateCategoryDto inputDto, string token);
        Task<List<CategoryResultDto>> GetCategoriesAsync(string token);
        Task<bool> UpdateCategoriesAsync(string token , int id , UpdateCategoryInputDto inputDto);


        Task<bool> CreateProductAsync(CreateProductDto inputDto, string token);
        Task<List<ProductResultDto>> GetProductsAsync(FilterProductDto filterDto);
        Task<List<OrderResultDto>> GetOrdersAsync(string token);
        Task<OrderResultDto> GetOrderByIdAsync(string token, int id);
        Task<bool> UpdateOrderStatusAsync(string token, int id, UpdateOrderStatusInputDto inputDto);
        Task<List<StatusResultDto>> GetOrderStatusAsync(string token);


        Task<bool> CreateBrandAsync (CreateBrandDto inputDto, string token);

        Task<bool> CreateCustomerAsync(CreateCustomerDTO inputDto, string token);
        Task<List<CustomerResponseDto>> GetCustomerListAsync(string token);
        Task<CustomerResponseDto> GetCustomerByIdAsync(string token, int id);

        Task<bool> UpdateCustomerAsync(string token, int id, CreateCustomerDTO inputDto);

    }
}
