using SallaStoreIntegration.Dtos.Brands;

namespace SallaStoreIntegration.Repositories.Brand
{
    public interface IBrandRepository
    {
        Task<bool> CreateBrandAsync(CreateBrandDto inputDto, string token);
    }
}
