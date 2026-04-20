using FluentValidation;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SallaStoreIntegration.Dtos;
using SallaStoreIntegration.Dtos.Category;
using SallaStoreIntegration.Dtos.Product;
using SallaStoreIntegration.Dtos.Response;
using SallaStoreIntegration.Enums.Products;
using SallaStoreIntegration.Repositories.Base;
using SallaStoreIntegration.Setting;
using SallaStoreIntegration.Validation;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SallaStoreIntegration.Repositories.Product
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(IHttpClientFactory httpClientFactory, IOptions<ExternalStoresSetting> settings)
            : base(httpClientFactory, settings)
        {
        }

        public async Task<SallaBaseResponse<ProductResponseDto>> CreateProductAsync(CreateProductDto inputDto, string token)
        {
            // Validate
            var validationResult = await new CreateProductDtoValidator().ValidateAsync(inputDto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

            // Serialize
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(inputDto, options),
                new UTF8Encoding(false),
                "application/json"
            );

            // Send
            var client = CreateClient(token);
            var result = await client.PostAsync($"{BaseUrl}/products", content);
            var responseBody = await result.Content.ReadAsStringAsync();

            if (!result.IsSuccessStatusCode)
                throw new HttpRequestException($"API error {result.StatusCode}: {responseBody}");

            // Deserialize
            var response = System.Text.Json.JsonSerializer.Deserialize<SallaBaseResponse<ProductResponseDto>>(responseBody, options);

            if (response is null || !response.Success)
                throw new HttpRequestException($"Salla returned failure: {responseBody}");

            return response;
        }



        public async Task<List<ProductResultDto>> GetProductsAsync(FilterProductDto filterDto)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("SallaApi");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", filterDto.Token);

                var queryParameters = new List<string>
                {
                    $"{nameof(filterDto.category)}={filterDto?.category}",
                    $"{nameof(filterDto.page)}={filterDto?.page}",
                    $"{nameof(filterDto.keyword)}={filterDto?.keyword}",
                    $"{nameof(filterDto.status)}={filterDto?.status}",
                    $"{nameof(filterDto.per_page)}={filterDto?.per_page}",
                };

                var requestUrl = $"products?{string.Join("&", queryParameters)}";
                HttpResponseMessage result = await client.GetAsync($"{BaseUrl}/{requestUrl}");

                if (result.IsSuccessStatusCode)
                {
                    string content = await result.Content.ReadAsStringAsync();
                    var resultJson = JObject.Parse(content);
                    return JsonConvert.DeserializeObject<List<ProductResultDto>>(resultJson["data"].ToString());
                }

                return new List<ProductResultDto> { new ProductResultDto { Message = "Invalid Token" } };
            }
            catch (Exception e)
            {
                return new List<ProductResultDto> { new ProductResultDto { Message = e.Message } };
            }
        }

        public async Task<ProductResultDto> GetProductAsync(int productId, string token)
        {
            return await GetAsync<ProductResultDto>($"products/{productId}", token);
        }

        public async Task<SallaBaseResponse<ProductResponseDto>> UpdateProductAsync(UpdateProductDto inputDto, string token)
        {
            var url= $"products/{inputDto.Id}";
            return await PutAsync<SallaBaseResponse<ProductResponseDto>, UpdateProductDto>(url, inputDto, token);
        }

        public async Task<SallaBaseResponse<ProductResponseDto>> GetProductBySKUAsync(string sku, string token)
        {
            var url = $"products/sku/{sku}";
            return await GetAsync<SallaBaseResponse<ProductResponseDto>>(url, token);
        }

        public async Task<SallaBaseResponse<ChangeStatusResponseDto>> ChangeProductStatusAsync(ChangeProductStatusInputDto inputDto, string token)
        {
            var url = $"products/{inputDto.ProductId}/status";
            return await PostAsync<SallaBaseResponse<ChangeStatusResponseDto>,ChangeProductStatusInputDto>(url,inputDto, token);
        }
    }
}
