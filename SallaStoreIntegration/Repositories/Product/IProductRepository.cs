using SallaStoreIntegration.Dtos;
using SallaStoreIntegration.Dtos.Product;
using SallaStoreIntegration.Dtos.Product.Image;
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
        Task<SallaBaseResponse<ProductResponseDto>> UpdateProductBySkuAsync(UpdateProductDto inputDto, string sku, string token);
        Task<SallaBaseResponse<ProductResponseDto>> UpdateProductPriceBySkuAsync(UpdateProductPriceBySkuDto price, string sku, string token);
        Task<SallaBaseResponse<ChangeStatusResponseDto>> UpdateBulkProductPriceAsync(BulkUpdateProductPriceRequestDto inputDto, string token);
        Task<SallaBaseResponse<ChangeStatusResponseDto>> DeleteProducteAsync(int productId, string token);
        Task<SallaBaseResponse<ChangeStatusResponseDto>> DeleteProducteBySkuAsync(string sku, string token);
        Task<SallaBaseResponse<ChangeStatusResponseDto>> ImportProducteAsync(Stream file, string FileName, ImportProductEnum type, string token);
        Task<SallaBaseResponse<AttachImageResponseDto>> AttachImageBySkuAsync(AttachImageFormDto form, string sku, string token);
        Task<SallaBaseResponse<AttachImageResponseDto>> AttachImageByProductIdAsync(AttachImageByProductIdFormDto inputDto, long productId, string token);
        Task<SallaBaseResponse<AttachImageResponseDto>> UpdateImageAsync(UpdateImageFormDto inputDto, string imageId, string token);
        Task<SallaBaseResponse<ChangeStatusResponseDto>> DeleteImageAsync( string imageId, string token);
        Task<SallaBaseResponse<AttachImageResponseDto>> AttachYoutubeVideoAsync(AttachVideoRequestDto inputDto,int productId, string token);
    }
}
