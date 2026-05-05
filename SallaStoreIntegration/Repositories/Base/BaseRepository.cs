using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using SallaStoreIntegration.Setting;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SallaStoreIntegration.Repositories.Base
{
    public abstract class BaseRepository
    {
        protected const string BaseUrl = "https://api.salla.dev/admin/v2";
        protected readonly IHttpClientFactory _httpClientFactory;
        protected readonly ExternalStoresSetting _settings;

        protected BaseRepository(IHttpClientFactory httpClientFactory, IOptions<ExternalStoresSetting> settings)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings.Value;
        }

        protected HttpClient CreateClient(string token = null)
        {
            var client = _httpClientFactory.CreateClient(ExternalStoresProviderEnum.Salla.ToString());
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        //protected HttpClient CreateDefaultClient(string token = null)
        //{
        //    var client = _httpClientFactory.CreateClient();
        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    }
        //    return client;
        //}
        protected HttpClient CreateDefaultClient(string token = null)
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(2); // ? add this
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        // Salla wraps successful single-resource responses as { "status":..., "success":..., "data":{...} }.
        // Unwrap "data" when present; otherwise deserialize the full body (keeps non-wrapped endpoints working).
        private static readonly JsonSerializerSettings _deserializeSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<Newtonsoft.Json.JsonConverter> { new StringEnumConverter() }
        };

        protected T DeserializeUnwrap<T>(string content) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(content)) return new T();
            try
            {
                var token = JToken.Parse(content);
                if (token.Type == JTokenType.Object)
                {
                    var dataNode = token["data"];
                    if (dataNode != null && dataNode.Type != JTokenType.Null)
                    {
                        return JsonConvert.DeserializeObject<T>(dataNode.ToString(), _deserializeSettings) ?? new T();
                    }
                }
            }
            catch
            {
                // not JSON or no data wrapper — fall through
            }
            return JsonConvert.DeserializeObject<T>(content, _deserializeSettings) ?? new T();
        }

        protected async Task<T> GetAsync<T>(string endpoint, string token) where T : class, new()
        {
            try
            {
                var client = CreateDefaultClient(token);
                var result = await client.GetAsync($"{BaseUrl}/{endpoint}");
                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                    return DeserializeUnwrap<T>(content);

                return CreateErrorResponse<T>(content, result.StatusCode);
            }
            catch (Exception e)
            {
                return CreateExceptionResponse<T>(e.Message);
            }
        }

        protected async Task<List<T>> GetListAsync<T>(string endpoint, string token) where T : class, new()
        {
            try
            {
                var client = CreateDefaultClient(token);
                var result = await client.GetAsync($"{BaseUrl}/{endpoint}");
                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    var resultJson = JObject.Parse(content);
                    var dataNode = resultJson["data"];
                    var json = dataNode != null ? dataNode.ToString() : content;
                    return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
                }

                return new List<T> { CreateErrorResponse<T>(content, result.StatusCode) };
            }
            catch (Exception e)
            {
                return new List<T> { CreateExceptionResponse<T>(e.Message) };
            }
        }

        protected async Task<List<T>> GetAllPagesAsync<T>(string endpoint, string token, int perPage = 50) where T : class, new()
        {
            var all = new List<T>();
            try
            {
                var client = CreateDefaultClient(token);
                var page = 1;
                while (true)
                {
                    var sep = endpoint.Contains("?") ? "&" : "?";
                    var url = $"{BaseUrl}/{endpoint}{sep}page={page}&per_page={perPage}";
                    var result = await client.GetAsync(url);
                    var content = await result.Content.ReadAsStringAsync();
                    if (!result.IsSuccessStatusCode) break;

                    var json = JObject.Parse(content);
                    var dataNode = json["data"];
                    if (dataNode == null) break;
                    var items = JsonConvert.DeserializeObject<List<T>>(dataNode.ToString());
                    if (items == null || items.Count == 0) break;

                    all.AddRange(items);

                    var pagination = json["pagination"];
                    var totalPages = pagination?["totalPages"]?.Value<int?>() ?? pagination?["total_pages"]?.Value<int?>();
                    if (totalPages.HasValue && page >= totalPages.Value) break;
                    if (items.Count < perPage) break;

                    page++;
                }
            }
            catch
            {
                // swallow; return what we have
            }
            return all;
        }

        protected async Task<T> PostAsync<T, TRequest>(string endpoint, TRequest request, string token) where T : class, new()
        {
            try
            {
                var client = CreateDefaultClient(token);
                var json = JsonConvert.SerializeObject(request);
               
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var result = await client.PostAsync($"{BaseUrl}/{endpoint}", httpContent);
                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                    return DeserializeUnwrap<T>(content);

                return CreateErrorResponse<T>(content, result.StatusCode);
            }
            catch (Exception e)
            {
                return CreateExceptionResponse<T>(e.Message);
            }
        }

        protected async Task<bool> PostBoolAsync<TRequest>(string endpoint, TRequest request, string token)
        {
            try
            {
                var client = CreateClient(token);
                var json = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var result = await client.PostAsync(endpoint, httpContent);
                return result.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        protected async Task<T> PutAsync<T, TRequest>(string endpoint, TRequest request, string token) where T : class, new()
        {
            try
            {
                var client = CreateDefaultClient(token);

                // Serialize with Newtonsoft
                var json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = new List<Newtonsoft.Json.JsonConverter> { new StringEnumConverter() }
                });

                var content = new StringContent(json, new UTF8Encoding(false), "application/json");

                var result = await client.PutAsync($"{BaseUrl}/{endpoint}", content);
                var responseContent = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                    return DeserializeUnwrap<T>(responseContent);

                return CreateErrorResponse<T>(responseContent, result.StatusCode);
            }
            catch (Exception e)
            {
                return CreateExceptionResponse<T>(e.Message);
            }
        }

        protected async Task<bool> PutBoolAsync<TRequest>(string endpoint, TRequest request, string token)
        {
            try
            {
                var client = CreateClient(token);
                var json = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var result = await client.PutAsync(endpoint, httpContent);
                return result.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        protected async Task<T> DeleteAsync<T>(string endpoint, string token) where T : class, new()
        {
            try
            {
                var client = CreateDefaultClient(token);
                var result = await client.DeleteAsync($"{BaseUrl}/{endpoint}");
                var content = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                    return DeserializeUnwrap<T>(content);

                return CreateErrorResponse<T>(content, result.StatusCode);
            }
            catch (Exception e)
            {
                return CreateExceptionResponse<T>(e.Message);
            }
        }
        protected async Task<bool> BoolDeleteAsync(string endpoint, string token)
        {
            try
            {
                var client = CreateDefaultClient(token);
                var result = await client.DeleteAsync($"{BaseUrl}/{endpoint}");
                return result.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        protected string GetErrorMessage(string content, System.Net.HttpStatusCode statusCode)
        {
            try
            {
                var error = JObject.Parse(content);
                return error["error"]?["message"]?.ToString() ?? statusCode.ToString();
            }
            catch
            {
                return statusCode.ToString();
            }
        }

        public T CreateErrorResponse<T>(string content, System.Net.HttpStatusCode statusCode) where T : class, new()
        {
            var instance = new T();
            var messageProperty = typeof(T).GetProperty("Message");
            if (messageProperty != null)
            {
                messageProperty.SetValue(instance, GetErrorMessage(content, statusCode));
            }
            return instance;
        }

        public T CreateExceptionResponse<T>(string message) where T : class, new()
        {
            var instance = new T();
            var messageProperty = typeof(T).GetProperty("Message");
            if (messageProperty != null)
            {
                messageProperty.SetValue(instance, message);
            }
            return instance;
        }
    }
}
