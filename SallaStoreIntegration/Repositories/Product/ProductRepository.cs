using FluentValidation;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SallaStoreIntegration.Dtos;
using SallaStoreIntegration.Dtos.Category;
using SallaStoreIntegration.Dtos.Product;
using SallaStoreIntegration.Dtos.Product.Image;
using SallaStoreIntegration.Dtos.Response;
using SallaStoreIntegration.Enums.Products;
using SallaStoreIntegration.Repositories.Base;
using SallaStoreIntegration.Setting;
using SallaStoreIntegration.Validation;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

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

        public async  Task<SallaBaseResponse<ProductResponseDto>> UpdateProductBySkuAsync(UpdateProductDto inputDto, string sku, string token)
        {
            var url = $"products/sku/{sku}";
            return await PutAsync<SallaBaseResponse<ProductResponseDto>, UpdateProductDto>(url, inputDto, token);
        }

        public async Task<SallaBaseResponse<ProductResponseDto>> UpdateProductPriceBySkuAsync(UpdateProductPriceBySkuDto price, string sku, string token)
        {
            var url = $"products/sku/{sku}/price";
            return await PostAsync<SallaBaseResponse<ProductResponseDto>, UpdateProductPriceBySkuDto>(url, price, token);
        }

        public async Task<SallaBaseResponse<ChangeStatusResponseDto>> UpdateBulkProductPriceAsync(BulkUpdateProductPriceRequestDto inputDto, string token)
        {
            var url = $"/products/prices/bulkPrice";
            return await PostAsync<SallaBaseResponse<ChangeStatusResponseDto>, BulkUpdateProductPriceRequestDto>(url, inputDto, token);
        }

        public async Task<SallaBaseResponse<ChangeStatusResponseDto>> DeleteProducteAsync(int productId, string token)
        {
            var url = $"products/{productId}";
            return await DeleteAsync<SallaBaseResponse<ChangeStatusResponseDto>>(url, token);
        }

        public async Task<SallaBaseResponse<ChangeStatusResponseDto>> DeleteProducteBySkuAsync(string sku, string token)
        {
            var url = $"/products/sku/{sku}";
            return await DeleteAsync<SallaBaseResponse<ChangeStatusResponseDto>>(url, token);
        }

        public async Task<SallaBaseResponse<ChangeStatusResponseDto>> ImportProducteAsync(Stream file, string FileName, ImportProductEnum type, string token)
        {
            try
            {
                var client = CreateDefaultClient(token);

                using var formData = new MultipartFormDataContent();

                // Add file
                var fileContent = new StreamContent(file);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                formData.Add(fileContent, "file", FileName);

                // Add type enum value
                var typeValue = type.GetType()
                    .GetMember(type.ToString())
                    .First()
                    .GetCustomAttribute<EnumMemberAttribute>()?.Value ?? type.ToString().ToLower();
                formData.Add(new StringContent(typeValue), "type");

                var result = await client.PostAsync($"{BaseUrl}/products/import", formData);
                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<SallaBaseResponse<ChangeStatusResponseDto>>(content);
                }
                return CreateErrorResponse<SallaBaseResponse<ChangeStatusResponseDto>>(content, result.StatusCode);
            }
            catch (Exception e)
            {
                return CreateExceptionResponse<SallaBaseResponse<ChangeStatusResponseDto>>(e.Message);
            }
        }
        public async Task<SallaBaseResponse<AttachImageResponseDto>> AttachImageByProductIdAsync(
            AttachImageByProductIdFormDto inputDto, long productId, string token)
        {
            try
            {
                var client = CreateDefaultClient(token);

                using var formData = new MultipartFormDataContent();

                if (inputDto.Photo != null)
                {
                    using var ms = new MemoryStream();
                    await inputDto.Photo.CopyToAsync(ms);
                    var imageBytes = ms.ToArray();
                    var imageContent = new ByteArrayContent(imageBytes);
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    var fileName = Path.GetFileNameWithoutExtension(inputDto.Photo.FileName) + ".jpg";
                    formData.Add(imageContent, "photo", fileName);
                }

                if (!string.IsNullOrEmpty(inputDto.Original))
                    formData.Add(new StringContent(inputDto.Original), "original");

                if (inputDto.Main.HasValue)
                    formData.Add(new StringContent(inputDto.Main.Value.ToString()), "main");

                if (inputDto.Sort.HasValue)
                    formData.Add(new StringContent(inputDto.Sort.Value.ToString()), "sort");

                if (!string.IsNullOrEmpty(inputDto.Alt))
                    formData.Add(new StringContent(inputDto.Alt), "alt");

                var result = await client.PostAsync($"{BaseUrl}/products/{productId}/images", formData);
                var content = await result.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {result.StatusCode}");
                Console.WriteLine($"Response: {content}");

                return JsonConvert.DeserializeObject<SallaBaseResponse<AttachImageResponseDto>>(content)
                    ?? new SallaBaseResponse<AttachImageResponseDto> { Status = (int)result.StatusCode };
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
                return CreateExceptionResponse<SallaBaseResponse<AttachImageResponseDto>>(e.Message);
            }
        }

        public async Task<SallaBaseResponse<AttachImageResponseDto>> UpdateImageAsync(
            UpdateImageFormDto inputDto, string imageId, string token)
        {
            try
            {
                var client = CreateDefaultClient(token);

                using var formData = new MultipartFormDataContent();

                using var ms = new MemoryStream();
                await inputDto.Photo.CopyToAsync(ms);
                var imageBytes = ms.ToArray();
                var imageContent = new ByteArrayContent(imageBytes);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                var fileName = Path.GetFileNameWithoutExtension(inputDto.Photo.FileName) + ".jpg";
                formData.Add(imageContent, "photo", fileName);

                if (inputDto.Default.HasValue)
                    formData.Add(new StringContent(inputDto.Default.Value.ToString()), "default");

                if (inputDto.Sort.HasValue)
                    formData.Add(new StringContent(inputDto.Sort.Value.ToString()), "sort");

                if (!string.IsNullOrEmpty(inputDto.Alt))
                    formData.Add(new StringContent(inputDto.Alt), "alt");

                var result = await client.PostAsync($"{BaseUrl}/products/images/{imageId}", formData);
                var content = await result.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {result.StatusCode}");
                Console.WriteLine($"Response: {content}");

                return JsonConvert.DeserializeObject<SallaBaseResponse<AttachImageResponseDto>>(content)
                    ?? new SallaBaseResponse<AttachImageResponseDto> { Status = (int)result.StatusCode };
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
                return CreateExceptionResponse<SallaBaseResponse<AttachImageResponseDto>>(e.Message);
            }
        }

        //public async Task<SallaBaseResponse<AttachImageResponseDto>> AttachImageBySkuAsync(
        //   byte[] imageBytes, string fileName, string sku, string token)
        //{
        //    try
        //    {
        //        var client = CreateDefaultClient(token);

        //        using var formData = new MultipartFormDataContent();
        //        var imageContent = new ByteArrayContent(imageBytes);
        //        imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        //        formData.Add(imageContent, "photo", fileName);

        //        Console.WriteLine($"Size: {imageBytes.Length / 1024}KB");

        //        var encodedSku = Uri.EscapeDataString(sku);
        //        var result = await client.PostAsync($"{BaseUrl}/products/sku/{encodedSku}/images", formData);
        //        var content = await result.Content.ReadAsStringAsync();

        //        Console.WriteLine($"Status: {result.StatusCode}");
        //        Console.WriteLine($"Response: {content}");

        //        // Salla error responses share the same JSON shape — deserialize directly so error details are preserved
        //        return JsonConvert.DeserializeObject<SallaBaseResponse<AttachImageResponseDto>>(content)
        //            ?? new SallaBaseResponse<AttachImageResponseDto> { Status = (int)result.StatusCode };
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"Exception: {e}");
        //        return CreateExceptionResponse<SallaBaseResponse<AttachImageResponseDto>>(e.Message);
        //    }
        //}
        public async Task<SallaBaseResponse<AttachImageResponseDto>> AttachImageBySkuAsync(
            AttachImageFormDto form, string sku , string token)
        {
            try
            {
                var client = CreateDefaultClient(token);

                using var formData = new MultipartFormDataContent();

                if (form.Photo != null)
                {
                    using var ms = new MemoryStream();
                    await form.Photo.CopyToAsync(ms);
                    var compressed = CompressImage(ms.ToArray());
                    var imageContent = new ByteArrayContent(compressed);
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    var fileName = Path.GetFileNameWithoutExtension(form.Photo.FileName) + ".jpg";
                    formData.Add(imageContent, "photo", fileName);
                    Console.WriteLine($"Size after compression: {compressed.Length / 1024}KB");
                }

                var encodedSku = Uri.EscapeDataString(sku);
                var result = await client.PostAsync($"{BaseUrl}/products/sku/{encodedSku}/images", formData);
                var content = await result.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {result.StatusCode}");
                Console.WriteLine($"Response: {content}");

                return JsonConvert.DeserializeObject<SallaBaseResponse<AttachImageResponseDto>>(content)
                    ?? new SallaBaseResponse<AttachImageResponseDto> { Status = (int)result.StatusCode };
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
                return CreateExceptionResponse<SallaBaseResponse<AttachImageResponseDto>>(e.Message);
            }
        }

        private static byte[] CompressImage(byte[] imageBytes, int maxSize = 1024, int quality = 80)
        {
            using var input = new MemoryStream(imageBytes);
            using var image = Image.Load(input);

            if (image.Width > maxSize || image.Height > maxSize)
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(maxSize, maxSize),
                    Mode = ResizeMode.Max
                }));

            using var output = new MemoryStream();
            image.Save(output, new JpegEncoder { Quality = quality });
            return output.ToArray();
        }

        public async Task<SallaBaseResponse<ChangeStatusResponseDto>> DeleteImageAsync(string imageId, string token)
        {
            try
            {
                var client = CreateDefaultClient(token);
                var result = await client.DeleteAsync($"{BaseUrl}/products/images/{imageId}");
                var content = await result.Content.ReadAsStringAsync();

                Console.WriteLine($"DeleteImage Status: {result.StatusCode}");
                Console.WriteLine($"DeleteImage Response: {content}");

                return JsonConvert.DeserializeObject<SallaBaseResponse<ChangeStatusResponseDto>>(content)
                    ?? new SallaBaseResponse<ChangeStatusResponseDto> { Status = (int)result.StatusCode };
            }
            catch (Exception e)
            {
                Console.WriteLine($"DeleteImage Exception: {e}");
                return CreateExceptionResponse<SallaBaseResponse<ChangeStatusResponseDto>>(e.Message);
            }
        }

        public async Task<SallaBaseResponse<AttachImageResponseDto>> AttachYoutubeVideoAsync(AttachVideoRequestDto inputDto,int productId, string token)
        {
            var url = $"products/{productId}/video";
            return await PostAsync<SallaBaseResponse<AttachImageResponseDto>, AttachVideoRequestDto>(url, inputDto, token);
        }
    }
}
