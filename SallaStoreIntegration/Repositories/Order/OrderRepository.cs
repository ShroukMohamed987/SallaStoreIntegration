using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SallaStoreIntegration.Dtos.Orders;
using SallaStoreIntegration.Repositories.Base;
using SallaStoreIntegration.Setting;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SallaStoreIntegration.Repositories.Order
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        private readonly HttpClient _httpClient;

        public OrderRepository(
            IHttpClientFactory httpClientFactory,
            IOptions<ExternalStoresSetting> settings,
            HttpClient httpClient)
            : base(httpClientFactory, settings)
        {
            _httpClient = httpClient;
        }

        public async Task<List<OrderResultDto>> GetOrdersAsync(string token, ListOrdersFilterDto filter = null)
        {
            try
            {
                var client = CreateDefaultClient(token);
                var query = "";

                if (filter != null)
                {
                    var parameters = new List<string>();
                    if (!string.IsNullOrEmpty(filter.Keyword)) parameters.Add($"keyword={filter.Keyword}");
                    if (!string.IsNullOrEmpty(filter.PaymentMethod)) parameters.Add($"payment_method={filter.PaymentMethod}");
                    if (!string.IsNullOrEmpty(filter.FromDate)) parameters.Add($"from_date={filter.FromDate}");
                    if (!string.IsNullOrEmpty(filter.ToDate)) parameters.Add($"to_date={filter.ToDate}");
                    if (filter.Country.HasValue) parameters.Add($"country={filter.Country}");
                    if (!string.IsNullOrEmpty(filter.City)) parameters.Add($"city={filter.City}");
                    if (filter.Page.HasValue) parameters.Add($"page={filter.Page}");
                    if (!string.IsNullOrEmpty(filter.SortBy)) parameters.Add($"sort_by={filter.SortBy}");
                    if (parameters.Any()) query = "?" + string.Join("&", parameters);
                }

                var result = await client.GetAsync($"{BaseUrl}/orders{query}");
                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    var json = JObject.Parse(content);
                    return JsonConvert.DeserializeObject<List<OrderResultDto>>(json["data"].ToString());
                }

                return new List<OrderResultDto> { new OrderResultDto { Message = GetErrorMessage(content, result.StatusCode) } };
            }
            catch (Exception e)
            {
                return new List<OrderResultDto> { new OrderResultDto { Message = e.Message } };
            }
        }

        public async Task<OrderResultDto> GetOrderAsync(long orderId, string token)
        {
            try
            {
                var client = CreateDefaultClient(token);
                var result = await client.GetAsync($"{BaseUrl}/orders/{orderId}");
                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    var json = JObject.Parse(content);
                    return JsonConvert.DeserializeObject<OrderResultDto>(json["data"].ToString());
                }

                return new OrderResultDto { Message = GetErrorMessage(content, result.StatusCode) };
            }
            catch (Exception e)
            {
                return new OrderResultDto { Message = e.Message };
            }
        }

        public async Task<bool> UpdateOrderStatusAsync(string token, int id, UpdateOrderStatusInputDto inputDto)
        {
            try
            {
                var client = CreateClient(token);
                string data = JsonConvert.SerializeObject(inputDto);
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage result = await client.PostAsync($"orders/{id}/status", content);
                return result.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<StatusResultDto>> GetOrderStatusAsync(string token)
        {
            try
            {
                var client = CreateClient(token);
                HttpResponseMessage result = await client.GetAsync("orders/statuses");

                if (result.IsSuccessStatusCode)
                {
                    string content = await result.Content.ReadAsStringAsync();
                    var resultJson = JObject.Parse(content);
                    return JsonConvert.DeserializeObject<List<StatusResultDto>>(resultJson["data"].ToString());
                }

                return new List<StatusResultDto> { new StatusResultDto { ErrorMessage = "Invalid Token" } };
            }
            catch (Exception e)
            {
                return new List<StatusResultDto> { new StatusResultDto { ErrorMessage = e.Message } };
            }
        }

        public async Task<string> ExecuteOrderActionsAsync(OrderActionsRequestDto request, string token)
        {
            request = new OrderActionsRequestDto
            {
                Operations = new List<OperationDto>
                {
                    new OperationDto
                    {
                        ActionName = "change_status",
                        Value = new { status = 123456 }
                    }
                },
                Filters = new FiltersDto
                {
                    OrderIds = new List<long> { 123456789 }
                }
            };

            var url = "https://api.salla.dev/admin/v2/orders/actions";
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            httpRequest.Content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(httpRequest);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Salla Error: {content}");

            return content;
        }
    }
}
