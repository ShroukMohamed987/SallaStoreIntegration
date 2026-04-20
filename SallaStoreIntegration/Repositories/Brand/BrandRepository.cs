using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SallaStoreIntegration.Dtos.Brands;
using SallaStoreIntegration.Repositories.Base;
using SallaStoreIntegration.Setting;
using System.Text;

namespace SallaStoreIntegration.Repositories.Brand
{
    public class BrandRepository : BaseRepository, IBrandRepository
    {
        public BrandRepository(IHttpClientFactory httpClientFactory, IOptions<ExternalStoresSetting> settings)
            : base(httpClientFactory, settings)
        {
        }

        public async Task<bool> CreateBrandAsync(CreateBrandDto inputDto, string token)
        {
            try
            {
                var client = CreateClient(token);
                var data = JsonConvert.SerializeObject(inputDto);
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("brands", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
