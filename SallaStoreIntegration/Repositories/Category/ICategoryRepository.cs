using SallaStoreIntegration.Dtos;
using SallaStoreIntegration.Dtos.Category;

namespace SallaStoreIntegration.Repositories.Category
{
    public interface ICategoryRepository
    {
        Task<List<CategoryResultDto>> GetCategoriesAsync(string token);
        Task<List<CategoryResultDto>> GetAllCategoriesAsync(string token);
        Task<CategoryResultDto> GetCategoryAsync(string token, long categoryId);
        Task<CategoryResultDto> CreateCategoryAsync(string token, CreateCategoryRequestDto request);
        Task<CategoryResultDto> UpdateCategoryAsync(string token, long categoryId, CreateCategoryRequestDto request);
        Task<bool> DeleteCategoryAsync(string token, long categoryId);
        Task<bool> DeleteCategoriesAsync(string token, List<long> categoryIds);
        Task<List<CategoryResultDto>> GetCategoryChildrenAsync(string token, long categoryId);
        Task<List<CategoryProductDto>> GetCategoryProductsAsync(string token, long categoryId);
        Task<List<CategoryResultDto>> SearchCategoriesAsync(string token, string keyword = null, List<long> ids = null);
        Task<CategoryNodeDto> CreateCategoryNodeAsync(string token, CategoryNodeDto category, CategoryNodeDto parent = null);
        Task<List<CategoryNodeDto>> UploadCategoriesAsync(string token, List<CategoryNodeDto> categories, CategoryNodeDto parent = null);
    }
}
