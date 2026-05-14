using SallaStoreIntegration.Dtos.Customers;

namespace SallaStoreIntegration.Repositories.Customer
{
    public interface ICustomerRepository
    {
        Task<int?> CreateCustomerAsync(CreateCustomerDTO inputDto, string token);
        Task<List<CustomerResponseDto>> GetCustomerListAsync(string token);
        Task<CustomerResponseDto> GetCustomerByIdAsync(string token, int id);
        Task<bool> UpdateCustomerAsync(string token, int id, CreateCustomerDTO inputDto);
    }
}
