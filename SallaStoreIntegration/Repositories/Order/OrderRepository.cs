using FluentValidation;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SallaStoreIntegration.Dtos.OrderInvoice;
using SallaStoreIntegration.Dtos.Orders;
using SallaStoreIntegration.Dtos.Response;
using SallaStoreIntegration.Repositories.Base;
using SallaStoreIntegration.Setting;
using SallaStoreIntegration.Validation;
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

        // Direct port of WordPress OrdersBLL.GetAllPendingOrders — paginates Salla until empty.
        // Salla expects status as an array of integer IDs: ?status[]=123&status[]=456
        public async Task<List<OrderResultDto>> GetAllOrdersByStatusAsync(string token, List<int> statusIds, CancellationToken ct = default)
        {
            var all = new List<OrderResultDto>();
            var page = 1;
            const int perPage = 50;

            try
            {
                var client = CreateDefaultClient(token);

                var statusQuery = "";
                if (statusIds != null && statusIds.Count > 0)
                    statusQuery = "&" + string.Join("&", statusIds.Select(id => $"status[]={id}"));

                while (true)
                {
                    ct.ThrowIfCancellationRequested();

                    var url = $"{BaseUrl}/orders?page={page}&per_page={perPage}{statusQuery}";

                    var result = await client.GetAsync(url, ct);
                    var content = await result.Content.ReadAsStringAsync(ct);
                    if (!result.IsSuccessStatusCode) break;

                    var json = JObject.Parse(content);
                    var dataNode = json["data"];
                    if (dataNode == null) break;

                    var items = JsonConvert.DeserializeObject<List<OrderResultDto>>(dataNode.ToString());
                    if (items == null || items.Count == 0) break;

                    all.AddRange(items);

                    var pagination = json["pagination"];
                    var totalPages = pagination?["totalPages"]?.Value<int?>() ?? pagination?["total_pages"]?.Value<int?>();
                    if (totalPages.HasValue && page >= totalPages.Value) break;
                    if (items.Count < perPage) break;

                    page++;
                }
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch
            {
                // swallow; return what we have
            }

            return all;
        }

        // GET /orders/{id} returns full details (items, shipments, pickup branch, customer groups) by default.
        // Returning JObject because we don't yet have a typed schema for items/shipments.
        public async Task<JObject> GetOrderDetailsRawAsync(long orderId, string token, CancellationToken ct = default)
        {
            try
            {
                var client = CreateDefaultClient(token);
                var result = await client.GetAsync($"{BaseUrl}/orders/{orderId}", ct);
                var content = await result.Content.ReadAsStringAsync(ct);
                if (!result.IsSuccessStatusCode) return new JObject();

                var json = JObject.Parse(content);
                var data = json["data"] as JObject;
                return data ?? json;
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch
            {
                return new JObject();
            }
        }

        // Option-B orchestrator: in-memory snapshot of pending orders.
        // Mirrors WordPress's GetListAllOrdersPendingAndAddToOffline, minus the DB writes.
        // List + per-order detail enrichment so caller has line items to build invoices.
        public async Task<PendingOrdersBatchDto> PullPendingOrdersAsync(string token, List<int> statusIds, bool includeDetails = true, CancellationToken ct = default)
        {
            var batch = new PendingOrdersBatchDto { StatusIds = statusIds ?? new List<int>() };
            try
            {
                var orders = await GetAllOrdersByStatusAsync(token, statusIds, ct);
                batch.Orders = orders;
                batch.Count = orders.Count;

                if (includeDetails)
                {
                    foreach (var order in orders)
                    {
                        ct.ThrowIfCancellationRequested();
                        var detail = await GetOrderDetailsRawAsync(order.Id, token, ct);
                        if (detail != null && detail.HasValues)
                            batch.Details[order.Id] = detail;
                    }
                }
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception e)
            {
                batch.Message = e.Message;
            }
            return batch;
        }

        public async Task<SallaBaseResponse<CreateOrderResponseDto>> CreateOrderAsync(CreateOrderRequestDto request, string token)
        {
            var validationResult = await new CreateOrderValidator().ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

            try
            {
                var client = CreateDefaultClient(token);

                var json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var result = await client.PostAsync($"{BaseUrl}/orders", httpContent);
                var content = await result.Content.ReadAsStringAsync();

                Console.WriteLine($"CreateOrder Status: {result.StatusCode}");
                Console.WriteLine($"CreateOrder Response: {content}");

                return JsonConvert.DeserializeObject<SallaBaseResponse<CreateOrderResponseDto>>(content)
                    ?? new SallaBaseResponse<CreateOrderResponseDto> { Status = (int)result.StatusCode };
            }
            catch (Exception e)
            {
                Console.WriteLine($"CreateOrder Exception: {e}");
                return CreateExceptionResponse<SallaBaseResponse<CreateOrderResponseDto>>(e.Message);
            }
        }

        public async Task<SallaBaseResponse<RelocateOrderStockResponseDto>> RelocateOrderStockAsync(RelocateOrderStockRequestDto request, long orderId, string token)
        {
            var validationResult = await new RelocateOrderStockValidator().ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

            try
            {
                var client = CreateDefaultClient(token);

                var json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var result = await client.PostAsync($"{BaseUrl}/orders/{orderId}/inventory/relocate", httpContent);
                var content = await result.Content.ReadAsStringAsync();

                Console.WriteLine($"RelocateOrderStock Status: {result.StatusCode}");
                Console.WriteLine($"RelocateOrderStock Response: {content}");

                return JsonConvert.DeserializeObject<SallaBaseResponse<RelocateOrderStockResponseDto>>(content)
                    ?? new SallaBaseResponse<RelocateOrderStockResponseDto> { Status = (int)result.StatusCode };
            }
            catch (Exception e)
            {
                Console.WriteLine($"RelocateOrderStock Exception: {e}");
                return CreateExceptionResponse<SallaBaseResponse<RelocateOrderStockResponseDto>>(e.Message);
            }
        }

        public async Task<SallaBaseResponse<BulkOrdersStatusesResponseDto>> UpdateBulkOrdersStatusesAsync(UpdateBulkOrdersStatusesFormDto form, string token)
        {
            var validationResult = await new UpdateBulkOrdersStatusesValidator().ValidateAsync(form);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

            try
            {
                var client = CreateDefaultClient(token);

                using var formData = new MultipartFormDataContent();
                using var stream = form.File.OpenReadStream();
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                formData.Add(fileContent, "file", form.File.FileName);

                var result = await client.PostAsync($"{BaseUrl}/orders/statuses/bulk", formData);
                var content = await result.Content.ReadAsStringAsync();

                Console.WriteLine($"UpdateBulkOrdersStatuses Status: {result.StatusCode}");
                Console.WriteLine($"UpdateBulkOrdersStatuses Response: {content}");

                return JsonConvert.DeserializeObject<SallaBaseResponse<BulkOrdersStatusesResponseDto>>(content)
                    ?? new SallaBaseResponse<BulkOrdersStatusesResponseDto> { Status = (int)result.StatusCode };
            }
            catch (Exception e)
            {
                Console.WriteLine($"UpdateBulkOrdersStatuses Exception: {e}");
                return CreateExceptionResponse<SallaBaseResponse<BulkOrdersStatusesResponseDto>>(e.Message);
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

        public async Task<SallaBaseResponse<OrderInvoiceResponseDto>> CreateOrderInvoiceAsync(long orderId, string token)
        {
            try
            {
                var client = CreateDefaultClient(token);
                var result = await client.PostAsync($"{BaseUrl}/orders/{orderId}/print-invoice", null);
                var content = await result.Content.ReadAsStringAsync();

                Console.WriteLine($"CreateOrderInvoice Status: {result.StatusCode}");
                Console.WriteLine($"CreateOrderInvoice Response: {content}");

                return JsonConvert.DeserializeObject<SallaBaseResponse<OrderInvoiceResponseDto>>(content)
                    ?? new SallaBaseResponse<OrderInvoiceResponseDto> { Status = (int)result.StatusCode };
            }
            catch (Exception e)
            {
                Console.WriteLine($"CreateOrderInvoice Exception: {e}");
                return CreateExceptionResponse<SallaBaseResponse<OrderInvoiceResponseDto>>(e.Message);
            }
        }
        public async Task<SallaBaseResponse<List<InvoiceDto>>> ListInvoicesAsync( string? fromDate, string? toDate, int? orderId, string token)
        {
            try
            {
                var client = CreateDefaultClient(token);

                var query = new List<string>();
                if (!string.IsNullOrEmpty(fromDate)) query.Add($"from_date={fromDate}");
                if (!string.IsNullOrEmpty(toDate)) query.Add($"to_date={toDate}");
                if (orderId.HasValue) query.Add($"order_id={orderId}");

                var url = $"{BaseUrl}/orders/invoices";
                if (query.Count > 0)
                    url += "?" + string.Join("&", query);

                var result = await client.GetAsync(url);
                var content = await result.Content.ReadAsStringAsync();

                Console.WriteLine($"ListInvoices Status: {result.StatusCode}");
                Console.WriteLine($"ListInvoices Response: {content}");

                return JsonConvert.DeserializeObject<SallaBaseResponse<List<InvoiceDto>>>(content)
                    ?? new SallaBaseResponse<List<InvoiceDto>> { Status = (int)result.StatusCode };
            }
            catch (Exception e)
            {
                Console.WriteLine($"ListInvoices Exception: {e}");
                return CreateExceptionResponse<SallaBaseResponse<List<InvoiceDto>>>(e.Message);
            }
        }

        public async Task<SallaBaseResponse<InvoiceDetailsDto>> GetInvoiceDetailsAsync(long invoiceId, string token)
        {
            try
            {
                var client = CreateDefaultClient(token);
                var result = await client.GetAsync($"{BaseUrl}/orders/invoices/{invoiceId}");
                var content = await result.Content.ReadAsStringAsync();

                Console.WriteLine($"GetInvoiceDetails Status: {result.StatusCode}");
                Console.WriteLine($"GetInvoiceDetails Response: {content}");

                return JsonConvert.DeserializeObject<SallaBaseResponse<InvoiceDetailsDto>>(content)
                    ?? new SallaBaseResponse<InvoiceDetailsDto> { Status = (int)result.StatusCode };
            }
            catch (Exception e)
            {
                Console.WriteLine($"GetInvoiceDetails Exception: {e}");
                return CreateExceptionResponse<SallaBaseResponse<InvoiceDetailsDto>>(e.Message);
            }
        }
        public async Task<SallaBaseResponse<CreateInvoiceResponseDto>> CreateInvoiceAsync(CreateInvoiceDto dto,string token)
        {
            try
            {
                var client = CreateDefaultClient(token);

                var json = JsonConvert.SerializeObject(dto);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                var result = await client.PostAsync(
                    $"{BaseUrl}/orders/invoices",
                    content);

                var responseContent = await result.Content.ReadAsStringAsync();

                Console.WriteLine($"CreateInvoice Status: {result.StatusCode}");
                Console.WriteLine($"CreateInvoice Response: {responseContent}");

                return JsonConvert.DeserializeObject<SallaBaseResponse<CreateInvoiceResponseDto>>(responseContent)
                       ?? new SallaBaseResponse<CreateInvoiceResponseDto>
                       {
                           Status = (int)result.StatusCode
                       };
            }
            catch (Exception e)
            {
                Console.WriteLine($"CreateInvoice Exception: {e}");

                return CreateExceptionResponse<SallaBaseResponse<CreateInvoiceResponseDto>>(e.Message);
            }
        }
    }
}
