using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SallaStoreIntegration.Dtos;
using SallaStoreIntegration.Dtos.Category;
using SallaStoreIntegration.Repositories.Base;
using SallaStoreIntegration.Setting;
using System.Text;

namespace SallaStoreIntegration.Repositories.Category
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(IHttpClientFactory httpClientFactory, IOptions<ExternalStoresSetting> settings)
            : base(httpClientFactory, settings)
        {
        }

        public async Task<List<CategoryResultDto>> GetCategoriesAsync(string token)
        {
            return await GetListAsync<CategoryResultDto>("categories", token);
        }

        public async Task<CategoryResultDto> GetCategoryAsync(string token, long categoryId)
        {
            return await GetAsync<CategoryResultDto>($"categories/{categoryId}", token);
        }

        public async Task<CategoryResultDto> CreateCategoryAsync(string token, CreateCategoryRequestDto request)
        {
            return await PostAsync<CategoryResultDto, CreateCategoryRequestDto>("categories", request, token);
        }

        public async Task<CategoryResultDto> UpdateCategoryAsync(string token, long categoryId, CreateCategoryRequestDto request)
        {
            return await PutAsync<CategoryResultDto, CreateCategoryRequestDto>($"categories/{categoryId}", request, token);
        }

        public async Task<bool> DeleteCategoryAsync(string token, long categoryId)
        {
            return await BoolDeleteAsync($"categories/{categoryId}", token);
        }

        public async Task<List<CategoryResultDto>> GetCategoryChildrenAsync(string token, long categoryId)
        {
            return await GetListAsync<CategoryResultDto>($"categories/{categoryId}/children", token);
        }

        public async Task<List<CategoryProductDto>> GetCategoryProductsAsync(string token, long categoryId)
        {
            return await GetListAsync<CategoryProductDto>($"categories/{categoryId}/products", token);
        }

        public async Task<List<CategoryResultDto>> SearchCategoriesAsync(string token, string keyword = null, List<long> ids = null)
        {
            try
            {
                var client = CreateDefaultClient(token);

                var query = "";
                if (!string.IsNullOrEmpty(keyword))
                    query += $"?keyword={keyword}";
                if (ids != null && ids.Any())
                {
                    var idsQuery = string.Join("&", ids.Select(id => $"ids[]={id}"));
                    query += (query.Contains("?") ? "&" : "?") + idsQuery;
                }

                var result = await client.GetAsync($"{BaseUrl}/categories/search{query}");
                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    var resultJson = JObject.Parse(content);
                    return JsonConvert.DeserializeObject<List<CategoryResultDto>>(resultJson["data"].ToString());
                }

                var error = JObject.Parse(content);
                return new List<CategoryResultDto>
                {
                    new CategoryResultDto
                    {
                        Message = error["error"]?["message"]?.ToString() ?? result.StatusCode.ToString()
                    }
                };
            }
            catch (Exception e)
            {
                return new List<CategoryResultDto> { new CategoryResultDto { Message = e.Message } };
            }
        }
    }
}
