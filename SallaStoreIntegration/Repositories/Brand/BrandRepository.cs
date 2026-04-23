using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SallaStoreIntegration.Dtos.Brands;
using SallaStoreIntegration.Dtos.Response;
using SallaStoreIntegration.Enums.Products;
using SallaStoreIntegration.Repositories.Base;
using SallaStoreIntegration.Setting;
using System.Net.Http.Headers;

namespace SallaStoreIntegration.Repositories.Brand
{
    public class BrandRepository : BaseRepository, IBrandRepository
    {
        public BrandRepository(IHttpClientFactory httpClientFactory, IOptions<ExternalStoresSetting> settings)
            : base(httpClientFactory, settings)
        {
        }

        public async Task<SallaBaseResponse<List<BrandResponseDto>>> ListBrandsAsync(ListBrandsFilterDto filter, string token)
        {
            try
            {
                var client = CreateDefaultClient(token);

                var queryParams = new List<string>();
                if (!string.IsNullOrEmpty(filter.Keyword))
                    queryParams.Add($"keyword={Uri.EscapeDataString(filter.Keyword)}");
                if (filter.Page.HasValue)
                    queryParams.Add($"page={filter.Page.Value}");
                if (!string.IsNullOrEmpty(filter.With))
                    queryParams.Add($"with={Uri.EscapeDataString(filter.With)}");

                var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
                var result = await client.GetAsync($"{BaseUrl}/brands{query}");
                var content = await result.Content.ReadAsStringAsync();

                Console.WriteLine($"ListBrands Status: {result.StatusCode}");
                Console.WriteLine($"ListBrands Response: {content}");

                return JsonConvert.DeserializeObject<SallaBaseResponse<List<BrandResponseDto>>>(content)
                    ?? new SallaBaseResponse<List<BrandResponseDto>> { Status = (int)result.StatusCode };
            }
            catch (Exception e)
            {
                Console.WriteLine($"ListBrands Exception: {e}");
                return CreateExceptionResponse<SallaBaseResponse<List<BrandResponseDto>>>(e.Message);
            }
        }

        public async Task<SallaBaseResponse<BrandResponseDto>> CreateBrandAsync(CreateBrandDto dto, string token)
        {
            try
            {
                var client = CreateDefaultClient(token);

                using var form = new MultipartFormDataContent();

                form.Add(new StringContent(dto.Name), "name");

                if (!string.IsNullOrEmpty(dto.Description))
                    form.Add(new StringContent(dto.Description), "description");

                if (!string.IsNullOrEmpty(dto.MetadataTitle))
                    form.Add(new StringContent(dto.MetadataTitle), "metadata[title]");

                if (!string.IsNullOrEmpty(dto.MetadataDescription))
                    form.Add(new StringContent(dto.MetadataDescription), "metadata[description]");

                if (!string.IsNullOrEmpty(dto.MetadataUrl))
                    form.Add(new StringContent(dto.MetadataUrl), "metadata[url]");

                var logoStream = dto.Logo.OpenReadStream();
                var logoContent = new StreamContent(logoStream);
                logoContent.Headers.ContentType = new MediaTypeHeaderValue(dto.Logo.ContentType);
                form.Add(logoContent, "logo", dto.Logo.FileName);

                if (dto.Banner != null)
                {
                    var bannerStream = dto.Banner.OpenReadStream();
                    var bannerContent = new StreamContent(bannerStream);
                    bannerContent.Headers.ContentType = new MediaTypeHeaderValue(dto.Banner.ContentType);
                    form.Add(bannerContent, "banner", dto.Banner.FileName);
                }

                if (!string.IsNullOrEmpty(dto.TranslationsJson))
                {
                    var translations = JsonConvert.DeserializeObject<List<TranslationDto>>(dto.TranslationsJson);
                    if (translations != null)
                    {
                        foreach (var t in translations)
                        {
                            var lang = t.Locale;

                            if (!string.IsNullOrEmpty(t.Name))
                                form.Add(new StringContent(t.Name), $"translations[{lang}][name]");

                            if (!string.IsNullOrEmpty(t.Description))
                                form.Add(new StringContent(t.Description), $"translations[{lang}][description]");

                            if (!string.IsNullOrEmpty(t.MetadataTitle))
                                form.Add(new StringContent(t.MetadataTitle), $"translations[{lang}][metadata_title]");

                            if (!string.IsNullOrEmpty(t.MetadataDescription))
                                form.Add(new StringContent(t.MetadataDescription), $"translations[{lang}][metadata_description]");

                            if (!string.IsNullOrEmpty(t.MetadataUrl))
                                form.Add(new StringContent(t.MetadataUrl), $"translations[{lang}][metadata_url]");
                        }
                    }
                }

                var result = await client.PostAsync($"{BaseUrl}/brands", form);
                var content = await result.Content.ReadAsStringAsync();

                Console.WriteLine($"CreateBrand Status: {result.StatusCode}");
                Console.WriteLine($"CreateBrand Response: {content}");

                return JsonConvert.DeserializeObject<SallaBaseResponse<BrandResponseDto>>(content)
                    ?? new SallaBaseResponse<BrandResponseDto> { Status = (int)result.StatusCode };
            }
            catch (Exception e)
            {
                Console.WriteLine($"CreateBrand Exception: {e}");
                return CreateExceptionResponse<SallaBaseResponse<BrandResponseDto>>(e.Message);
            }
        }

