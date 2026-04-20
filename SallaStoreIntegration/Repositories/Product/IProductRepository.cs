using SallaStoreIntegration.Dtos;
using SallaStoreIntegration.Dtos.Product;
using SallaStoreIntegration.Dtos.Response;
using SallaStoreIntegration.Enums.Products;

namespace SallaStoreIntegration.Repositories.Product
{
    public interface IProductRepository
    {
        Task<SallaBaseResponse<ProductResponseDto>> CreateProductAsync(CreateProductDto inputDto, string token);
        Task<List<ProductResultDto>> GetProductsAsync(FilterProductDto filterDto);
        Task<ProductResultDto> GetProductAsync(int productId, string token);
        Task<SallaBaseResponse<ProductResponseDto>> UpdateProductAsync(UpdateProductDto inputDto, string token);
        Task<SallaBaseResponse<ProductResponseDto>> GetProductBySKUAsync(string sku, string token);
        Task<SallaBaseResponse<ChangeStatusResponseDto>> ChangeProductStatusAsync(ChangeProductStatusInputDto inputDto, string token);
    }
}
