using SallaStoreIntegration.Dtos;

namespace SallaStoreIntegration.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<string> AuthorizeAsync();
        Task<LoginResultDto> CallbackAsync(string code, string state);
        Task<LoginResultDto> RefreshTokenAsync(string refreshToken);
        Task<UserInfoDto> GetUserInfoAsync(string token);
    }
}