        public async Task<SallaBaseResponse<BrandResponseDto>> GetBrandDetailsAsync(int brandId, string? with, string token)
        {
            try
            {
                var client = CreateDefaultClient(token);

                var queryParams = new List<string>();

                if (!string.IsNullOrEmpty(with))
                    queryParams.Add($"with={Uri.EscapeDataString(with)}");

                var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
                var result = await client.GetAsync($"{BaseUrl}/brands/{brandId}");
                var content = await result.Content.ReadAsStringAsync();

                Console.WriteLine($"ListBrands Status: {result.StatusCode}");
                Console.WriteLine($"ListBrands Response: {content}");

                return JsonConvert.DeserializeObject<SallaBaseResponse<BrandResponseDto>>(content)
                    ?? new SallaBaseResponse<BrandResponseDto> { Status = (int)result.StatusCode };
            }
            catch (Exception e)
            {
                Console.WriteLine($"ListBrands Exception: {e}");
                return CreateExceptionResponse<SallaBaseResponse<BrandResponseDto>>(e.Message);
            }
        }

        public async Task<SallaBaseResponse<BrandResponseDto>> UpdateBrandAsync(UpdateBrandDto dto, int brandId, string token)
        {
            try
            {
                var client = CreateDefaultClient(token);

                using var form = new MultipartFormDataContent();

                form.Add(new StringContent(dto.Name), "name");

                if (!string.IsNullOrEmpty(dto.Description))
                    form.Add(new StringContent(dto.Description), "description");

                if (!string.IsNullOrEmpty(dto.MetadataTitle))
                    form.Add(new StringContent(dto.MetadataTitle), "metadata[title]");

                if (!string.IsNullOrEmpty(dto.MetadataDescription))
                    form.Add(new StringContent(dto.MetadataDescription), "metadata[description]");

                if (!string.IsNullOrEmpty(dto.MetadataUrl))
                    form.Add(new StringContent(dto.MetadataUrl), "metadata[url]");

                var logoStream = dto.Logo.OpenReadStream();
                var logoContent = new StreamContent(logoStream);
                logoContent.Headers.ContentType = new MediaTypeHeaderValue(dto.Logo.ContentType);
                form.Add(logoContent, "logo", dto.Logo.FileName);

                if (dto.Banner != null)
                {
                    var bannerStream = dto.Banner.OpenReadStream();
                    var bannerContent = new StreamContent(bannerStream);
                    bannerContent.Headers.ContentType = new MediaTypeHeaderValue(dto.Banner.ContentType);
                    form.Add(bannerContent, "banner", dto.Banner.FileName);
                }

                if (!string.IsNullOrEmpty(dto.TranslationsJson))
                {
                    var translations = JsonConvert.DeserializeObject<List<TranslationDto>>(dto.TranslationsJson);
                    if (translations != null)
                    {
                        foreach (var t in translations)
                        {
                            var lang = t.Locale;

                            if (!string.IsNullOrEmpty(t.Name))
                                form.Add(new StringContent(t.Name), $"translations[{lang}][name]");

                            if (!string.IsNullOrEmpty(t.Description))
                                form.Add(new StringContent(t.Description), $"translations[{lang}][description]");

                            if (!string.IsNullOrEmpty(t.MetadataTitle))
                                form.Add(new StringContent(t.MetadataTitle), $"translations[{lang}][metadata_title]");

                            if (!string.IsNullOrEmpty(t.MetadataDescription))
                                form.Add(new StringContent(t.MetadataDescription), $"translations[{lang}][metadata_description]");

                            if (!string.IsNullOrEmpty(t.MetadataUrl))
                                form.Add(new StringContent(t.MetadataUrl), $"translations[{lang}][metadata_url]");
                        }
                    }
                }

                var result = await client.PutAsync($"{BaseUrl}/brands/{brandId}", form);
                var content = await result.Content.ReadAsStringAsync();

                Console.WriteLine($"UpdateBrand Status: {result.StatusCode}");
                Console.WriteLine($"UpdateBrand Response: {content}");

                return JsonConvert.DeserializeObject<SallaBaseResponse<BrandResponseDto>>(content)
                    ?? new SallaBaseResponse<BrandResponseDto> { Status = (int)result.StatusCode };
            }
            catch (Exception e)
            {
                Console.WriteLine($"UpdateBrand Exception: {e}");
                return CreateExceptionResponse<SallaBaseResponse<BrandResponseDto>>(e.Message);
            }
        }

        public async Task<SallaBaseResponse<ChangeStatusResponseDto>> DeleteBrandAsync(int brandId, string token)
        {
            try
            {
                var client = CreateDefaultClient(token);
                var result = await client.DeleteAsync($"{BaseUrl}/brands/{brandId}");
                var content = await result.Content.ReadAsStringAsync();
                Console.WriteLine($"DeleteBrand Status: {result.StatusCode}");
                Console.WriteLine($"DeleteBrand Response: {content}");
                return JsonConvert.DeserializeObject<SallaBaseResponse<ChangeStatusResponseDto>>(content)
                    ?? new SallaBaseResponse<ChangeStatusResponseDto> { Status = (int)result.StatusCode };

            }
            catch (Exception e)
            {
                Console.WriteLine($"DeleteBrand Exception: {e}");
                return CreateExceptionResponse<SallaBaseResponse<ChangeStatusResponseDto>>(e.Message);
            }
        }
    }
}
