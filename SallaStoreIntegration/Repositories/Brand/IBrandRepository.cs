using SallaStoreIntegration.Dtos.Brands;
using SallaStoreIntegration.Dtos.Response;
using SallaStoreIntegration.Enums.Products;

namespace SallaStoreIntegration.Repositories.Brand
{
    public interface IBrandRepository
    {
        Task<SallaBaseResponse<BrandResponseDto>> CreateBrandAsync(CreateBrandDto dto, string token);
        Task<SallaBaseResponse<List<BrandResponseDto>>> ListBrandsAsync(ListBrandsFilterDto filter, string token);
        Task<SallaBaseResponse<BrandResponseDto>> GetBrandDetailsAsync(int brandId,string? with, string token);
        Task<SallaBaseResponse<BrandResponseDto>> UpdateBrandAsync(UpdateBrandDto dto,int brandId, string token);
        Task<SallaBaseResponse<ChangeStatusResponseDto>> DeleteBrandAsync(int brandId, string token);
    }
}
