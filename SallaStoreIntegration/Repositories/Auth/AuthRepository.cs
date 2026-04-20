using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SallaStoreIntegration.Dtos;
using SallaStoreIntegration.Repositories.Base;
using SallaStoreIntegration.Setting;
using System.Net.Http.Headers;

namespace SallaStoreIntegration.Repositories.Auth
{
    public class AuthRepository : BaseRepository, IAuthRepository
    {
        public AuthRepository(IHttpClientFactory httpClientFactory, IOptions<ExternalStoresSetting> settings)
            : base(httpClientFactory, settings)
        {
        }

        public async Task<string> AuthorizeAsync()
        {
            var challengeUrl = $"{_settings.SallaConfigSetting.AuthUri}?" +
                $"{AuthSallaParameterEnum.client_id}={_settings.SallaConfigSetting.ClientId}&" +
                $"{AuthSallaParameterEnum.response_type}={_settings.SallaConfigSetting.ResponseType}&" +
                $"{AuthSallaParameterEnum.redirect_uri}={_settings.SallaConfigSetting.RedirectUri}&" +
                $"{AuthSallaParameterEnum.scope}={_settings.SallaConfigSetting.Scope}&" +
                $"{AuthSallaParameterEnum.state}={new Random().Next(111111111, 999999999)}";

            return challengeUrl;
        }

        public async Task<LoginResultDto> CallbackAsync(string code, string state)
        {
            var response = new LoginResultDto();

            try
            {
                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, _settings.SallaConfigSetting.TokenUri);
                var formData = PrepareFormData(code, state, isCallback: true);
                tokenRequest.Content = new FormUrlEncodedContent(formData);

                var tokenResponse = await _httpClientFactory.CreateClient().SendAsync(tokenRequest);
                tokenResponse.EnsureSuccessStatusCode();

                if (!tokenResponse.IsSuccessStatusCode)
                {
                    response.ErrorMessage = tokenResponse.StatusCode.ToString();
                    return response;
                }

                var tokenResponseBody = await tokenResponse.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<LoginResultDto>(tokenResponseBody);

                return response;
            }
            catch (Exception e)
            {
                response.ErrorMessage += e.ToString();
                return response;
            }
        }

        public async Task<LoginResultDto> RefreshTokenAsync(string refreshToken)
        {
            var response = new LoginResultDto();

            try
            {
                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, _settings.SallaConfigSetting.TokenUri);
                var formData = PrepareFormData(null, null, isCallback: false, refreshToken: refreshToken);
                tokenRequest.Content = new FormUrlEncodedContent(formData);

                var tokenResponse = await _httpClientFactory.CreateClient().SendAsync(tokenRequest);
                tokenResponse.EnsureSuccessStatusCode();

                if (!tokenResponse.IsSuccessStatusCode)
                {
                    response.ErrorMessage = tokenResponse.StatusCode.ToString();
                    return response;
                }

                var tokenResponseBody = await tokenResponse.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<LoginResultDto>(tokenResponseBody);

                return response;
            }
            catch (Exception e)
            {
                response.ErrorMessage += e.ToString();
                return response;
            }
        }

        public async Task<UserInfoDto> GetUserInfoAsync(string token)
        {
            try
            {
                var client = CreateClient(token);
                var result = await client.GetAsync("oauth2/user/info");

                if (!result.IsSuccessStatusCode)
                {
                    return new UserInfoDto { success = false, ErrorMessage = "Invalid Token" };
                }

                var content = await result.Content.ReadAsStringAsync();
                var userInfo = JsonConvert.DeserializeObject<UserInfoDto>(content);

                return userInfo;
            }
            catch (Exception e)
            {
                return new UserInfoDto { success = false, ErrorMessage = e.Message };
            }
        }

        private List<KeyValuePair<string, string>> PrepareFormData(string code, string state, bool isCallback, string refreshToken = null)
        {
            var formData = new List<KeyValuePair<string, string>>
            {
                new(AuthSallaParameterEnum.client_id.ToString(), _settings.SallaConfigSetting.ClientId),
                new(AuthSallaParameterEnum.client_secret.ToString(), _settings.SallaConfigSetting.ClientSecret),
                new(AuthSallaParameterEnum.redirect_uri.ToString(), _settings.SallaConfigSetting.RedirectUri),
            };

            if (isCallback)
            {
                formData.AddRange(new List<KeyValuePair<string, string>>
                {
                    new(AuthSallaParameterEnum.grant_type.ToString(), _settings.SallaConfigSetting.GrantType),
                    new(AuthSallaParameterEnum.code.ToString(), code),
                    new(AuthSallaParameterEnum.state.ToString(), state),
                    new(AuthSallaParameterEnum.scope.ToString(), _settings.SallaConfigSetting.Scope),
                });
            }
            else
            {
                formData.AddRange(new List<KeyValuePair<string, string>>
                {
                    new(AuthSallaParameterEnum.grant_type.ToString(), "refresh_token"),
                    new(AuthSallaParameterEnum.refresh_token.ToString(), refreshToken),
                });
            }

            return formData;
        }
    }
}
