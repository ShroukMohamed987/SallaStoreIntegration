using SallaStoreIntegration.Dtos;
using SallaStoreIntegration.Dtos.Category;

namespace SallaStoreIntegration.Repositories.Category
{
    public interface ICategoryRepository
    {
        Task<List<CategoryResultDto>> GetCategoriesAsync(string token);
        Task<CategoryResultDto> GetCategoryAsync(string token, long categoryId);
        Task<CategoryResultDto> CreateCategoryAsync(string token, CreateCategoryRequestDto request);
        Task<CategoryResultDto> UpdateCategoryAsync(string token, long categoryId, CreateCategoryRequestDto request);
        Task<bool> DeleteCategoryAsync(string token, long categoryId);
        Task<List<CategoryResultDto>> GetCategoryChildrenAsync(string token, long categoryId);
        Task<List<CategoryProductDto>> GetCategoryProductsAsync(string token, long categoryId);
        Task<List<CategoryResultDto>> SearchCategoriesAsync(string token, string keyword = null, List<long> ids = null);
    }
}
