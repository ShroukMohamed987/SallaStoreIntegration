using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SallaStoreIntegration.Dtos;
using SallaStoreIntegration.Dtos.Category;
using SallaStoreIntegration.Repositories.Base;
using SallaStoreIntegration.Setting;

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

        public async Task<List<CategoryResultDto>> GetAllCategoriesAsync(string token)
        {
            return await GetAllPagesAsync<CategoryResultDto>("categories", token);
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

        public async Task<bool> DeleteCategoriesAsync(string token, List<long> categoryIds)
        {
            if (categoryIds == null || categoryIds.Count == 0) return true;
            var allOk = true;
            foreach (var id in categoryIds)
            {
                var ok = await BoolDeleteAsync($"categories/{id}", token);
                if (!ok) allOk = false;
            }
            return allOk;
        }

        public async Task<CategoryNodeDto> CreateCategoryNodeAsync(string token, CategoryNodeDto category, CategoryNodeDto parent = null)
        {
            // idempotency: try to find existing category by name (under same parent if known) before creating
            var existing = await SearchCategoriesAsync(token, category.Name);
            var match = existing?.FirstOrDefault(c =>
                string.Equals(c.Name, category.Name, StringComparison.OrdinalIgnoreCase) &&
                (parent?.Id == null || c.ParentId == parent.Id));

            if (match != null && string.IsNullOrEmpty(match.Message))
            {
                category.Id = match.Id;
                category.ParentId = match.ParentId;
                return category;
            }

            var request = new CreateCategoryRequestDto
            {
                Name = category.Name,
                ParentId = parent?.Id,
                showIn = new CategoryShowInDto { App = true }
            };

            var created = await CreateCategoryAsync(token, request);
            if (created == null || created.Id == 0)
            {
                category.Message = created?.Message ?? "Failed to create category";
                return category;
            }

            category.Id = created.Id;
            category.ParentId = created.ParentId;
            return category;
        }

        public async Task<List<CategoryNodeDto>> UploadCategoriesAsync(string token, List<CategoryNodeDto> categories, CategoryNodeDto parent = null)
        {
            if (categories == null) return new List<CategoryNodeDto>();

            foreach (var node in categories)
            {
                var created = await CreateCategoryNodeAsync(token, node, parent);
                if (!string.IsNullOrEmpty(created.Message)) continue; // skip subtree on failure

                if (node.Subcategories != null && node.Subcategories.Count > 0)
                {
                    await UploadCategoriesAsync(token, node.Subcategories, created);
                }
            }

            return categories;
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
